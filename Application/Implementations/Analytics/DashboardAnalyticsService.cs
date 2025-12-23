using Application.Interfaces.Analytics;
using Application.Interfaces.CORE;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using Domain.Interfaces.FIN;
using Domain.Interfaces.IDN;
using DTOs.Analytics.Dashboard.Requests;
using DTOs.Analytics.Dashboard.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Application.Implementations.Analytics;

/// <summary>
/// Service implementation for dashboard analytics
/// </summary>
public class DashboardAnalyticsService : IDashboardAnalyticsService
{
    private readonly IACAD_EnrollmentRepository _enrollmentRepository;
    private readonly IACAD_CourseRepository _courseRepository;
    private readonly IACAD_ClassRepository _classRepository;
    private readonly IIDN_StudentRepository _studentRepository;
    private readonly IFIN_PaymentRepository _paymentRepository;
    private readonly ICORE_LookUpService _lookUpService;
    private readonly ILogger<DashboardAnalyticsService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public DashboardAnalyticsService(
        IACAD_EnrollmentRepository enrollmentRepository,
        IACAD_CourseRepository courseRepository,
        IACAD_ClassRepository classRepository,
        IIDN_StudentRepository studentRepository,
        IFIN_PaymentRepository paymentRepository,
        ICORE_LookUpService lookUpService,
        ILogger<DashboardAnalyticsService> logger,
        IConfiguration configuration)
    {
        _enrollmentRepository = enrollmentRepository;
        _courseRepository = courseRepository;
        _classRepository = classRepository;
        _studentRepository = studentRepository;
        _paymentRepository = paymentRepository;
        _lookUpService = lookUpService;
        _logger = logger;
        _httpClient = new HttpClient();
        _configuration = configuration;
    }

    public async Task<RevenueAnalyticsResponse> GetRevenueAnalyticsAsync()
    {
        try
        {
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;
            var currentQuarter = (currentMonth - 1) / 3 + 1;

            // Get all payments with Completed status
            var allPayments = await _paymentRepository.GetAllAsync();
            var payments = allPayments.ToList();

            var response = new RevenueAnalyticsResponse
            {
                Monthly = CalculateMonthlyRevenue(payments, currentYear),
                Quarterly = CalculateQuarterlyRevenue(payments, currentYear),
                Yearly = CalculateYearlyRevenue(payments),
            };

            // Calculate current period values
            response.CurrentMonth = response.Monthly
                .FirstOrDefault(m => m.Period == currentDate.ToString("MMM"))?.Revenue ?? 0;
            
            response.CurrentQuarter = response.Quarterly
                .FirstOrDefault(q => q.Period == $"Q{currentQuarter} {currentYear}")?.Revenue ?? 0;
            
            response.CurrentYear = response.Yearly
                .FirstOrDefault(y => y.Period == currentYear.ToString())?.Revenue ?? 0;

            // Project next month based on average growth
            var monthsWithGrowth = response.Monthly.Where(m => m.Growth != 0).ToList();
            var avgGrowth = monthsWithGrowth.Any() ? monthsWithGrowth.Average(m => m.Growth) : 0;
            response.ProjectedNextMonth = response.CurrentMonth * (1 + (avgGrowth / 100));

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating revenue analytics");
            throw;
        }
    }

    public async Task<CourseEnrollmentStatsResponse> GetTopEnrolledCoursesAsync(TopCoursesRequest request)
    {
        try
        {
            var allCourses = await _courseRepository.GetAllAsync();
            var courses = allCourses.Where(c => !c.IsDeleted).ToList();
            
            var allEnrollments = await _enrollmentRepository.GetAllEnrollment();
            var enrollments = allEnrollments.ToList();

            // Filter by date if specified
            if (request.FromDate.HasValue)
                enrollments = enrollments.Where(e => e.CreatedAt >= request.FromDate.Value).ToList();
            
            if (request.ToDate.HasValue)
                enrollments = enrollments.Where(e => e.CreatedAt <= request.ToDate.Value).ToList();

            // Calculate metrics per course
            var courseStats = courses.Select(course =>
            {
                var courseEnrollments = enrollments.Where(e => e.CourseID == course.Id).ToList();
                var activeEnrollments = courseEnrollments.Count(e => 
                    e.EnrollmentStatus.Code != "Refunded" && 
                    e.EnrollmentStatus.Code != "Dropped");
            
                // Calculate growth rate (compare last 30 days to previous 30 days)
                var last30Days = courseEnrollments.Count(e => e.CreatedAt >= DateTime.Now.AddDays(-30));
                var previous30Days = courseEnrollments.Count(e => 
                    e.CreatedAt >= DateTime.Now.AddDays(-60) && 
                    e.CreatedAt < DateTime.Now.AddDays(-30));
                
                var growthRate = previous30Days > 0 
                    ? ((last30Days - previous30Days) / (decimal)previous30Days * 100) 
                    : 0;

                return new TopCourseResponse
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    CourseCode = course.CourseCode,
                    Category = course.Category?.Name ?? "Uncategorized",
                    TotalEnrollments = courseEnrollments.Count,
                    ActiveEnrollments = activeEnrollments,
                    CompletionRate = 0, // Removed completion rate calculation
                    AverageRating = course.AverageRating ?? 0,
                    Revenue = CalculateCourseRevenue(course.Id, enrollments),
                    Trend = growthRate > 5 ? "up" : growthRate < -5 ? "down" : "stable",
                    GrowthRate = growthRate
                };
            }).ToList();

            // Filter by category if specified
            if (!string.IsNullOrEmpty(request.CategoryFilter))
                courseStats = courseStats.Where(c => c.Category == request.CategoryFilter).ToList();

            // Sort by requested criteria
            courseStats = request.SortBy.ToLower() switch
            {
                "revenue" => courseStats.OrderByDescending(c => c.Revenue).ToList(),
                "rating" => courseStats.OrderByDescending(c => c.AverageRating).ToList(),
                "growth" => courseStats.OrderByDescending(c => c.GrowthRate).ToList(),
                _ => courseStats.OrderByDescending(c => c.TotalEnrollments).ToList(),
            };

            return new CourseEnrollmentStatsResponse
            {
                TopCourses = courseStats.Take(request.TopN).ToList(),
                TotalCourses = courses.Count,
                TotalEnrollments = enrollments.Count,
                AverageEnrollmentPerCourse = courses.Any() 
                    ? enrollments.Count / (decimal)courses.Count 
                    : 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top courses");
            throw;
        }
    }

