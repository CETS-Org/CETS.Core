using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.CORE;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.CORE
{
    public class CORE_LookUpTypeRepository : BaseRepository<CORE_LookUpType>, ICORE_LookUpTypeRepository
    {
        public CORE_LookUpTypeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CORE_LookUpType?> GetByCodeAsync(string code)
        {
            return await _context.CORE_LookUpTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(lut => lut.Code == code);
        }

        public async Task<CORE_LookUpType?> GetByNameAsync(string name)
        {
            return await _context.CORE_LookUpTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(lut => lut.Name == name);
        }
    }
}


