using System.Linq.Expressions;
using ConfigManager.Entities.DbClass;

namespace ConfigManager.DataAccess.Abstract;

public interface IGenericRepository<T> where T : class, IEntity, new()
{
    List<T> GetList(Expression<Func<T, bool>>? filter = null);

    T? Get(Expression<Func<T, bool>> filter);

    T Update(T entity);

    T Add(T entity);

    void Delete(T entity);
    void UpdateRange(List<T> entities);
    void AddRange(List<T> entities);
    bool Any(Expression<Func<T, bool>>? filter = null);
}