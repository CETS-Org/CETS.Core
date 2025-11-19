using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_PlacementTestRepository : BaseRepository<ACAD_PlacementTest>, IACAD_PlacementTestRepository
    {
        public ACAD_PlacementTestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ACAD_PlacementTest?> GetPlacementTestWithResultsAsync(Guid id)
        {
            return await _context.ACAD_PlacementTests
                .FirstOrDefaultAsync(pt => pt.Id == id && !pt.IsDeleted);
        }

        public async Task<IEnumerable<ACAD_PlacementTest>> GetAllActivePlacementTestsAsync()
        {
            return await _context.ACAD_PlacementTests
                .Where(pt => !pt.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_PlacementTest>> GetAllPlacementTestsForStaffAsync()
        {
            // Get all tests including deleted ones for staff management
            return await _context.ACAD_PlacementTests
                .ToListAsync();
        }
    }
}

