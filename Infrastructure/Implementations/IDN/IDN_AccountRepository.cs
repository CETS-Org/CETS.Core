using Domain.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.IDN;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_AccountRepository : BaseRepository<IDN_Account>, IIDN_AccountRepository
    {
        public IDN_AccountRepository(AppDbContext context) : base(context)
        {
        }
    }
}
