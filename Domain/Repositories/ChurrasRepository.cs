using Domain.Entities;

namespace Domain.Repositories;

public class ChurrasRepository : IChurrasRepository
{
    public Task<IEnumerable<Churras>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Churras> GetByIdAsync(Churras entity)
    {
        throw new NotImplementedException();
    }

    public Task<Churras> AddAsync(Churras entity)
    {
        throw new NotImplementedException();
    }

    public Task<Churras> UpdateAsync(Churras entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> RemoveAsync(Churras entity)
    {
        throw new NotImplementedException();
    }
}