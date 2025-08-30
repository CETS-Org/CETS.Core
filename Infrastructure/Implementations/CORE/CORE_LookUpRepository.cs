using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.CORE;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CORE
{
    public class CORE_LookUpRepository : BaseRepository<CORE_LookUp>, ICORE_LookUpRepository
    {
        public CORE_LookUpRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CORE_LookUp?> GetByCodeAsync(string lookUpTypeCode, string code)
        {
            return await _context.CORE_LookUps
                .AsNoTracking()
                .FirstOrDefaultAsync(lu => lu.LookUpType.Code.ToString() == lookUpTypeCode
                                    && lu.Code == code 
                                    && lu.IsActive == true);
        }
        public async Task<CORE_LookUp?> GetByCodeAsync(string code)
        {
            return await _context.CORE_LookUps
                .AsNoTracking()
                .FirstOrDefaultAsync(lu => lu.Code == code 
                                    && lu.IsActive == true);
        }

        public async Task<CORE_LookUp?> GetByNameAsync(string name)
        {
            return await _context.CORE_LookUps
                .AsNoTracking()
                .FirstOrDefaultAsync(lu => lu.Name == name 
                                    && lu.IsActive == true);
        }

        public async Task<IReadOnlyList<CORE_LookUp>> GetByTypeAsync(Guid lookUpTypeId)
        {
            return await _context.CORE_LookUps
                .AsNoTracking()
                .Where(lu => lu.LookUpTypeID == lookUpTypeId 
                    && lu.IsActive == true)
                .ToListAsync();
        }
        public async Task<IReadOnlyList<CORE_LookUp>> GetByTypeAsync(string lookUpTypeCode)
        {
            return await _context.CORE_LookUps
                .AsNoTracking()
                .Include(lu => lu.LookUpType) 
                .Where(lu => lu.LookUpType != null 
                    && lu.LookUpType.Code == lookUpTypeCode
                    && lu.IsActive == true)
                .ToListAsync();
        }
    }
}