    public async Task<StudentDropoutAnalyticsResponse> GetStudentDropoutAnalyticsAsync(DropoutAnalysisRequest request)
    {
        try
        {
            var allEnrollments = await _enrollmentRepository.GetAllEnrollment();
            var enrollments = allEnrollments.ToList();
            
            var allStudents = await _studentRepository.GetAllAsync();
            var students = allStudents.ToList();

            // Filter by date range
            if (request.FromDate.HasValue)
                enrollments = enrollments.Where(e => e.UpdatedAt >= request.FromDate.Value).ToList();
            
            if (request.ToDate.HasValue)
                enrollments = enrollments.Where(e => e.UpdatedAt <= request.ToDate.Value).ToList();

            var droppedOutEnrollments = enrollments
                .Where(e => e.EnrollmentStatus.Code == "Dropped")
                .ToList();

            var response = new StudentDropoutAnalyticsResponse
            {
                OverallDropoutRate = enrollments.Any() 
                    ? (droppedOutEnrollments.Count / (decimal)enrollments.Count * 100) 
                    : 0,
                DropoutTrend = CalculateDropoutTrend(enrollments, 12), // Last 12 months
                TopReasons = CalculateDropoutReasons(droppedOutEnrollments),
                HighRiskStudents = CalculateHighRiskStudents(enrollments),
                AverageTimeToDropout = CalculateAverageTimeToDropout(droppedOutEnrollments),
                DropoutByCourse = CalculateDropoutByCourse(enrollments)
            };

            if (request.IncludeDemographics)
            {
                response.DemographicAnalysis = CalculateDemographicAnalysis(
                    enrollments, 
                    students, 
                    request.AgeGroupFilter, 
                    request.CourseTypeFilter);
            }

            if (request.IncludeRecommendations)
            {
                response.Recommendations = GenerateDropoutRecommendations(response);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dropout analytics");
            throw;
        }
    }

    public async Task<StudentEnrollmentAnalyticsResponse> GetEnrollmentAnalyticsAsync()
    {
        try
        {
            var currentDate = DateTime.Now;
            var allEnrollments = await _enrollmentRepository.GetAllEnrollment();
            var enrollments = allEnrollments.ToList();

            var response = new StudentEnrollmentAnalyticsResponse
            {
                TotalEnrollments = enrollments.Count,
                ActiveEnrollments = enrollments.Count(e => e.EnrollmentStatus.Code == "Active"),
                CompletedEnrollments = enrollments.Count(e => e.EnrollmentStatus.Code == "Completed"),
                DroppedEnrollments = enrollments.Count(e => e.EnrollmentStatus.Code == "Dropped"),
                MonthlyTrend = CalculateMonthlyEnrollmentTrend(enrollments, 12),
                QuarterlyTrend = CalculateQuarterlyEnrollmentTrend(enrollments, 4),
                TopGrowingCourses = await CalculateTopGrowingCoursesAsync(enrollments),
                EnrollmentByCourse = CalculateEnrollmentByCourse(enrollments),
                Insights = GenerateEnrollmentInsights(enrollments)
            };

            // Calculate month-over-month growth
            var monthlyTrend = response.MonthlyTrend;
            if (monthlyTrend.Count >= 2)
            {
                var currentMonth = monthlyTrend[monthlyTrend.Count - 1].TotalEnrollments;
                var previousMonth = monthlyTrend[monthlyTrend.Count - 2].TotalEnrollments;
                response.MonthOverMonthGrowth = previousMonth > 0 
                    ? ((currentMonth - previousMonth) / (decimal)previousMonth * 100) 
                    : 0;
            }

            // Calculate quarter-over-quarter growth
            var quarterlyTrend = response.QuarterlyTrend;
            if (quarterlyTrend.Count >= 2)
            {
                var currentQuarter = quarterlyTrend[quarterlyTrend.Count - 1].TotalEnrollments;
                var previousQuarter = quarterlyTrend[quarterlyTrend.Count - 2].TotalEnrollments;
                response.QuarterOverQuarterGrowth = previousQuarter > 0 
                    ? ((currentQuarter - previousQuarter) / (decimal)previousQuarter * 100) 
                    : 0;
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting enrollment analytics");
            throw;
        }
    }

    public async Task<AIAnalysisResponse> GetAIRecommendationsAsync(AIRecommendationRequest request)
    {
        try
        {
            // Get all analytics data for AI context
            var revenueData = await GetRevenueAnalyticsAsync();
            var courseData = await GetTopEnrolledCoursesAsync(new TopCoursesRequest { TopN = 10 });
            var dropoutData = await GetStudentDropoutAnalyticsAsync(new DropoutAnalysisRequest());
            var enrollmentData = await GetEnrollmentAnalyticsAsync();

            // Call Groq AI directly (no fallback)
            var aiResponse = await CallGroqForRecommendationsAsync(revenueData, courseData, dropoutData, enrollmentData, request);
            return aiResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting AI recommendations");
            throw;
        }
    }

    private async Task<AIAnalysisResponse> CallGroqForRecommendationsAsync(
        RevenueAnalyticsResponse revenue,
        CourseEnrollmentStatsResponse courses,
        StudentDropoutAnalyticsResponse dropout,
        StudentEnrollmentAnalyticsResponse enrollment,
        AIRecommendationRequest request)
    {
        var apiKey = _configuration["GroqApi:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("Groq API Key not configured");
        }

        var model = _configuration["GroqApi:Model"] ?? "llama-3.1-70b-versatile";
        var baseUrl = _configuration["GroqApi:BaseUrl"] ?? "https://api.groq.com/openai/v1";
        string apiUrl = $"{baseUrl}/chat/completions";

        // Build comprehensive prompt with analytics data
        var prompt = BuildAIPrompt(revenue, courses, dropout, enrollment, request);

        var requestJson = new
        {
            model = model,
            messages = new[]
            {
                new {
                    role = "user",
                    content = prompt
                }
            },
            temperature = 0.7,
            max_tokens = 2048,
            response_format = new { type = "json_object" }
        };

        var jsonBody = JsonSerializer.Serialize(requestJson);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
        };
        httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");

        var response = await _httpClient.SendAsync(httpRequest);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Groq API failed: {response.StatusCode} - {responseBody}");
        }

        // Parse Groq response (OpenAI-compatible format)
        var doc = JsonDocument.Parse(responseBody);
        var textResponse = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        // Clean up markdown code blocks if present
        textResponse = textResponse?.Trim();
        if (textResponse?.StartsWith("```json") == true)
        {
            textResponse = textResponse.Substring(7);
        }
        if (textResponse?.StartsWith("```") == true)
        {
            textResponse = textResponse.Substring(3);
        }
        if (textResponse?.EndsWith("```") == true)
        {
            textResponse = textResponse.Substring(0, textResponse.Length - 3);
        }
        textResponse = textResponse?.Trim();

        // Parse AI response into our DTO
        var aiResult = JsonSerializer.Deserialize<AIAnalysisResponse>(textResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (aiResult == null)
        {
            throw new Exception("Failed to parse Groq response");
        }

        aiResult.GeneratedAt = DateTime.Now;
        return aiResult;
    }

    private string BuildAIPrompt(
        RevenueAnalyticsResponse revenue,
        CourseEnrollmentStatsResponse courses,
        StudentDropoutAnalyticsResponse dropout,
        StudentEnrollmentAnalyticsResponse enrollment,
        AIRecommendationRequest request)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("You are an expert education business analyst for an English training center in Vietnam.");
        sb.AppendLine("Analyze this data and provide strategic recommendations.");
        sb.AppendLine();
        sb.AppendLine("## CURRENT ANALYTICS DATA");
        sb.AppendLine();
        
        // Revenue data
        sb.AppendLine("### REVENUE PERFORMANCE");
        sb.AppendLine($"- Current Month Revenue: {revenue.CurrentMonth:N0} VND");
        sb.AppendLine($"- Current Quarter Revenue: {revenue.CurrentQuarter:N0} VND");
        sb.AppendLine($"- Current Year Revenue: {revenue.CurrentYear:N0} VND");
        sb.AppendLine($"- Projected Next Month: {revenue.ProjectedNextMonth:N0} VND");
        
        if (revenue.Yearly.Any())
        {
            var latestYear = revenue.Yearly.Last();
            sb.AppendLine($"- YoY Growth: {latestYear.Growth:F1}%");
        }
        
        if (revenue.Monthly.Any())
        {
            var last3Months = revenue.Monthly.TakeLast(3);
            sb.AppendLine($"- Last 3 Months Trend: {string.Join(", ", last3Months.Select(m => $"{m.Period}: {m.Revenue:N0} VND ({m.Growth:F1}% growth)"))}");
        }
        
        sb.AppendLine();
        
        // Enrollment analytics
        sb.AppendLine("### ENROLLMENT TRENDS");
        sb.AppendLine($"- Total Enrollments: {enrollment.TotalEnrollments}");
        sb.AppendLine($"- Active Enrollments: {enrollment.ActiveEnrollments}");
        sb.AppendLine($"- Completed Enrollments: {enrollment.CompletedEnrollments}");
        sb.AppendLine($"- Dropped Enrollments: {enrollment.DroppedEnrollments}");
        sb.AppendLine($"- Month-over-Month Growth: {enrollment.MonthOverMonthGrowth:F1}%");
        sb.AppendLine($"- Quarter-over-Quarter Growth: {enrollment.QuarterOverQuarterGrowth:F1}%");
        
        if (enrollment.MonthlyTrend.Any())
        {
            sb.AppendLine($"- Recent Monthly Trend: {string.Join(", ", enrollment.MonthlyTrend.TakeLast(3).Select(m => $"{m.Period}: {m.TotalEnrollments} ({m.GrowthRate:F1}% growth)"))}");
        }
        
        if (enrollment.TopGrowingCourses.Any())
        {
            sb.AppendLine("- Top Growing Courses:");
            foreach (var course in enrollment.TopGrowingCourses.Take(3))
            {
                sb.AppendLine($"  * {course.CourseName}: {course.TotalEnrollments} enrollments, {course.GrowthRate:F1}% growth");
            }
        }
        
        if (enrollment.Insights.Any())
        {
            sb.AppendLine("- Key Insights:");
            foreach (var insight in enrollment.Insights)
            {
                sb.AppendLine($"  * {insight}");
            }
        }
        
        sb.AppendLine();
        
        // Course enrollment data
        sb.AppendLine("### TOP PERFORMING COURSES");
        sb.AppendLine($"- Total Courses: {courses.TotalCourses}");
        sb.AppendLine($"- Total Enrollments: {courses.TotalEnrollments}");
        sb.AppendLine($"- Average Enrollments per Course: {courses.AverageEnrollmentPerCourse:F1}");
        sb.AppendLine();
        
        foreach (var course in courses.TopCourses.Take(5))
        {
            sb.AppendLine($"**{course.CourseName}** ({course.CourseCode})");
            sb.AppendLine($"  - Category: {course.Category}");
            sb.AppendLine($"  - Total Enrollments: {course.TotalEnrollments}");
            sb.AppendLine($"  - Active Students: {course.ActiveEnrollments}");
            sb.AppendLine($"  - Growth Rate: {course.GrowthRate:F1}% (Trend: {course.Trend})");
            sb.AppendLine($"  - Average Rating: {course.AverageRating:F1}/5");
            sb.AppendLine($"  - Revenue: {course.Revenue:N0} VND");
            sb.AppendLine();
        }
        
        // Dropout analysis
        sb.AppendLine("### STUDENT DROPOUT ANALYSIS");
        sb.AppendLine($"- Overall Dropout Rate: {dropout.OverallDropoutRate:F1}%");
        sb.AppendLine($"- High Risk Students: {dropout.HighRiskStudents}");
        sb.AppendLine($"- Average Time to Dropout: {dropout.AverageTimeToDropout} days");
        
        if (dropout.TopReasons.Any())
        {
            sb.AppendLine("- Top Dropout Reasons:");
            foreach (var reason in dropout.TopReasons.Take(3))
            {
                sb.AppendLine($"  * {reason.Reason}: {reason.Count} students ({reason.Percentage:F1}%)");
            }
        }
        
        if (dropout.DropoutTrend.Any())
        {
            sb.AppendLine($"- Recent Dropout Trend: {string.Join(", ", dropout.DropoutTrend.TakeLast(3).Select(d => $"{d.Period}: {d.DropoutRate:F1}%"))}");
        }
        
        sb.AppendLine();
        
        // Focus areas
        sb.AppendLine("## YOUR TASK");
        sb.AppendLine($"Focus Areas: {string.Join(", ", request.FocusAreas)}");
        sb.AppendLine($"Time Frame: {request.Timeframe}");
        sb.AppendLine();
        
        sb.AppendLine("Generate exactly 3 strategic recommendations based on the data above.");
        sb.AppendLine("Recommendations should be:");
        sb.AppendLine("- Data-driven and specific to the Vietnamese English training center context");
        sb.AppendLine("- Actionable with clear steps");
        sb.AppendLine("- Include realistic estimated impact numbers");
        sb.AppendLine("- Assign confidence score (0-100)");
        sb.AppendLine();
        
        sb.AppendLine("IMPORTANT RULES for estimatedImpact:");
        sb.AppendLine("1. revenue: Expected revenue increase in VND (use realistic numbers based on current revenue, e.g., 5000000 to 50000000)");
        sb.AppendLine("2. enrollments: Expected new enrollments (use positive integers, e.g., 10 to 100)");
        sb.AppendLine("3. retention: Expected retention rate improvement in percentage (e.g., 5 to 15)");
        sb.AppendLine("4. ALL THREE FIELDS MUST have numeric values (never use 0 or null)");
        sb.AppendLine("5. Match the impact to the recommendation category:");
        sb.AppendLine("   - Revenue recommendations: high revenue, moderate enrollments, low retention");
        sb.AppendLine("   - Retention recommendations: moderate revenue, moderate enrollments, high retention");
        sb.AppendLine("   - Enrollment recommendations: moderate revenue, high enrollments, moderate retention");
        sb.AppendLine();
        
        sb.AppendLine("Return response in this EXACT JSON format (no markdown, no code blocks):");
        sb.AppendLine(@"{
  ""recommendations"": [
    {
      ""id"": ""rec-1"",
      ""category"": ""revenue"",
      ""priority"": ""high"",
      ""title"": ""Optimize Pricing Strategy for High-Demand Courses"",
      ""description"": ""Based on current enrollment trends and student satisfaction scores, implement dynamic pricing for top-performing courses. Analysis shows strong demand with 85% capacity utilization."",
      ""impact"": ""Estimated 15-20% revenue increase with minimal enrollment impact"",
      ""actionItems"": [
        ""Analyze competitor pricing in Ho Chi Minh City market"",
        ""Implement A/B testing with 10-15% price increase"",
        ""Create premium packages with additional features"",
        ""Monitor enrollment rates weekly during transition""
      ],
      ""estimatedImpact"": {
        ""revenue"": 25000000,
        ""enrollments"": 15,
        ""retention"": 5
      },
      ""confidence"": 85,
      ""generatedAt"": ""2024-01-01T00:00:00""
    },
    {
      ""id"": ""rec-2"",
      ""category"": ""retention"",
      ""priority"": ""high"",
      ""title"": ""Implement Student Success Program"",
      ""description"": ""Address the 57.9% dropout rate by creating a comprehensive student support system with regular check-ins and personalized learning paths."",
      ""impact"": ""Reduce dropout rate by 30% and improve student satisfaction"",
      ""actionItems"": [
        ""Develop onboarding checklist for new students"",
        ""Assign mentors for first 30 days"",
        ""Implement automated check-ins at days 7, 14, 21"",
        ""Create peer study groups""
      ],
      ""estimatedImpact"": {
        ""revenue"": 15000000,
        ""enrollments"": 25,
        ""retention"": 12
      },
      ""confidence"": 90,
      ""generatedAt"": ""2024-01-01T00:00:00""
    },
    {
      ""id"": ""rec-3"",
      ""category"": ""enrollment"",
      ""priority"": ""medium"",
      ""title"": ""Expand High-Growth Course Categories"",
      ""description"": ""Capitalize on growing demand in business English and IELTS preparation by developing advanced level courses and corporate training packages."",
      ""impact"": ""Increase quarterly enrollments by 50-80 students"",
      ""actionItems"": [
        ""Develop advanced business English curriculum"",
        ""Create IELTS intensive preparation tracks"",
        ""Partner with local companies for corporate training"",
        ""Launch online hybrid learning options""
      ],
      ""estimatedImpact"": {
        ""revenue"": 35000000,
        ""enrollments"": 65,
        ""retention"": 8
      },
      ""confidence"": 78,
      ""generatedAt"": ""2024-01-01T00:00:00""
    }
  ],
  ""summary"": ""The English training center shows strong growth potential with opportunities to optimize pricing, improve retention, and expand course offerings. Focus on reducing the 57.9% dropout rate while capitalizing on high-demand categories."",
  ""keyInsights"": [
    ""Current enrollment growth rate indicates strong market demand"",
    ""Top-performing courses have capacity for premium pricing"",
    ""Student dropout rate requires immediate intervention"",
    ""Business English and IELTS categories show highest growth potential"",
    ""Corporate training market remains largely untapped""
  ],
  ""riskFactors"": [
    ""High dropout rate of 57.9% impacting revenue and reputation"",
    ""Over-reliance on single course category for revenue"",
    ""Increasing competition in Ho Chi Minh City market""
  ],
  ""opportunities"": [
    ""Growing corporate training demand in Vietnam"",
    ""Online and hybrid learning expansion potential"",
    ""Premium pricing opportunity for high-rated courses"",
    ""Partnership opportunities with international companies""
  ],
  ""generatedAt"": ""2024-01-01T00:00:00""
}");
        
        return sb.ToString();
    }

