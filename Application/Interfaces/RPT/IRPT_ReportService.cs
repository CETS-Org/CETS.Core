using Domain.Entities;
using DTOs.RPT.RPT_Report.Requests;
using DTOs.RPT.RPT_Report.Responses;

namespace Application.Interfaces.RPT
{
	public interface IRPT_ReportService : IBaseService<RPT_Report, ReportResponse, UpdateReportRequest, CreateReportRequest>
	{
		Task<IReadOnlyList<ReportResponse>> GetByStatusIdAsync(Guid statusId);
		Task<IReadOnlyList<ReportResponse>> GetBySubmitterAsync(Guid submitterId);
		Task<string> GetDownloadUrlAsync(Guid id);
		
		// System Complaint methods - using ReportType to filter
		Task<IReadOnlyList<ReportResponse>> GetSystemComplaintsAsync();
		Task<IReadOnlyList<ReportResponse>> GetSystemComplaintsByStatusAsync(Guid reportTypeId, Guid statusId);
		Task<IReadOnlyList<ReportResponse>> GetSystemComplaintsByReportTypeAsync(Guid reportTypeId);
	}
}



