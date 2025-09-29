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
                .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => src.PlanType.Name));
        }
    }
}