    private AIAnalysisResponse GenerateFallbackRecommendations(
        RevenueAnalyticsResponse revenue,
        CourseEnrollmentStatsResponse courses,
        StudentDropoutAnalyticsResponse dropout,
        StudentEnrollmentAnalyticsResponse enrollment,
        AIRecommendationRequest request)
    {
        var response = new AIAnalysisResponse
        {
            Recommendations = GenerateAIRecommendations(revenue, courses, dropout, request),
            Summary = GenerateExecutiveSummary(revenue, courses, dropout, enrollment),
            KeyInsights = GenerateKeyInsights(revenue, courses, dropout, enrollment),
            GeneratedAt = DateTime.Now
        };

        if (request.IncludeRiskAnalysis)
        {
            response.RiskFactors = GenerateRiskFactors(dropout, courses);
        }

        if (request.IncludeOpportunities)
        {
            response.Opportunities = GenerateOpportunities(revenue, courses);
        }

        return response;
    }

    #region Private Helper Methods

    private List<RevenueDataPoint> CalculateMonthlyRevenue(
        List<FIN_Payment> payments, int year)
    {
        var monthlyData = new List<RevenueDataPoint>();
        
        for (int month = 1; month <= 12; month++)
        {
            var monthPayments = payments
                .Where(p => p.PaymentDate.Year == year && p.PaymentDate.Month == month)
                .ToList();

            var revenue = monthPayments.Sum(p => (decimal)p.Amount);
            
            // Calculate growth vs previous month
            var previousMonth = month == 1 ? 12 : month - 1;
            var previousYear = month == 1 ? year - 1 : year;
            var previousMonthPayments = payments
                .Where(p => p.PaymentDate.Year == previousYear && p.PaymentDate.Month == previousMonth)
                .ToList();
            var previousRevenue = previousMonthPayments.Sum(p => (decimal)p.Amount);
            
            var growth = previousRevenue > 0 
                ? ((revenue - previousRevenue) / previousRevenue * 100) 
                : 0;

            monthlyData.Add(new RevenueDataPoint
            {
                Period = new DateTime(year, month, 1).ToString("MMM"),
                Revenue = revenue,
                Growth = growth,
                TransactionCount = monthPayments.Count
            });
        }

        return monthlyData;
    }

