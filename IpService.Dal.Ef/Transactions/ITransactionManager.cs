using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace IpService.Dal.Ef.Transactions;

public interface ITransactionManager
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);

    IDbContextTransaction BeginTransaction();

    TransactionScope BeginScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        TransactionScopeOption option = TransactionScopeOption.Required, TimeSpan? timeout = null);
}