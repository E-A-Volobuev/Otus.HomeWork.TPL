using GrpcWithDbService.Entities;
using System.Linq.Expressions;

namespace GrpcWithDbService.Service.Abstractions;

public interface IEFGenericRepository<TEntity> where TEntity : class, IEntityId
{
    Task CreateAsync(TEntity item);
    Task<TEntity> FindByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task RemoveAsync(TEntity item);
    Task UpdateAsync(TEntity item);
    Task CreateRangeAsync(TEntity[] items);
}