    private List<RevenueDataPoint> CalculateQuarterlyRevenue(
        List<FIN_Payment> payments, int year)
    {
        var quarterlyData = new List<RevenueDataPoint>();
        
        for (int quarter = 1; quarter <= 4; quarter++)
        {
            var startMonth = (quarter - 1) * 3 + 1;
            var endMonth = quarter * 3;
            
            var quarterPayments = payments
                .Where(p => p.PaymentDate.Year == year && 
                           p.PaymentDate.Month >= startMonth && 
                           p.PaymentDate.Month <= endMonth)
                .ToList();

            var revenue = quarterPayments.Sum(p => (decimal)p.Amount);
            
            // Calculate growth vs previous quarter
            var previousQuarter = quarter == 1 ? 4 : quarter - 1;
            var previousYear = quarter == 1 ? year - 1 : year;
            var prevStartMonth = (previousQuarter - 1) * 3 + 1;
            var prevEndMonth = previousQuarter * 3;
            
            var previousQuarterPayments = payments
                .Where(p => p.PaymentDate.Year == previousYear && 
                           p.PaymentDate.Month >= prevStartMonth && 
                           p.PaymentDate.Month <= prevEndMonth)
                .ToList();
            var previousRevenue = previousQuarterPayments.Sum(p => (decimal)p.Amount);
            
            var growth = previousRevenue > 0 
                ? ((revenue - previousRevenue) / previousRevenue * 100) 
                : 0;

            quarterlyData.Add(new RevenueDataPoint
            {
                Period = $"Q{quarter} {year}",
                Revenue = revenue,
                Growth = growth,
                TransactionCount = quarterPayments.Count
            });
        }

        return quarterlyData;
    }

