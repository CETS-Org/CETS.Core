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

	}
}



