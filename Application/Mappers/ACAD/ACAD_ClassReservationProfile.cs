using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_ClassReservation.Requests;
using DTOs.ACAD.ACAD_ClassReservation.Responses;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using DTOs.ACAD.ACAD_ReservationItem.Responses;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_ClassReservationProfile : Profile
    {
        public ACAD_ClassReservationProfile()
        {
            // Request -> Entity
            CreateMap<CreateClassReservationRequest, ACAD_ClassReservation>();
            CreateMap<UpdateClassReservationRequest, ACAD_ClassReservation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Entity -> Response
            CreateMap<ACAD_ClassReservation, ClassReservationResponse>()
                .ForMember(dest => dest.PackageCode, opt => opt.MapFrom(src => src.CoursePackage.PackageCode))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.CoursePackage.Name))
                .ForMember(dest => dest.PackageImageUrl, opt => opt.MapFrom(src => src.CoursePackage.PackageImageUrl))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.CoursePackage.TotalPrice))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.CoursePackage.Description))
                .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.ReservationStatus.Name));
            
            
        }
    }
}
