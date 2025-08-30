using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Syllabus.Requests;
using DTOs.ACAD.ACAD_Syllabus.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Requests;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_SyllabusProfile : Profile
    {
        public ACAD_SyllabusProfile()
        {
            // Map Entity -> Response
            CreateMap<ACAD_Syllabus, SyllabusResponse>()
                .ForMember(dest => dest.SyllabusID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ACAD_SyllabusItems));

            CreateMap<ACAD_SyllabusItem, SyllabusItemResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            // Map Request -> Entity
            CreateMap<CreateSyllabusRequest, ACAD_Syllabus>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateSyllabusRequest, ACAD_Syllabus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SyllabusID))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<CreateSyllabusItemRequest, ACAD_SyllabusItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateSyllabusItemRequest, ACAD_SyllabusItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SyllabusItemID))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
