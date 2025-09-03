using Application.Interfaces.RPT;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.RPT;
using DTOs.RPT.RPT_Report.Requests;
using DTOs.RPT.RPT_Report.Responses;

namespace Application.Implementations.RPT
{
	public class RPT_ReportService : BaseService<RPT_Report, ReportResponse, UpdateReportRequest, CreateReportRequest>, IRPT_ReportService
	{
		public RPT_ReportService(IRPT_ReportRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
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
	}
}



