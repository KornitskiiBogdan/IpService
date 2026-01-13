using IpService.Domain.Entities;

namespace IpService.Domain.Store;

public interface IStore<TEntity> where TEntity : class
{
    Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken token = default);

    Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken token = default);

    Task<Result> DeleteAsync(TEntity entity, CancellationToken token = default);
}