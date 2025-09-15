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
                .Include(a => a.AccountStatus)
                .Include(a => a.IDN_AccountRoles)
                .ThenInclude(r => r.Role);
        }

        public async Task<IDN_Account?> GetDetailByIdAsync(Guid id)
        {
            return await _context.IDN_Accounts
                .Include(a => a.IDN_StudentAccount)
                .Include(a => a.IDN_TeacherAccount)
                .Include(a => a.IDN_AccountRoles)
                    .ThenInclude( q => q.Role)
                .Include(a => a.AccountStatus)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IDN_Account?> GetUserByEmailAsync(string email)
        {
            return await _context.IDN_Accounts
                .Include(a => a.IDN_StudentAccount)
                .Include(a => a.IDN_TeacherAccount)
                .Include(a => a.IDN_AccountRoles)
                .ThenInclude(ar => ar.Role)
                .Include(a => a.AccountStatus)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IDN_Account?> GetUserByPhoneAsync(string phoneNumber)
        {
            return await _context.IDN_Accounts
                .Include(a => a.IDN_StudentAccount)
                .Include(a => a.IDN_TeacherAccount)
                .Include(a => a.IDN_AccountRoles)
                .Include(a => a.AccountStatus)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.IDN_Accounts.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsPhoneUniqueAsync(string phoneNumber)
        {
            return !await _context.IDN_Accounts.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
