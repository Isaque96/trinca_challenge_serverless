using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Web;
using Domain.Models.Responses;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;

namespace Domain.Abstractions;

public class PaginationAbstraction
{
    protected static PaginatedResponse<T> GetPaginatedResponse<T>(Container container, int pageSize, int pageNumber, HttpRequestData request)
    {
        var query = container.GetItemLinqQueryable<T>();
        var totalEntities = GetTotalEntities(query);
        var totalPages = GetTotalPages(pageSize, totalEntities);
        var skip = GetSkip(pageNumber, pageSize);
        var nextPage = CreatePageUrl(pageNumber, 1, pageSize, totalPages, request);
        var previousPage = CreatePageUrl(pageNumber, -1, pageSize, totalPages, request);
        var entities = GetEntities(query, skip, pageSize);

        return new PaginatedResponse<T>(pageNumber, pageSize, nextPage, previousPage, totalPages, totalEntities, entities);
    }

    private static T[] GetEntities<T>(IQueryable<T> query, int skip, int pageSize)
        => query.Skip(skip).Take(pageSize).ToArray();

    private static int GetTotalPages(int pageSize, int totalEntities)
        => (int)Math.Ceiling((double)totalEntities / pageSize);

    private static int GetSkip(int pageNumber, int pageSize)
            => (pageNumber - 1) * pageSize;

    private static int GetTotalEntities<T>(IQueryable<T> entities)
        => entities.Count();

    private static string CreateBaseUrl(HttpRequestData request)
        => request.Url.Scheme + "://" + request.Url.Host + request.Url.AbsolutePath;

    private static string? GetRestOfQuery(int pageNumber, int toGoPage, int pageSize, HttpRequestData request)
    {
        var parse = HttpUtility.ParseQueryString(string.Empty);
        parse.Add("pageNumber", (pageNumber + toGoPage).ToString());
        parse.Add("pageSize", pageSize.ToString());

        request.Query
            .Cast<KeyValuePair<string, string>>()
            .Where(vl => vl.Key != "pageNumber" && vl.Key != "pageSize")
            .ToList()
            .ForEach(kv => parse.Add(kv.Key, kv.Value));

        return parse.ToString();
    }

    private static string? CreatePageUrl(int pageNumber, int toGoPage, int pageSize, int totalPages, HttpRequestData request)
    {            
        var newPageNumber = pageNumber + toGoPage;

        if (newPageNumber > totalPages ||
            newPageNumber < 1)
            return null;

        return CreateBaseUrl(request) + "?" + GetRestOfQuery(pageNumber, toGoPage, pageSize, request);
    }
}