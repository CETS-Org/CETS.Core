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

			// Academic Request specific mappings
			CreateMap<SubmitAcademicReportRequest, RPT_Report>()
				.ForMember(dest => dest.ResolvedAt, opt => opt.Ignore())
				.ForMember(dest => dest.ResolvedBy, opt => opt.Ignore())
				.ForMember(dest => dest.ReportUrl, opt => opt.Ignore())
				.ForMember(dest => dest.ReportStatus, opt => opt.Ignore())
				.ForMember(dest => dest.ReportType, opt => opt.Ignore())
				.ForMember(dest => dest.SubmittedByNavigation, opt => opt.Ignore())
				.ForMember(dest => dest.ResolvedByNavigation, opt => opt.Ignore());


			CreateMap<RPT_Report, AcademicReportResponse>()
				.ForMember(dest => dest.ReportTypeName, opt => opt.MapFrom(src => src.ReportType != null ? src.ReportType.Name : null))
				.ForMember(dest => dest.SubmitterName, opt => opt.MapFrom(src => src.SubmittedByNavigation != null ? src.SubmittedByNavigation.FullName : null))
				.ForMember(dest => dest.SubmitterEmail, opt => opt.MapFrom(src => src.SubmittedByNavigation != null ? src.SubmittedByNavigation.Email : null))
				.ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.ReportStatus != null ? src.ReportStatus.Name : null))
				.ForMember(dest => dest.ResolvedByName, opt => opt.MapFrom(src => src.ResolvedByNavigation != null ? src.ResolvedByNavigation.FullName : null));
		}
	}
}



