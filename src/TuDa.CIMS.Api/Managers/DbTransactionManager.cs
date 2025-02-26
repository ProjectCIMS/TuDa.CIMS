using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;

namespace TuDa.CIMS.Api.Managers;

[ScopedService]
public class DbTransactionManager : IDbTransactionManager
{
    private readonly CIMSDbContext _context;

    public DbTransactionManager(CIMSDbContext context)
    {
        _context = context;
    }

    public Task RunInTransactionAsync(Func<Task> action) =>
        _context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });

    public Task<T> RunInTransactionAsync<T>(Func<Task<T>> action) =>
        _context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var result = await action();
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });

    public Task<ErrorOr<T>> RunInTransactionAsync<T>(Func<Task<ErrorOr<T>>> action) =>
        _context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    return await action().ThenDoAsync(_ => transaction.CommitAsync());
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
}
