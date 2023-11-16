using Domain.Entities;
using Domain.Models.Responses;

namespace Domain.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<IEnumerable<Churras>> GetAllNotAcceptedChurras(string id);
    Task<Person> VinculateBbq(string id, Agenda agenda);
    Task<AcceptedInvite> AcceptBbqInvite(string id, string churrasId);
    Task<DeclinedInvite> DeclineBbqInvite(string id, string churrasId);
}