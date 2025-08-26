using BusinessObjects.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
       
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<Task<TResult>> work,
            IsolationLevel isolation = IsolationLevel.ReadCommitted,
            CancellationToken ct = default)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(isolation, ct);

                try
                {
                    var result = await work();
                    await _context.SaveChangesAsync(ct);    // Note: SaveChangesAsync is often called inside the 'work' delegate by the repositories themselves, but calling it here acts as a final commit of all tracked changes before the transaction's CommitAsync.
                    await transaction.CommitAsync(ct);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            });
        }
        public async Task ExecuteInTransactionAsync(
            Func<Task> work,
            IsolationLevel isolation = IsolationLevel.ReadCommitted,
            CancellationToken ct = default)
        {
            await ExecuteInTransactionAsync(async () =>
            {
                await work();
                return true; 
            }, isolation, ct);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }



    }
}