    private List<RevenueDataPoint> CalculateYearlyRevenue(
        List<FIN_Payment> payments)
    {
        var years = payments.Select(p => p.PaymentDate.Year).Distinct().OrderBy(y => y);
        var yearlyData = new List<RevenueDataPoint>();
        
        decimal previousRevenue = 0;
        foreach (var year in years)
        {
            var yearPayments = payments.Where(p => p.PaymentDate.Year == year).ToList();
            var revenue = yearPayments.Sum(p => (decimal)p.Amount);
            
            var growth = previousRevenue > 0 
                ? ((revenue - previousRevenue) / previousRevenue * 100) 
                : 0;

            yearlyData.Add(new RevenueDataPoint
            {
                Period = year.ToString(),
                Revenue = revenue,
                Growth = growth,
                TransactionCount = yearPayments.Count
            });

            previousRevenue = revenue;
        }

        return yearlyData;
    }

    private decimal CalculateCourseRevenue(Guid courseId, List<ACAD_Enrollment> enrollments)
    {
        // Calculate revenue based on enrollments
        var courseEnrollments = enrollments.Where(e => e.CourseID == courseId).ToList();
        return courseEnrollments.Count * 1000000; // Placeholder: 1M VND per enrollment
    }

    private List<DropoutTrendPoint> CalculateDropoutTrend(
        List<ACAD_Enrollment> enrollments, int months)
    {
        var trend = new List<DropoutTrendPoint>();
        
        for (int i = months - 1; i >= 0; i--)
        {
            var targetDate = DateTime.Now.AddMonths(-i);
            // Use the latest update timestamp when grouping by month; fallback to CreatedAt
            var monthEnrollments = enrollments
                .Where(e =>
                {
                    var date = e.UpdatedAt ?? e.CreatedAt;
                    return date.Year == targetDate.Year && date.Month == targetDate.Month;
                })
                .ToList();

            var droppedOut = monthEnrollments
                .Count(e => e.EnrollmentStatus.Code == "Dropped");

            var total = monthEnrollments.Count;
            var dropoutRate = total > 0 ? (droppedOut / (decimal)total * 100) : 0;

            trend.Add(new DropoutTrendPoint
            {
                Period = targetDate.ToString("MMM"),
                TotalStudents = total,
                DroppedOut = droppedOut,
                DropoutRate = dropoutRate,
                RetentionRate = 100 - dropoutRate
            });
        }

        return trend;
    }

    private List<DropoutReason> CalculateDropoutReasons(
        List<ACAD_Enrollment> droppedEnrollments)
    {
        // This would need actual dropout reason field in database
        // Placeholder implementation
        return new List<DropoutReason>
        {
            new() { Reason = "Not enough time for study", Count = 0, Percentage = 0 },
            new() { Reason = "Financial difficulties", Count = 0, Percentage = 0 },
            new() { Reason = "Course content not suitable", Count = 0, Percentage = 0 },
            new() { Reason = "Personal health issues", Count = 0, Percentage = 0 },
            new() { Reason = "Found employment", Count = 0, Percentage = 0 }
        };
    }

    private int CalculateHighRiskStudents(List<ACAD_Enrollment> enrollments)
    {
        // Calculate based on engagement metrics, attendance, etc.
        // Placeholder: students with < 50% attendance in last 30 days
        return 0; // Would need actual attendance data
    }

    private int CalculateAverageTimeToDropout(List<ACAD_Enrollment> droppedEnrollments)
    {
        if (!droppedEnrollments.Any()) return 0;

        var timeSpans = droppedEnrollments
            .Where(e => e.UpdatedAt.HasValue)
            .Select(e => (e.UpdatedAt.Value - e.CreatedAt).TotalDays);

        return timeSpans.Any() ? (int)timeSpans.Average() : 0;
    }

    private List<DemographicDropoutAnalysis> CalculateDemographicAnalysis(
        List<ACAD_Enrollment> enrollments,
        List<IDN_Student> students,
        string? ageGroupFilter,
        string? courseTypeFilter)
    {
        // Would need to join enrollments with student demographics
        // Placeholder implementation
        return new List<DemographicDropoutAnalysis>();
    }


