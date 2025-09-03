using AutoMapper;
using DTOs.ACAD.ACAD_CourseCategory.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_CourseCategoryProfile : Profile
    {
        public ACAD_CourseCategoryProfile()
        {
            CreateMap<ACAD_CourseCategoryProfile, CourseCategoryResponse>();
        }
    }
}
