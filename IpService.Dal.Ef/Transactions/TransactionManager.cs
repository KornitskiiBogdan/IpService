using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace IpService.Dal.Ef.Transactions;

internal sealed class TransactionManager : ITransactionManager
{
    private readonly DbContextBase _dbContext;

    public TransactionManager(DbContextBase dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default) =>
        _dbContext.Database.BeginTransactionAsync(token);

    public IDbContextTransaction BeginTransaction() => _dbContext.Database.BeginTransaction();

    public TransactionScope BeginScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        TransactionScopeOption option = TransactionScopeOption.Required, TimeSpan? timeout = null) => new(
        option,
        new TransactionOptions { IsolationLevel = isolationLevel, Timeout = timeout ?? System.Transactions.TransactionManager.MaximumTimeout },
        TransactionScopeAsyncFlowOption.Enabled);
}