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
    private readonly Container _agendas;

    public PersonRepository(CosmosConn cosmosConn)
    {
        _agendas = cosmosConn.GetContainer("Persons");
    }

    public Task<PaginatedResponse<Person>> GetPaginatedAsync(HttpRequestData? req, int pageNumber = 1, int pageSize = 50)
    {
        if (req == null)
            throw new ArgumentException("You should send the request!");

        var response = GetPaginatedResponse<Person>(_agendas, pageSize, pageNumber, req);
        
        return Task.FromResult(response);
    }

    public async Task<Person> GetByIdAsync(string id)
    {
        Person person;
        try
        {
            person = await _agendas.ReadItemAsync<Person>(
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
            person = await _agendas.CreateItemAsync(entity, new PartitionKey(entity.Id));
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
        var item = await _agendas.ReadItemAsync<Person>(entity.Id, new PartitionKey(entity.Id));
        var person = item.Resource;

        var personToUpdate = Util.UpdateLogic(entity, person);

        item = await _agendas.ReplaceItemAsync(
            personToUpdate,
            personToUpdate.Id,
            new PartitionKey(personToUpdate.Id)
        );

        return item.Resource;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var item = await _agendas.DeleteItemAsync<Person>(id, new PartitionKey(id));

        return item.StatusCode == HttpStatusCode.NoContent;
    }
}