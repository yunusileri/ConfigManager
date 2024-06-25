using System.Linq.Expressions;
using ConfigManager.DataAccess.Abstract;
using ConfigManager.Entities.DbClass;
using Microsoft.EntityFrameworkCore;

namespace ConfigManager.DataAccess.Concrete;

public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext, new()
{
    public TEntity Add(TEntity entity)
    {
        using var context = new TContext();
        var addedEntity = context.Entry(entity);
        addedEntity.State = EntityState.Added;
        context.SaveChanges();
        return entity;
    }

    public void AddRange(List<TEntity> entities)
    {
        using var context = new TContext();
        context.AddRange(entities);
        context.SaveChanges();
    }

    public bool Any(Expression<Func<TEntity, bool>>? filter = null)
    {
        using var context = new TContext();
        return filter == null ? context.Set<TEntity>().Any() : context.Set<TEntity>().Any(filter);
    }

    public void Delete(TEntity entity)
    {
        using var context = new TContext();
        var deletedEntity = context.Entry(entity);
        deletedEntity.State = EntityState.Deleted;
        context.SaveChanges();
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> filter)
    {
        using var context = new TContext();
        return context.Set<TEntity>().SingleOrDefault(filter);
    }

    public List<TEntity> GetList(Expression<Func<TEntity, bool>>? filter = null)
    {
        using var context = new TContext();
        return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
    }

    public TEntity Update(TEntity entity)
    {
        using var context = new TContext();
        var updatedEntity = context.Entry(entity);
        updatedEntity.State = EntityState.Modified;
        context.SaveChanges();
        return entity;
    }

    public void UpdateRange(List<TEntity> entities)
    {
        using var context = new TContext();
        context.UpdateRange(entities);
        context.SaveChanges();
    }
}