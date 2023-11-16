using System.Net;
using Domain.Abstractions;
using Domain.Database;
using Domain.Entities;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Domain.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Repositories;

public class ChurrasRepository : PaginationAbstraction, IChurrasRepository
{
    private readonly Container _churras;
    private readonly IAgendaRepository _agendaRepository;

    public ChurrasRepository(CosmosConn cosmos, IAgendaRepository agendaRepository)
    {
        _churras = cosmos.GetContainer("Churras");
        _agendaRepository = agendaRepository;
    }

    public Task<PaginatedResponse<Churras>> GetPaginatedAsync(HttpRequestData? req, int pageNumber = 1, int pageSize = 50)
    {
        if (req == null)
            throw new ArgumentException("You should send the request!");

        var response = GetPaginatedResponse<Churras>(_churras, pageSize, pageNumber, req);
        
        return Task.FromResult(response);
    }

    public async Task<Churras> GetByIdAsync(string id)
    {
        Churras churras;
        try
        {
            churras = await _churras.ReadItemAsync<Churras>(
                id, new PartitionKey(id)
            );
        }
        catch (Exception e)
        {
            // TODO: Implement Logs
            Console.WriteLine(e);
            churras = null!;
        }

        return churras;
    }

    public async Task<Churras> AddAsync(Churras entity)
    {
        var churras = await ChurrasCreation(entity);

        return churras.Churras;
    }

    public async Task<ChurrasWithAgenda> ChurrasCreation(Churras entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
            entity.Id = Guid.NewGuid().ToString();
        
        ChurrasWithAgenda cwa = new();
        try
        {
            cwa.Churras = await _churras.CreateItemAsync(entity, new PartitionKey(entity.Id));
            cwa.Agenda = await _agendaRepository.AddAsync(
                CreateAgendaFromChurras(cwa.Churras)
            );
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            cwa = null!;
        }

        return cwa;
    }

    public async Task<Churras> UpdateAsync(Churras entity)
    {
        var item = await _churras.ReadItemAsync<Churras>(entity.Id, new PartitionKey(entity.Id));
        var churras = item.Resource;

        var churrasToUpdate = Util.UpdateLogic(entity, churras);

        item = await _churras.ReplaceItemAsync(
            churrasToUpdate,
            churrasToUpdate.Id,
            new PartitionKey(churrasToUpdate.Id)
        );

        var agenda = await _agendaRepository.GetAgendaByChurrasId(item.Resource.Id);
        await _agendaRepository.UpdateAsync(CreateAgendaFromChurras(item.Resource, agenda.Id));
        
        return item.Resource;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var item = await _churras.DeleteItemAsync<Churras>(id, new PartitionKey(id));

        return item.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<IEnumerable<Churras>> GetChurrasByIdsList(params string[] ids)
    {
        // https://github.com/Azure/azure-cosmosdb-node/issues/156
        // The supposedly simple query, "SELECT * FROM c WHERE c.id IN (@IdsList)" doesn't work, there's a workaround at this link
        var query = new QueryDefinition("SELECT * FROM c WHERE ARRAY_CONTAINS(@IdsList, c.id)")
            .WithParameter("@IdsList", ids);
        
        var churrasEntities = _churras.GetItemQueryIterator<Churras>(query);

        List<Churras> churrasList = new();
        while (churrasEntities.HasMoreResults)
        {
            var agendasResponse = await churrasEntities.ReadNextAsync();
            churrasList.AddRange(agendasResponse);
        }

        return churrasList;
    }

    private static Agenda CreateAgendaFromChurras(Churras churras, string? id = null)
    {
        return new Agenda(
            churras.Reason,
            churras.Date,
            DateTime.UtcNow >= churras.Date.AddHours(1),
            id,
            churras.Id
        );
    }
}