using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using IpService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IpService.Dal.Ef.Store;

internal sealed class Store<TEntity> : IEfStore<TEntity> where TEntity : class
{
    private readonly DbContextBase _dbContext;

    public Store([NotNull] DbContextBase dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Result<TEntity>> UpdateAsync(TEntity entity, CancellationToken token = default)
    {
        _dbContext.Set<TEntity>().Update(entity);

        var res = await SaveChangesAsync(token);

        return res.IsSuccess ? entity : res.Error;
    }

    public async Task<Result<TEntity>> AddAsync(TEntity entity, CancellationToken token = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, token);

        var res = await SaveChangesAsync(token);

        return res.IsSuccess ? entity : res.Error;
    }

    public async Task<Result<IEnumerable<TEntity>>> AddManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        var dataToAdd = entities.ToArray();

        await _dbContext.Set<TEntity>().AddRangeAsync(dataToAdd);

        var res = await SaveChangesAsync(token);

        return res.IsSuccess ? Result<IEnumerable<TEntity>>.Success(dataToAdd) : res.Error;
    }

    public async Task<Result> DeleteAsync(TEntity entity, CancellationToken token = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);

        return await SaveChangesAsync(token);
    }

    public async Task<Result> UpdateManyAsync(Expression<Func<TEntity, bool>> filter, Action<TEntity> updateDefinition, CancellationToken token = default)
    {
        var entities = await _dbContext.Set<TEntity>().Where(filter).ToListAsync(token);

        entities.ForEach(updateDefinition.Invoke);

        return await SaveChangesAsync(token);
    }

    public async Task<Result> UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);

        return await SaveChangesAsync(token);
    }

    public async Task<Result> DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);

        return await SaveChangesAsync(token);
    }

    private async Task<Result> SaveChangesAsync(CancellationToken token = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(token);

            return Result.Success();
        }
        catch (DbUpdateConcurrencyException e)
        {
            return Error.Create($"Record was updated by another user, {e.Message}");
        }
    }
}