using Domain.Entities;

namespace Domain.Repositories;

public interface IAgendaRepository : IRepository<Agenda>
{
    Task<Agenda> GetAgendaByChurrasId(string id);
    Task<IEnumerable<Agenda>> GetAllAgendasWithBbq();
}