    private List<DropoutByCourse> CalculateDropoutByCourse(List<ACAD_Enrollment> enrollments)
    {
        // Group enrollments by CourseID
        var courseGroups = enrollments
            .Where(e => e.Course != null)
            .GroupBy(e => e.CourseID)
            .ToList();

        var dropoutByCourse = new List<DropoutByCourse>();

        foreach (var courseGroup in courseGroups)
        {
            var courseEnrollments = courseGroup.ToList();
            if (!courseEnrollments.Any()) continue;

            var droppedOut = courseEnrollments.Count(e => e.EnrollmentStatus.Code == "Dropped");
            var dropoutRate = courseEnrollments.Count > 0 
                ? (droppedOut / (decimal)courseEnrollments.Count * 100) 
                : 0;

            // Get course info from first enrollment
            var firstEnrollment = courseEnrollments.FirstOrDefault();
            var course = firstEnrollment?.Course;
            
            // Count distinct classes for this course
            var numberOfClasses = courseEnrollments
                .Where(e => e.ClassID.HasValue)
                .Select(e => e.ClassID.Value)
                .Distinct()
                .Count();

            dropoutByCourse.Add(new DropoutByCourse
            {
                CourseId = courseGroup.Key.ToString(),
                CourseName = course?.CourseName ?? "N/A",
                CourseCode = course?.CourseCode,
                TotalStudents = courseEnrollments.Count,
                DroppedOut = droppedOut,
                DropoutRate = dropoutRate,
                NumberOfClasses = numberOfClasses
            });
        }

        return dropoutByCourse.OrderByDescending(d => d.DropoutRate).ToList();
    }

    private List<EnrollmentByCourseAggregated> CalculateEnrollmentByCourse(List<ACAD_Enrollment> enrollments)
    {
        // Group enrollments by CourseID
        var courseGroups = enrollments
            .Where(e => e.Course != null)
            .GroupBy(e => e.CourseID)
            .ToList();

        var enrollmentByCourse = new List<EnrollmentByCourseAggregated>();

        foreach (var courseGroup in courseGroups)
        {
            var courseEnrollments = courseGroup.ToList();
            if (!courseEnrollments.Any()) continue;

            var activeEnrollments = courseEnrollments.Count(e => e.EnrollmentStatus.Code == "Active");
            var completedEnrollments = courseEnrollments.Count(e => e.EnrollmentStatus.Code == "Completed");
            var droppedEnrollments = courseEnrollments.Count(e => e.EnrollmentStatus.Code == "Dropped");

            // Get course info from first enrollment
            var firstEnrollment = courseEnrollments.FirstOrDefault();
            var course = firstEnrollment?.Course;
            
            // Count distinct classes for this course (only enrollments that still have ClassID)
            var numberOfClasses = courseEnrollments
                .Where(e => e.ClassID.HasValue)
                .Select(e => e.ClassID.Value)
                .Distinct()
                .Count();

            enrollmentByCourse.Add(new EnrollmentByCourseAggregated
            {
                CourseId = courseGroup.Key.ToString(),
                CourseName = course?.CourseName ?? "N/A",
                CourseCode = course?.CourseCode,
                TotalEnrollments = courseEnrollments.Count,
                ActiveEnrollments = activeEnrollments,
                CompletedEnrollments = completedEnrollments,
                DroppedEnrollments = droppedEnrollments,
                NumberOfClasses = numberOfClasses
            });
        }

        return enrollmentByCourse.OrderByDescending(e => e.TotalEnrollments).ToList();
    }

    private List<string> GenerateDropoutRecommendations(StudentDropoutAnalyticsResponse data)
    {
        var recommendations = new List<string>();

        if (data.OverallDropoutRate > 20)
        {
            recommendations.Add("Create financial support program for students in need");
            recommendations.Add("Improve onboarding process during first month");
        }

        if (data.AverageTimeToDropout < 45)
        {
            recommendations.Add("Add flexible scheduling options for busy students");
            recommendations.Add("Enhance mentor support for early-stage students");
        }

        recommendations.Add("Create smaller checkpoints and milestones to increase motivation");

        return recommendations;
    }

    private List<AIRecommendation> GenerateAIRecommendations(
        RevenueAnalyticsResponse revenue,
        CourseEnrollmentStatsResponse courses,
        StudentDropoutAnalyticsResponse dropout,
        AIRecommendationRequest request)
    {
        var recommendations = new List<AIRecommendation>();

        // Revenue optimization
        if (request.FocusAreas.Contains("revenue"))
        {
            var topCourse = courses.TopCourses.FirstOrDefault();
            if (topCourse != null && topCourse.GrowthRate > 15)
            {
                recommendations.Add(new AIRecommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = "revenue",
                    Priority = "high",
                    Title = $"Optimize Pricing Strategy for {topCourse.CourseName}",
                    Description = $"{topCourse.CourseName} has high conversion rate and strong demand. Can increase price by 15-20% without impacting enrollment.",
                    Impact = "Estimated revenue increase of 15-20%",
                    ActionItems = new List<string>
                    {
                        "Analyze competitor pricing",
                        "A/B testing with different price points",
                        "Create premium package with additional features",
                        "Apply dynamic pricing based on demand"
                    },
                    EstimatedImpact = new EstimatedImpact
                    {
                        Revenue = topCourse.Revenue * 0.175m,
                        Enrollments = -5
                    },
                    Confidence = 85,
                    GeneratedAt = DateTime.Now
                });
            }
        }

        // Retention improvement
        if (request.FocusAreas.Contains("retention") && dropout.OverallDropoutRate > 15)
        {
            recommendations.Add(new AIRecommendation
            {
                Id = Guid.NewGuid().ToString(),
                Category = "retention",
                Priority = "high",
                Title = "Reduce Student Dropout Rate",
                Description = $"{dropout.OverallDropoutRate:F1}% dropout rate detected. Early intervention program needed.",
                Impact = $"Reduce dropout from {dropout.OverallDropoutRate:F1}% to {dropout.OverallDropoutRate * 0.7m:F1}%",
                ActionItems = new List<string>
                {
                    "Create clear onboarding checklist",
                    "Assign mentor to new students for first 30 days",
                    "Automated check-ins on days 7, 14, 21",
                    "Organize study groups for peer support"
                },
                EstimatedImpact = new EstimatedImpact
                {
                    Retention = dropout.OverallDropoutRate * 0.3m,
                    Enrollments = (int)(courses.TotalEnrollments * 0.03m)
                },
                Confidence = 92,
                GeneratedAt = DateTime.Now
            });
        }

