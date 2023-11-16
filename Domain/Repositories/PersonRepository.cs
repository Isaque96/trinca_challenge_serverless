using System.Net;
using Domain.Abstractions;
using Domain.Database;
using Domain.Entities;
using Domain.Models.Responses;
using Domain.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Repositories;

public class PersonRepository : PaginationAbstraction, IPersonRepository
{
    private readonly Container _persons;
    private readonly IAgendaRepository _agendaRepository;
    private readonly IChurrasRepository _churrasRepository;

    public PersonRepository(CosmosConn cosmosConn, IAgendaRepository agendaRepository, IChurrasRepository churrasRepository)
    {
        _persons = cosmosConn.GetContainer("Persons");
        _agendaRepository = agendaRepository;
        _churrasRepository = churrasRepository;
    }

    public Task<PaginatedResponse<Person>> GetPaginatedAsync(HttpRequestData? req, int pageNumber = 1, int pageSize = 50)
    {
        if (req == null)
            throw new ArgumentException("You should send the request!");

        var response = GetPaginatedResponse<Person>(_persons, pageSize, pageNumber, req);
        
        return Task.FromResult(response);
    }

    public async Task<Person> GetByIdAsync(string id)
    {
        Person person;
        try
        {
            person = await _persons.ReadItemAsync<Person>(
                id, new PartitionKey(id)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            person = null!;
        }

        return person;
    }

    public async Task<Person> AddAsync(Person entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
            entity.Id = Guid.NewGuid().ToString();
        
        Person person;
        try
        {
            person = await _persons.CreateItemAsync(entity, new PartitionKey(entity.Id));
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            person = null!;
        }

        return person;
    }

    public async Task<Person> UpdateAsync(Person entity)
    {
        try
        {
            var item = await _persons.ReadItemAsync<Person>(entity.Id, new PartitionKey(entity.Id));
            var person = item.Resource;

            var personToUpdate = Util.UpdateLogic(entity, person);

            item = await Update(personToUpdate);

            return item.Resource;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var item = await _persons.DeleteItemAsync<Person>(id, new PartitionKey(id));

        return item.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<IEnumerable<Churras>> GetAllNotAcceptedChurras(string id)
    {
        var allAgendasWithBbq = await _agendaRepository.GetAllAgendasWithBbq();
        var person = await GetByIdAsync(id);
    
        var notAcceptedAgendas = allAgendasWithBbq
            .Where(agenda => !person.AgendasIds.Contains(agenda.Id));
    
        var bbqs = await _churrasRepository.GetChurrasByIdsList(
            notAcceptedAgendas.Select(a => a.ChurrasId).ToArray()
        );
    
        return bbqs;
    }

    public async Task<Person> VinculateBbq(string id, Agenda agenda)
    {
        var person = await GetByIdAsync(id);
        person.AgendasIds.Add(agenda.Id);
        
        return await Update(person);
    }

    public async Task<AcceptedInvite> AcceptBbqInvite(string id, string churrasId)
    {
        var person = await GetByIdAsync(id);
        if (person == null)
            return new AcceptedInvite(
                null,
                "Pessoa não encontrada na nossa base de dados!",
                null,
                null
            );
        
        var agenda = await _agendaRepository.GetAgendaByChurrasId(churrasId);
        if (agenda == null)
            return new AcceptedInvite(
                person.Name,
                "Convite Inexistente!",
                null,
                null
            );
        
        person.AgendasIds.Add(agenda.Id);

        var updatedPerson = await Update(person);

        return new AcceptedInvite(
            updatedPerson.Resource.Name,
            "Convite aceito com Sucesso!",
            agenda.AgendaTitle,
            agenda.Id
        );
    }
    
    public async Task<DeclinedInvite> DeclineBbqInvite(string id, string churrasId)
    {
        var person = await GetByIdAsync(id);
        if (person == null)
            return new DeclinedInvite(
                null,
                "Pessoa não encontrada na nossa base de dados!",
                null
            );
        
        var agenda = await _agendaRepository.GetAgendaByChurrasId(churrasId);
        if (agenda == null)
            return new DeclinedInvite(
                person.Name,
                "Convite Inexistente!",
                null
            );

        if (person.AgendasIds.Contains(agenda.Id))
        {
            person.AgendasIds.Remove(agenda.Id);
            person = await Update(person);
        }
        
        return new DeclinedInvite(
            person.Name,
            "Convite recusado",
            agenda.Id
        );
    }
    
    private async Task<ItemResponse<Person>> Update(Person person)
        => await _persons.ReplaceItemAsync(
            person,
            person.Id,
            new PartitionKey(person.Id)
        );
}