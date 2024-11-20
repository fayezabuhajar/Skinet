
using Core.Entities;

namespace Core.Interfaces;

// Defines a generic repository interface for common CRUD operations
public interface IGenericRepository<T> where T : BaseEntity
{
    // Retrieves an entity by its ID asynchronously
    Task<T?> GetByIdAsync(int id);

    // Lists all entities in a read-only list asynchronously
    Task<IReadOnlyList<T>> ListALLAsync();

    // Retrieves an entity based on a specification
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);

    // Lists all entities that match a specification asynchronously
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

     Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T,TResult> spec);

 
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T,TResult> spec);

    // Adds a new entity to the repository
    void Add(T entity);

    // Updates an existing entity in the repository
    void Update(T entity);

    // Removes an entity from the repository
    void Remove(T entity);

    // Saves all changes to the repository asynchronously
    Task<bool> SaveAllAsync();

    // Checks if an entity with a given ID exists in the repository
    bool Exists(int id);

    Task<int> CountAsync(ISpecification<T> spec);
}
