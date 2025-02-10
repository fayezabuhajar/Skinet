using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

// Implements a generic repository for data access with Entity Framework Core
public class GenericRepository<T>(StoreContext context) : IGenericRepository<T>
    where T : BaseEntity
{
    // Adds a new entity to the context
    public void Add(T entity) => context.Set<T>().Add(entity);

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        var query = context.Set<T>().AsQueryable();

        query = spec.ApplyCriteria(query);

        return await query.CountAsync();
    }

    // Checks if an entity exists by its ID
    public bool Exists(int id) => context.Set<T>().Any(x => x.Id == id);

    // Retrieves an entity by its ID asynchronously
    public async Task<T?> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);

    // Retrieves an entity based on a specification asynchronously
    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        // Apply the specification and fetch the first entity that matches
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    // Retrieves all entities of type T as a read-only list asynchronously
    public async Task<IReadOnlyList<T>> ListALLAsync() => await context.Set<T>().ToListAsync();

    // Retrieves entities that match a specification as a read-only list asynchronously
    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        // Apply the specification and return the matching entities
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    // Removes an entity from the context
    public void Remove(T entity) => context.Set<T>().Remove(entity);

    // Saves all pending changes to the database asynchronously
    public async Task<bool> SaveAllAsync() => await context.SaveChangesAsync() > 0;

    // Updates an existing entity in the context
    public void Update(T entity)
    {
        // Attach the entity and mark it as modified for update
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    // Applies a specification to filter entities based on the given criteria
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        // Uses SpecificationEvaluator to apply the specification to the query
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        // Uses SpecificationEvaluator to apply the specification to the query
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), spec);
    }
}
