using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IDN
{
    public interface IIDN_AccountRepository : IBaseRepository<IDN_Account>
    {
        IQueryable<IDN_Account> QueryWithRoles();
        Task<IDN_Account?> GetDetailByIdAsync(Guid id);
    }
}
