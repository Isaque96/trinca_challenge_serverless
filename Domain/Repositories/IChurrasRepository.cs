using Domain.Entities;
using Domain.Models.Responses;

namespace Domain.Repositories;

public interface IChurrasRepository : IRepository<Churras>
{
    Task<IEnumerable<Churras>> GetChurrasByIdsList(params string[] ids);
    Task<ChurrasWithAgenda> ChurrasCreation(Churras entity);
}