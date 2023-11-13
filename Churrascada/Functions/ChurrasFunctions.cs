using System.Net;
using System.Text.Json;
using Churrascada.Extensions;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Churrascada.Functions;

public static class ChurrasFunctions
{
    [Function("ItWillReallyHappen")]
    public static HttpResponseData GetChurras(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            Route = "churras/{id}"
        )] HttpRequestData req,
        string id
    )
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString("Welcome to Azure Functions!");

        return response;
    }

    [Function("CallOutTheTroops")]
    public static async Task<HttpResponseData> PostChurras(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "post",
            Route = "churras"
        )] HttpRequestData req
    )
    {
        var body = req.Body;
        HttpResponseData response;
        if (body.Length <= 0 && req.Headers.TryGetValues("Content-Type", out var header))
        {
            response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var fullHeader = string.Join(";", header);
            await response.WriteAsJsonAsync(
                fullHeader.ToUpper().Contains("JSON")
                    ? new FailureMessage("no-code", "You should send a body with information!")
                    : new FailureMessage("no-code", "You should send a JSON body!")
            );

            return response;
        }
        
        response = req.CreateResponse(HttpStatusCode.OK);
        var churrasInfo = await body.ToObjectAsync<ChurrasInfo>();

        return response;
    }
}