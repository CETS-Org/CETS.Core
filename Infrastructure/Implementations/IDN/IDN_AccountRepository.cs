using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_AccountRepository : BaseRepository<IDN_Account>, IIDN_AccountRepository
    {
        public IDN_AccountRepository(AppDbContext context) : base(context)
        {
        }
        public IQueryable<IDN_Account> QueryWithRoles()
        {
            return _context.IDN_Accounts
                .Include(a => a.IDN_AccountRoles)
                .ThenInclude(r => r.Role);
        }

        public async Task<IDN_Account?> GetDetailByIdAsync(Guid id)
        {
            return await _context.IDN_Accounts
                .Include(a => a.IDN_StudentAccount)
                .Include(a => a.IDN_TeacherAccount)
                .Include(a => a.IDN_AccountRoles)
                .Include(a => a.AccountStatus)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
