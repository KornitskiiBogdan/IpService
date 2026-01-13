using System.Diagnostics.CodeAnalysis;
using IpService.Domain.Query.Abstractions;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace IpService.Dal.Ef.QueryProvider;

internal sealed class QueryProvider<TModel> : IQueryProvider<TModel>, IProjectionProvider<TModel>
    where TModel : class
{
    private readonly DbContextBase _dbContext;

    public QueryProvider([NotNull] DbContextBase dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<TProject> ProjectAsync<TProject>(IExecutor<TModel, TProject, IQueryable<TModel>> exec,
        ISpecification<TModel, IQueryable<TModel>>? spec = null, CancellationToken token = default) =>
        await exec.ExecuteAsync(GetQuery(spec), token);

    public async Task<TModel?> FirstOrDefaultBySpecAsync(ISpecification<TModel, IQueryable<TModel>>? spec = null,
        CancellationToken token = default) =>
        await GetQuery(spec).FirstOrDefaultAsync(token);

    public async Task<TModel?> SingleOrDefaultBySpecAsync(ISpecification<TModel, IQueryable<TModel>>? spec = null,
        CancellationToken token = default) =>
        await GetQuery(spec).SingleOrDefaultAsync(token);

    public async Task<IEnumerable<TModel>> EnumerableBySpecAsync(ISpecification<TModel, IQueryable<TModel>>? spec = null,
        CancellationToken token = default) =>
        await GetQuery(spec).ToArrayAsync(token);

    public async Task<long> CountBySpecAsync(ISpecification<TModel, IQueryable<TModel>>? spec = null,
        CancellationToken token = default) =>
        await GetQuery(spec).CountAsync(token);

    public async Task<bool> AnyBySpecAsync(ISpecification<TModel, IQueryable<TModel>>? spec = null,
        CancellationToken token = default) =>
        await GetQuery(spec).AnyAsync(token);

    public IAsyncEnumerable<TModel> AsyncEnumerableBySpec(ISpecification<TModel, IQueryable<TModel>>? spec = null) =>
        GetQuery(spec).AsAsyncEnumerable();

    public IAsyncEnumerable<TProject> ProjectAsyncEnumerableBySpec<TProject>(
        ISpecification<TModel, IQueryable<TModel>>? spec = null,
        CancellationToken token = default) =>
        GetQuery(spec).ProjectToType<TProject>().AsAsyncEnumerable();

    private IQueryable<TModel> GetQuery(ISpecification<TModel, IQueryable<TModel>>? spec)
    {
        var query = _dbContext.Set<TModel>().AsNoTracking();

        if (spec != null)
        {
            query = spec.Apply(query);
        }

        return query;
    }


}