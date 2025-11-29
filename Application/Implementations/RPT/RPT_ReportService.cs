using Application.Interfaces.RPT;
using Application.Interfaces.Common.Storage;
using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.RPT;
using Domain.Interfaces.CORE;
using DTOs.RPT.RPT_Report.Requests;
using DTOs.RPT.RPT_Report.Responses;
using DTOs.COM.COM_Notification.Requests;

namespace Application.Implementations.RPT
{
	public class RPT_ReportService : BaseService<RPT_Report, ReportResponse, UpdateReportRequest, CreateReportRequest>, IRPT_ReportService
	{
		private readonly IRPT_ReportRepository _reportRepository;
		private readonly ICORE_LookUpRepository _lookUpRepository;
		private readonly IFileStorageService _fileStorageService;
		private readonly ICOM_NotificationService _notificationService;

		public RPT_ReportService(
			IRPT_ReportRepository repository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			ICORE_LookUpRepository lookUpRepository,
			IFileStorageService fileStorageService,
			ICOM_NotificationService notificationService)
			: base(repository, unitOfWork, mapper)
		{
			_reportRepository = repository;
			_lookUpRepository = lookUpRepository;
			_fileStorageService = fileStorageService;
			_notificationService = notificationService;
		}

		public async Task<IReadOnlyList<ReportResponse>> GetByStatusIdAsync(Guid statusId)
		{
			var items = await _reportRepository.GetAllAsync();
			var filteredItems = items.Where(r => r.ReportStatusID == statusId).ToList();
			return _mapper.Map<IReadOnlyList<ReportResponse>>(filteredItems);
		}

		public async Task<IReadOnlyList<ReportResponse>> GetBySubmitterAsync(Guid submitterId)
		{
			var items = await _reportRepository.GetAllAsync();
			var filteredItems = items.Where(r => r.SubmittedBy == submitterId).ToList();
			return _mapper.Map<IReadOnlyList<ReportResponse>>(filteredItems);
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

		public async Task<IReadOnlyList<ReportResponse>> GetSystemComplaintsAsync()
		{
			// Get System Complaint ReportType by code
			/*var systemComplaintType = await _lookUpRepository.GetByCodeAsync(Domain.Constants.LookUpTypes.ReportType, "SYSTEM_COMPLAINT");
			if (systemComplaintType == null)
				return new List<ReportResponse>().AsReadOnly();
			*/
			var items = await _reportRepository.GetAllAsync();
			return _mapper.Map<IReadOnlyList<ReportResponse>>(items);
		}

		public async Task<IReadOnlyList<ReportResponse>> GetSystemComplaintsByStatusAsync(Guid reportTypeId, Guid statusId)
		{
			var items = await _reportRepository.GetSystemComplaintsByStatusAsync(reportTypeId, statusId);
			return _mapper.Map<IReadOnlyList<ReportResponse>>(items);
		}

		public async Task<IReadOnlyList<ReportResponse>> GetSystemComplaintsByReportTypeAsync(Guid reportTypeId)
		{
			var items = await _reportRepository.GetSystemComplaintsByReportTypeAsync(reportTypeId);
			return _mapper.Map<IReadOnlyList<ReportResponse>>(items);
		}

		public override async Task<ReportResponse> UpdateAsync(Guid id, UpdateReportRequest dto)
		{
			// Get existing entity with navigation properties
			var existingEntity = await _reportRepository.GetByIdAsync(id);
			if (existingEntity == null)
			{
				throw new KeyNotFoundException($"{typeof(RPT_Report).Name} with id {id} not found.");
			}

			// Store old status for comparison
			var oldStatusId = existingEntity.ReportStatusID;

			// Update entity
			_mapper.Map(dto, existingEntity);
			_reportRepository.Update(existingEntity);
			await _unitOfWork.SaveChangesAsync();

			// Reload entity with navigation properties to get updated status
			var updatedEntity = await _reportRepository.GetByIdAsync(id);
			if (updatedEntity != null && updatedEntity.ReportStatusID != oldStatusId)
			{
				// Status changed, send notification
				await SendReportStatusNotificationAsync(updatedEntity);
			}

			return _mapper.Map<ReportResponse>(updatedEntity ?? existingEntity);
		}

		private async Task SendReportStatusNotificationAsync(RPT_Report report)
		{
			try
			{
				var status = await _lookUpRepository.GetByIdAsync(report.ReportStatusID);
				if (status == null) return;

				var reportType = await _lookUpRepository.GetByIdAsync(report.ReportTypeID);
				var reportTypeName = reportType?.Name ?? "Complaint";

				var statusName = status.Name?.ToLower();
				var isResolved = statusName == "resolved";
				var isClosed = statusName == "closed";
				var isInProgress = statusName == "in progress";

				// Send notifications for In Progress, Resolved, or Closed status
				if (!isResolved && !isClosed && !isInProgress)
					return;

				string title;
				string message;
				string type;

				if (isResolved)
				{
					title = $"✅ {reportTypeName} Resolved";
					message = $"Great news! Your {reportTypeName.ToLower()} has been resolved by admin. ";
					type = "info";
				}
				else if (isClosed)
				{
					title = $"❌ {reportTypeName} Closed";
					message = $"Your {reportTypeName.ToLower()} has been closed by admin. ";
					type = "warning";
				}
				else // In Progress
				{
					title = $"📋 {reportTypeName} In Progress";
					message = $"Your {reportTypeName.ToLower()} is now being processed by admin. ";
					type = "info";
				}

				// Add admin response if available
				if (!string.IsNullOrEmpty(report.AdminResponse))
				{
					message += $"Admin comment: {report.AdminResponse}";
				}
				else
				{
					if (isResolved)
					{
						message += "Thank you for your patience.";
					}
					else if (isClosed)
					{
						message += "Please review the requirements and submit a new complaint if needed.";
					}
					else // In Progress
					{
						message += "We will update you as soon as there are any changes.";
					}
				}

				var notificationRequest = new CreateNotificationRequest
				{
					UserId = report.SubmittedBy.ToString().ToUpperInvariant(),
					Title = title,
					Message = message,
					Type = type,
					IsRead = false
				};

				await _notificationService.CreateAsync(notificationRequest);
			}
			catch (Exception ex)
			{
				// Log the error but don't fail the update operation
				Console.WriteLine($"Failed to send notification for report {report.Id}: {ex.Message}");
			}
		}

	}
}



