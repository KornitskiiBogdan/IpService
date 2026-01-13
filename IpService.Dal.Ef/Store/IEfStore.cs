using System.Linq.Expressions;
using IpService.Domain.Entities;
using IpService.Domain.Store;

namespace IpService.Dal.Ef.Store;

public interface IEfStore<TModel> : IStore<TModel> where TModel : class
{
    Task<Result<IEnumerable<TModel>>> AddManyAsync(IEnumerable<TModel> entities, CancellationToken token = default);

    Task<Result> UpdateManyAsync(Expression<Func<TModel, bool>> filter, Action<TModel> updateDefinition, CancellationToken token = default);

    Task<Result> UpdateManyAsync(IEnumerable<TModel> entities, CancellationToken token = default);

    Task<Result> DeleteManyAsync(IEnumerable<TModel> entities, CancellationToken token = default);
}