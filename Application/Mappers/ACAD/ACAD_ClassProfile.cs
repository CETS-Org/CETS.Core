using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.ACAD.ACAD_Class.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_ClassProfile : Profile
    {
        public ACAD_ClassProfile()
        {
            // Request -> Entity
            CreateMap<CreateClassRequest, ACAD_Class>();
            CreateMap<UpdateClassRequest, ACAD_Class>();

            // Entity -> Response
            CreateMap<ACAD_Class, ClassResponse>()
                .ForMember(dest => dest.StatusName,
                           opt => opt.MapFrom(src => src.ClassStatus != null ? src.ClassStatus.Name : string.Empty));
        }
    }
}
