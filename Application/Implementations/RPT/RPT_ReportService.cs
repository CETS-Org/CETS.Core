using Application.Interfaces.RPT;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.RPT;
using Domain.Interfaces.CORE;
using DTOs.RPT.RPT_Report.Requests;
using DTOs.RPT.RPT_Report.Responses;

namespace Application.Implementations.RPT
{
	public class RPT_ReportService : BaseService<RPT_Report, ReportResponse, UpdateReportRequest, CreateReportRequest>, IRPT_ReportService
	{
		private readonly IRPT_ReportRepository _reportRepository;
		private readonly ICORE_LookUpRepository _lookUpRepository;
		private readonly IFileStorageService _fileStorageService;

		public RPT_ReportService(
			IRPT_ReportRepository repository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			ICORE_LookUpRepository lookUpRepository,
			IFileStorageService fileStorageService)
			: base(repository, unitOfWork, mapper)
		{
			_reportRepository = repository;
			_lookUpRepository = lookUpRepository;
			_fileStorageService = fileStorageService;
		}

		public async Task<IReadOnlyList<ReportResponse>> GetByStatusIdAsync(Guid statusId)
		{
			var items = await _repository.FindAsync(r => r.ReportStatusID == statusId);
			return _mapper.Map<IReadOnlyList<ReportResponse>>(items);
		}

		public async Task<IReadOnlyList<ReportResponse>> GetBySubmitterAsync(Guid submitterId)
		{
			var items = await _repository.FindAsync(r => r.SubmittedBy == submitterId);
			return _mapper.Map<IReadOnlyList<ReportResponse>>(items);
		}

		#region Academic Request Methods

		
		public async Task<AcademicReportUploadResponse> SubmitAcademicRequestAsync(SubmitAcademicReportRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Title))
				throw new ArgumentException("Title is required");

			if (string.IsNullOrWhiteSpace(request.Description))
				throw new ArgumentException("Description is required");

			var pendingStatus = await _lookUpRepository.FindFirstAsync(l => l.Code == "Pending" && l.LookUpType.Code == "AcademicRequestStatus");
			if (pendingStatus == null)
				throw new InvalidOperationException("PENDING status not found in system");

			string? uploadUrl = null;
			string? filePath = null;

			var report = _mapper.Map<RPT_Report>(request);
			
			if (!string.IsNullOrWhiteSpace(request.FileName) && !string.IsNullOrWhiteSpace(request.ContentType))
			{
				// Get presigned upload URL and generated file path
				var (presignedUrl, generatedFilePath) = await _fileStorageService.GetPresignedPutUrlAsync(
					"academic-requests",
					request.FileName,
					request.ContentType);

				uploadUrl = presignedUrl;
				filePath = generatedFilePath;
				
				report.AttachmentUrl = filePath;
			}
			
			report.ReportStatusID = pendingStatus.Id;

			_reportRepository.Add(report);
			await _unitOfWork.SaveChangesAsync();

			var savedReport = await _reportRepository.GetAcademicRequestWithDetailsAsync(report.Id);
			
			var reportResponse = _mapper.Map<AcademicReportResponse>(savedReport);

			return new AcademicReportUploadResponse
			{
				Report = reportResponse,
				UploadUrl = uploadUrl,
				FilePath = filePath
			};
		}

		public async Task<IReadOnlyList<AcademicReportResponse>> GetAcademicRequestsBySubmitterAsync(Guid submitterId)
		{
			var reports = await _reportRepository.GetAcademicRequestsBySubmitterAsync(submitterId);
			return _mapper.Map<IReadOnlyList<AcademicReportResponse>>(reports);
		}

		public async Task<AcademicReportResponse?> GetAcademicRequestDetailsAsync(Guid requestId)
		{
			var report = await _reportRepository.GetAcademicRequestWithDetailsAsync(requestId);
			return report == null ? null : _mapper.Map<AcademicReportResponse>(report);
		}

		public async Task<IReadOnlyList<AcademicReportResponse>> GetAcademicRequestsByCourseAsync(Guid courseId)
		{
			var reports = await _reportRepository.GetAcademicRequestsByCourseAsync(courseId);
			return _mapper.Map<IReadOnlyList<AcademicReportResponse>>(reports);
		}

		public async Task<IReadOnlyList<AcademicReportResponse>> GetAcademicRequestsByClassAsync(Guid classId)
		{
			var reports = await _reportRepository.GetAcademicRequestsByClassAsync(classId);
			return _mapper.Map<IReadOnlyList<AcademicReportResponse>>(reports);
		}

		public async Task<IReadOnlyList<AcademicReportResponse>> GetPendingAcademicRequestsAsync()
		{
			var reports = await _reportRepository.GetPendingAcademicRequestsAsync();
			return _mapper.Map<IReadOnlyList<AcademicReportResponse>>(reports);
		}

	
		public async Task ProcessAcademicRequestAsync(Guid requestId, ProcessAcademicReportRequest request)
		{
			var report = await _reportRepository.GetByIdAsync(requestId);
			if (report == null)
				throw new ArgumentException("Academic request not found");

			// Update status and resolution information
			report.ReportStatusID = request.NewStatusId;
			report.ResolvedBy = request.ProcessedBy;
			report.ResolvedAt = DateTime.Now;
			
			if (!string.IsNullOrWhiteSpace(request.Notes))
			{
				report.Description += $"\n\n--- Staff Notes ---\n{request.Notes}";
			}

			_reportRepository.Update(report);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<string> GetDownloadUrlAsync(Guid id)
		{
			var report = await _reportRepository.GetByIdAsync(id);
			if (report == null)
				throw new KeyNotFoundException("Report not found");

			if (string.IsNullOrEmpty(report.AttachmentUrl))
				throw new InvalidOperationException("Report has no associated file");

			var fileExists = await _fileStorageService.FileExistsAsync(report.AttachmentUrl);
			if (!fileExists)
				throw new InvalidOperationException($"File not found in storage: {report.AttachmentUrl}");

			return await _fileStorageService.GetPresignedGetUrlAsync(report.AttachmentUrl);
		}

		#endregion
	}
}



