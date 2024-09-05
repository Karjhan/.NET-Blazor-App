using Domain.Abstractions;

namespace Infrastructure.Abstractions.Persistence;

public interface IGenericRepository<T>
{
    public Task<T?> GetByIdAsync(Guid id);
    
    public Task<IReadOnlyList<T>> GetAllAsync();

    public Task<T?> GetEntityWithSpecification(ISpecification<T> specification);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);
}