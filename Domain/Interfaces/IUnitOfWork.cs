using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();

        // Auto-transaction helper
        Task<TResult> ExecuteInTransactionAsync<TResult>(
           Func<Task<TResult>> work,
           IsolationLevel isolation = IsolationLevel.ReadCommitted,
           CancellationToken ct = default);

        // An overload for actions that don't return a value.
        Task ExecuteInTransactionAsync(
            Func<Task> work,
            IsolationLevel isolation = IsolationLevel.ReadCommitted,
            CancellationToken ct = default);

        /*
         Task BeginTransactionAsync();
         Task CommitTransactionAsync();
         Task RollbackTransactionAsync();
       */
    }
}
