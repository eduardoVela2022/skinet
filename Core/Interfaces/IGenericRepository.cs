// Imports
using Core.Entities;

// File path
namespace Core.Interfaces;

// This generic repository provides the most common queries
// So that we don't have to write them over and over again
public interface IGenericRepository<T> where T : BaseEntity
{
    // Get by id
    Task<T?> GetByIdAsync(int id);

    // Get all
    Task<IReadOnlyList<T>> ListAllAsync();

    // Get entity by specs
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);

    // Get all entities with certain specs
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

    // Get entity by specs and with capabilities to return somthing different from T
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);

    // Get all entities with certain specs  and with capabilities to return somthing different from T
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);

    // Add entity
    void Add (T entity);

    // Update entity
    void Update(T entity);

    // Remove entity
    void Remove(T entity);

    // Save all database changes
    Task<bool> SaveAllAsync();

    // Check if entity exists
    bool Exists(int id);
}
