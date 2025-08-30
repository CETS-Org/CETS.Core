using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Attendance.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<ACAD_Attendance, AttendanceResponse>();
        }
    }
}
