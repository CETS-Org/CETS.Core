using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_PlacementQuestionRepository : IBaseRepository<ACAD_PlacementQuestion>
    {
        Task<IEnumerable<ACAD_PlacementQuestion>> GetQuestionsByCriteriaAsync(
            Guid questionTypeId, 
            int difficulty, 
            Guid? skillTypeId = null);
        
        Task<IEnumerable<ACAD_PlacementQuestion>> GetRandomQuestionsByCriteriaAsync(
            Guid questionTypeId, 
            int difficulty, 
            int count,
            Guid? skillTypeId = null);
    }
}

