using Domain.Models.Responses;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Repositories;

public interface IRepository<T>
{
    Task<PaginatedResponse<T>> GetPaginatedAsync(HttpRequestData? req, int pageNumber = 1, int pageSize = 50);
    Task<T> GetByIdAsync(string id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> RemoveAsync(string id);
}