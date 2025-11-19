using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_PlacementTestRepository : IBaseRepository<ACAD_PlacementTest>
    {
        Task<ACAD_PlacementTest?> GetPlacementTestWithResultsAsync(Guid id);
        Task<IEnumerable<ACAD_PlacementTest>> GetAllActivePlacementTestsAsync();
        Task<IEnumerable<ACAD_PlacementTest>> GetAllPlacementTestsForStaffAsync(); // Get all tests including deleted ones for staff management
    }
}

