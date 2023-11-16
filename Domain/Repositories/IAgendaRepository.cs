using Domain.Entities;

namespace Domain.Repositories;

public interface IAgendaRepository : IRepository<Agenda>
{
    Task<IEnumerable<Agenda>> GetAllAgendas();
    Task<Agenda> GetAgendaByChurrasId(string churrasId);
    Task<IEnumerable<Agenda>> GetAllAgendasWithBbq();
}