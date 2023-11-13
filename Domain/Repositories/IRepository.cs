namespace Domain.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(T entity);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<int> RemoveAsync(T entity);
}