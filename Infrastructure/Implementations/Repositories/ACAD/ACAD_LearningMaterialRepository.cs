using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_LearningMaterialRepository : BaseRepository<ACAD_LearningMaterial>, IACAD_LearningMaterialRepository
    {
        public ACAD_LearningMaterialRepository(AppDbContext context) : base(context)
        {
        }
    }
}