        // Enrollment growth
        if (request.FocusAreas.Contains("enrollment"))
        {
            var growingCourses = courses.TopCourses.Where(c => c.Trend == "up").ToList();
            if (growingCourses.Any())
            {
                var topGrowing = growingCourses.First();
                recommendations.Add(new AIRecommendation
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = "enrollment",
                    Priority = "medium",
                    Title = $"Expand {topGrowing.Category} Course Offerings",
                    Description = $"{topGrowing.Category} courses showing {topGrowing.GrowthRate:F1}% growth. Market demand is strong.",
                    Impact = "Increase 50-80 new enrollments per quarter",
                    ActionItems = new List<string>
                    {
                        $"Develop advanced {topGrowing.Category} courses",
                        "Create specialized tracks and certifications",
                        "Partnership with companies for real projects",
                        "Corporate training packages"
                    },
                    EstimatedImpact = new EstimatedImpact
                    {
                        Revenue = topGrowing.Revenue * 0.5m,
                        Enrollments = 65
                    },
                    Confidence = 78,
                    GeneratedAt = DateTime.Now
                });
            }
        }

        return recommendations;
    }

    private string GenerateExecutiveSummary(
        RevenueAnalyticsResponse revenue,
        CourseEnrollmentStatsResponse courses,
        StudentDropoutAnalyticsResponse dropout,
        StudentEnrollmentAnalyticsResponse enrollment)
    {
        var yearlyGrowth = revenue.Yearly.LastOrDefault()?.Growth ?? 0;
        return $"System shows good performance with {yearlyGrowth:F1}% YoY growth. " +
               $"Priorities: reduce dropout rate ({dropout.OverallDropoutRate:F1}%), " +
               $"optimize top courses, expand high-demand categories.";
    }

    private List<string> GenerateKeyInsights(
        RevenueAnalyticsResponse revenue,
        CourseEnrollmentStatsResponse courses,
        StudentDropoutAnalyticsResponse dropout,
        StudentEnrollmentAnalyticsResponse enrollment)
    {
        var insights = new List<string>();
        
        // Add enrollment insights
        if (enrollment.MonthOverMonthGrowth > 0)
        {
            insights.Add($"Enrollments growing {enrollment.MonthOverMonthGrowth:F1}% month-over-month");
        }
        else if (enrollment.MonthOverMonthGrowth < 0)
        {
            insights.Add($"Enrollments declining {Math.Abs(enrollment.MonthOverMonthGrowth):F1}% month-over-month - requires attention");
        }
        
        if (enrollment.TopGrowingCourses.Any())
        {
            var topGrowingCourse = enrollment.TopGrowingCourses.First();
            insights.Add($"{topGrowingCourse.CourseName} is fastest growing course with {topGrowingCourse.GrowthRate:F1}% growth");
        }

        if (courses.TopCourses.Any())
        {
            var topCategories = courses.TopCourses
                .GroupBy(c => c.Category)
                .OrderByDescending(g => g.Sum(c => c.TotalEnrollments))
                .Take(2)
                .Select(g => g.Key);
            
            insights.Add($"Top categories: {string.Join(", ", topCategories)} - driving majority of enrollment");
        }

        var avgCompletion = courses.TopCourses.Any() 
            ? courses.TopCourses.Average(c => c.CompletionRate) 
            : 0;
        insights.Add($"Average completion rate {avgCompletion:F0}% - {(avgCompletion > 65 ? "above" : "below")} industry standard (65%)");

        var avgRating = courses.TopCourses.Any() 
            ? courses.TopCourses.Average(c => c.AverageRating) 
            : 0;
        insights.Add($"Student satisfaction {avgRating:F1}/5 - {(avgRating >= (decimal)4.5 ? "excellent" : "good")} feedback");

        var latestGrowth = revenue.Quarterly.LastOrDefault()?.Growth ?? 0;
        insights.Add($"Recent quarter shows {Math.Abs(latestGrowth):F1}% {(latestGrowth >= 0 ? "growth" : "decline")} in revenue");

        return insights;
    }

    private List<string> GenerateRiskFactors(
        StudentDropoutAnalyticsResponse dropout,
        CourseEnrollmentStatsResponse courses)
    {
        var risks = new List<string>();

        if (dropout.OverallDropoutRate > 20)
        {
            risks.Add($"Dropout rate {dropout.OverallDropoutRate:F1}% - needs immediate attention");
        }

        if (dropout.HighRiskStudents > 50)
        {
            risks.Add($"{dropout.HighRiskStudents} high-risk students identified");
        }

        var decliningCourses = courses.TopCourses.Count(c => c.Trend == "down");
        if (decliningCourses > 0)
        {
            risks.Add($"{decliningCourses} courses showing downward enrollment trend");
        }

        // Check course concentration
        if (courses.TopCourses.Any())
        {
            var topCoursePercentage = (courses.TopCourses.First().TotalEnrollments / 
                                      (decimal)courses.TotalEnrollments) * 100;
            if (topCoursePercentage > 40)
            {
                risks.Add($"Over-dependency on single course ({topCoursePercentage:F0}% of enrollments)");
            }
        }

        return risks;
    }

    private List<string> GenerateOpportunities(
        RevenueAnalyticsResponse revenue,
        CourseEnrollmentStatsResponse courses)
    {
        var opportunities = new List<string>();

        var growingCourses = courses.TopCourses.Count(c => c.Trend == "up");
        if (growingCourses > 0)
        {
            opportunities.Add($"{growingCourses} courses growing - expand these offerings");
        }

        if (courses.TopCourses.Any(c => c.AverageRating >= 4.7m))
        {
            opportunities.Add("High-rated courses can support premium pricing");
        }

        opportunities.Add("Corporate training market potential");
        opportunities.Add("Online/hybrid learning demand growing");

        return opportunities;
    }

    #endregion

    #region Enrollment Analytics Helper Methods

    private List<EnrollmentTrendPoint> CalculateMonthlyEnrollmentTrend(
        List<ACAD_Enrollment> enrollments, int months)
    {
        var trend = new List<EnrollmentTrendPoint>();
        
        for (int i = months - 1; i >= 0; i--)
        {
            var targetDate = DateTime.Now.AddMonths(-i);
            var monthEnrollments = enrollments
                .Where(e => e.CreatedAt.Year == targetDate.Year && 
                           e.CreatedAt.Month == targetDate.Month)
                .ToList();

            var total = monthEnrollments.Count;
            var active = monthEnrollments.Count(e => e.EnrollmentStatus.Code == "Active");
            var completed = monthEnrollments.Count(e => e.EnrollmentStatus.Code == "Completed");
            var dropped = monthEnrollments.Count(e => e.EnrollmentStatus.Code == "Dropped");

            // Calculate growth rate compared to previous month
            decimal growthRate = 0;
            if (i < months - 1 && trend.Any())
            {
                var previousTotal = trend.Last().TotalEnrollments;
                if (previousTotal > 0)
                {
                    growthRate = ((total - previousTotal) / (decimal)previousTotal * 100);
                }
            }

            trend.Add(new EnrollmentTrendPoint
            {
                Period = targetDate.ToString("MMM yyyy"),
                TotalEnrollments = total,
                ActiveEnrollments = active,
                CompletedEnrollments = completed,
                DroppedEnrollments = dropped,
                GrowthRate = growthRate
            });
        }

        return trend;
    }

    private List<EnrollmentTrendPoint> CalculateQuarterlyEnrollmentTrend(
        List<ACAD_Enrollment> enrollments, int quarters)
    {
        var trend = new List<EnrollmentTrendPoint>();
        var currentDate = DateTime.Now;
        
        for (int i = quarters - 1; i >= 0; i--)
        {
            var targetDate = currentDate.AddMonths(-i * 3);
            var quarter = (targetDate.Month - 1) / 3 + 1;
            var year = targetDate.Year;

            var quarterEnrollments = enrollments
                .Where(e => e.CreatedAt.Year == year && 
                           ((e.CreatedAt.Month - 1) / 3 + 1) == quarter)
                .ToList();

            var total = quarterEnrollments.Count;
            var active = quarterEnrollments.Count(e => e.EnrollmentStatus.Code == "Active");
            var completed = quarterEnrollments.Count(e => e.EnrollmentStatus.Code == "Completed");
            var dropped = quarterEnrollments.Count(e => e.EnrollmentStatus.Code == "Dropped");

            // Calculate growth rate compared to previous quarter
            decimal growthRate = 0;
            if (i < quarters - 1 && trend.Any())
            {
                var previousTotal = trend.Last().TotalEnrollments;
                if (previousTotal > 0)
                {
                    growthRate = ((total - previousTotal) / (decimal)previousTotal * 100);
                }
            }

            trend.Add(new EnrollmentTrendPoint
            {
                Period = $"Q{quarter} {year}",
                TotalEnrollments = total,
                ActiveEnrollments = active,
                CompletedEnrollments = completed,
                DroppedEnrollments = dropped,
                GrowthRate = growthRate
            });
        }

        return trend;
    }

    private async Task<List<EnrollmentByCourse>> CalculateTopGrowingCoursesAsync(
        List<ACAD_Enrollment> enrollments)
    {
        var allCourses = await _courseRepository.GetAllCourse();
        var courses = allCourses.Where(c => !c.IsDeleted).ToList();

        var courseEnrollmentData = new List<EnrollmentByCourse>();
        var currentDate = DateTime.Now;
        var threeMonthsAgo = currentDate.AddMonths(-3);

        foreach (var course in courses)
        {
            var courseEnrollments = enrollments.Where(e => e.CourseID == course.Id).ToList();
            if (!courseEnrollments.Any()) continue;

            var recentEnrollments = courseEnrollments.Count(e => e.CreatedAt >= threeMonthsAgo);
            var olderEnrollments = courseEnrollments.Count(e => e.CreatedAt < threeMonthsAgo);

            decimal growthRate = 0;
            string trend = "stable";
            
            if (olderEnrollments > 0)
            {
                growthRate = ((recentEnrollments - olderEnrollments) / (decimal)olderEnrollments * 100);
                if (growthRate > 10) trend = "up";
                else if (growthRate < -10) trend = "down";
            }
            else if (recentEnrollments > 0)
            {
                growthRate = 100;
                trend = "up";
            }

            courseEnrollmentData.Add(new EnrollmentByCourse
            {
                CourseId = course.Id.ToString(),
                CourseName = course.CourseName,
                CourseCode = course.CourseCode ?? "N/A",
                Category = course.Category.Name ?? "N/A",
                TotalEnrollments = courseEnrollments.Count,
                ActiveEnrollments = courseEnrollments.Count(e => e.EnrollmentStatus.Code == "Active"),
                GrowthRate = growthRate,
                Trend = trend
            });
        }

        return courseEnrollmentData
            .OrderByDescending(c => c.GrowthRate)
            .Take(10)
            .ToList();
    }

    private List<string> GenerateEnrollmentInsights(List<ACAD_Enrollment> enrollments)
    {
        var insights = new List<string>();
        
        if (!enrollments.Any())
        {
            insights.Add("No enrollment data available");
            return insights;
        }

        // Calculate monthly trend
        var lastMonth = enrollments.Count(e => e.CreatedAt >= DateTime.Now.AddMonths(-1));
        var previousMonth = enrollments.Count(e => e.CreatedAt >= DateTime.Now.AddMonths(-2) && 
                                                   e.CreatedAt < DateTime.Now.AddMonths(-1));
        
        if (previousMonth > 0)
        {
            var monthlyGrowth = ((lastMonth - previousMonth) / (decimal)previousMonth * 100);
            if (monthlyGrowth > 0)
            {
                insights.Add($"Enrollments increased by {monthlyGrowth:F1}% compared to last month");
            }
            else if (monthlyGrowth < 0)
            {
                insights.Add($"Enrollments decreased by {Math.Abs(monthlyGrowth):F1}% compared to last month");
            }
        }

        // Peak enrollment period
        var enrollmentsByMonth = enrollments
            .GroupBy(e => e.CreatedAt.Month)
            .Select(g => new { Month = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .FirstOrDefault();

        if (enrollmentsByMonth != null)
        {
            var monthName = new DateTime(2024, enrollmentsByMonth.Month, 1).ToString("MMMM");
            insights.Add($"Peak enrollment period: {monthName} with {enrollmentsByMonth.Count} enrollments");
        }

        // Completion rate
        var completedCount = enrollments.Count(e => e.EnrollmentStatus.Code == "Completed");
        if (enrollments.Count > 0)
        {
            var completionRate = (completedCount / (decimal)enrollments.Count * 100);
            insights.Add($"Overall completion rate: {completionRate:F1}%");
        }

        // Active vs inactive ratio
        var activeCount = enrollments.Count(e => e.EnrollmentStatus.Code == "Active");
        if (enrollments.Count > 0)
        {
            var activeRate = (activeCount / (decimal)enrollments.Count * 100);
            insights.Add($"Currently {activeRate:F1}% of enrollments are active");
        }

        return insights;
    }

    #endregion
}

