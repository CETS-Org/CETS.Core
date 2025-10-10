using AutoMapper;
using Domain.Entities;
using DTOs.RPT.RPT_Report.Requests;
using DTOs.RPT.RPT_Report.Responses;

namespace Application.Mappers.RPT
{
	public class RPT_ReportProfile : Profile
	{
		public RPT_ReportProfile()
		{
			CreateMap<RPT_Report, ReportResponse>().ReverseMap();
			CreateMap<CreateReportRequest, RPT_Report>();
			CreateMap<UpdateReportRequest, RPT_Report>();
		}
	}
}



