using System.Net;
using Domain.Abstractions;
using Domain.Database;
using Domain.Entities;
using Domain.Models.Responses;
using Domain.Utils;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Repositories;

public class ChurrasRepository : PaginationAbstraction, IChurrasRepository
{
    private readonly Container _churras;
    
    public ChurrasRepository(CosmosConn cosmos)
    {
        _churras = cosmos.GetContainer("Churras");
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
        if (string.IsNullOrEmpty(entity.Id))
            entity.Id = Guid.NewGuid().ToString();
        
        Churras churras;
        try
        {
            churras = await _churras.CreateItemAsync(entity, new PartitionKey(entity.Id));
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            churras = null!;
        }

        return churras;
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

        return item.Resource;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var item = await _churras.DeleteItemAsync<Churras>(id, new PartitionKey(id));

        return item.StatusCode == HttpStatusCode.NoContent;
    }
}