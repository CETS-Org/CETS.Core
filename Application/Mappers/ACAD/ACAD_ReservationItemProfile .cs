using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_ReservationItem.Requests;
using DTOs.ACAD.ACAD_ReservationItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_ReservationItemProfile : Profile
    {
        public ACAD_ReservationItemProfile()
        {
            // --- Mapping từ Request DTO sang Entity ---

            CreateMap<CreateReservationItemRequests, ACAD_ReservationItem>();
            CreateMap<UpdateReservationItemRequest, ACAD_ReservationItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

          
            CreateMap<ACAD_ReservationItem, ReservationItemResponse>()
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.CourseCode))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
                .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.Course.CourseImageUrl))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Course.Description))
                .ForMember(dest => dest.StandardPrice, opt => opt.MapFrom(src => src.Course.StandardPrice))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Course.Category.Name))
                .ForMember(dest => dest.InvoiceStatus, opt => opt.MapFrom(src => src.Invoice.InvoiceStatus.Name))
                .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => src.PlanType.Name));
        }
    }
}
