using Domain.Entities;
using DTOs.RPT.RPT_Report.Requests;
using DTOs.RPT.RPT_Report.Responses;

namespace Application.Interfaces.RPT
{
	public interface IRPT_ReportService : IBaseService<RPT_Report, ReportResponse, UpdateReportRequest, CreateReportRequest>
	{
		Task<IReadOnlyList<ReportResponse>> GetByStatusIdAsync(Guid statusId);
		Task<IReadOnlyList<ReportResponse>> GetBySubmitterAsync(Guid submitterId);
		Task<AcademicReportUploadResponse> SubmitAcademicRequestAsync(SubmitAcademicReportRequest request);
		Task<IReadOnlyList<AcademicReportResponse>> GetAcademicRequestsBySubmitterAsync(Guid submitterId);
		Task<AcademicReportResponse?> GetAcademicRequestDetailsAsync(Guid requestId);
		Task<IReadOnlyList<AcademicReportResponse>> GetAcademicRequestsByCourseAsync(Guid courseId);
		Task<IReadOnlyList<AcademicReportResponse>> GetAcademicRequestsByClassAsync(Guid classId);
		Task<IReadOnlyList<AcademicReportResponse>> GetPendingAcademicRequestsAsync();
		Task ProcessAcademicRequestAsync(Guid requestId, ProcessAcademicReportRequest request);
		Task<string> GetDownloadUrlAsync(Guid id);
	}
}



