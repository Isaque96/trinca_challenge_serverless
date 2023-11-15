using System.Net;
using Domain.Abstractions;
using Domain.Database;
using Domain.Entities;
using Domain.Models.Responses;
using Domain.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Repositories;

public class AgendaRepository : PaginationAbstraction, IAgendaRepository
{
    private readonly Container _agendas;

    public AgendaRepository(CosmosConn cosmosConn)
    {
        _agendas = cosmosConn.GetContainer("Agendas");
    }
    
    public Task<PaginatedResponse<Agenda>> GetPaginatedAsync(HttpRequestData? req, int pageNumber = 1, int pageSize = 50)
    {
        if (req == null)
            throw new ArgumentException("You should send the request!");

        var response = GetPaginatedResponse<Agenda>(_agendas, pageSize, pageNumber, req);
        
        return Task.FromResult(response);
    }

    public async Task<Agenda> GetByIdAsync(string id)
    {
        Agenda agenda;
        try
        {
            agenda = await _agendas.ReadItemAsync<Agenda>(
                id, new PartitionKey(id)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            agenda = null!;
        }

        return agenda;
    }

    public async Task<Agenda> AddAsync(Agenda entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
            entity.Id = Guid.NewGuid().ToString();
        
        Agenda agenda;
        try
        {
            agenda = await _agendas.CreateItemAsync(entity, new PartitionKey(entity.Id));
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            agenda = null!;
        }

        return agenda;
    }

    public async Task<Agenda> UpdateAsync(Agenda entity)
    {
        var item = await _agendas.ReadItemAsync<Agenda>(entity.Id, new PartitionKey(entity.Id));
        var agenda = item.Resource;

        var agendaToUpdate = Util.UpdateLogic(entity, agenda);

        item = await _agendas.ReplaceItemAsync(
            agendaToUpdate,
            agendaToUpdate.Id,
            new PartitionKey(agendaToUpdate.Id)
        );

        return item.Resource;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var item = await _agendas.DeleteItemAsync<Agenda>(id, new PartitionKey(id));

        return item.StatusCode == HttpStatusCode.NoContent;
    }
}