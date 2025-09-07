using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Assignment.Requests;
using DTOs.ACAD.ACAD_Assignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<CreateAssignmentRequest, ACAD_Assignment>();
            CreateMap<UpdateAssignmentRequest, ACAD_Assignment>();
            CreateMap<ACAD_Assignment, AssignmentResponse>();
        }
    }
}
