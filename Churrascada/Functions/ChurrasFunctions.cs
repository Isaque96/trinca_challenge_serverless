using System.Net;
using Churrascada.Extensions;
using Domain.Entities;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Domain.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Churrascada.Functions;

public class ChurrasFunctions
{
    private readonly IChurrasRepository _repository;

    public ChurrasFunctions(IChurrasRepository repository)
    {
        _repository = repository;
    }
    
    [Function("ItWillReallyHappen")]
    public async Task<HttpResponseData> GetChurras(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            Route = "churras/{id}"
        )] HttpRequestData req,
        string id
    )
    {
        var churras = await _repository.GetByIdAsync(id);
        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        if (churras == null!)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            await response.WriteStringAsync("We couldn't find the BBQ you select!");
        }
        else
        {
            response.StatusCode = HttpStatusCode.OK;
            var couldParse = Enum.TryParse<ChurrasStatus>(churras.Status, out var status);
            string message;
            if (couldParse)
            {
                message = status switch
                {
                    ChurrasStatus.Confirmed => "UHUUUU! Vai rolar!",
                    ChurrasStatus.PendingConfirmations => "Ainda no aguardo!",
                    ChurrasStatus.ItsNotGonnaHappen => "Puxa! É, fica pra próxima!",
                    ChurrasStatus.New => "O pessoal está bem agarrado, ninguém viu a solicitação ainda!",
                    _ => "Whoops! Something went wrong with the BBQ status!"
                };
            }
            else
            {
                message = "Whoops! Something went wrong we couldn't understand the BBQ status!";
            }
            
            await response.WriteStringAsync(message);
        }

        return response;
    }

    [Function("CallOutTheTroops")]
    public async Task<HttpResponseData> PostChurras(
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
            return await BodyValidation(req, header);
        
        var churrasInfo = await body.ToObjectAsync<ChurrasInfo>();

        var churras = await _repository.AddAsync(churrasInfo!.CreateEntity());

        if (churras != null!)
        {
            response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(churras);            
        }
        else
        {
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new FailureMessage("no-code", "Something went wrong"));
        }
        
        return response;
    }

    [Function("HasBackingOfPartners")]
    public async Task<HttpResponseData> PutChurras(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "put",
            Route = "churras/{id}/moderar"
        )]
        HttpRequestData req,
        string id
    )
    {
        var body = req.Body;
        HttpResponseData response;
        if (body.Length <= 0 && req.Headers.TryGetValues("Content-Type", out var header))
            return await BodyValidation(req, header);
        
        var itGonnaHappen = await body.ToObjectAsync<ItGonnaHappen>();

        var churras = await _repository.UpdateAsync(new Churras
        {
            Id = id,
            Status = itGonnaHappen!.GonnaHappen ?
                ChurrasStatus.Confirmed.ToString() :
                ChurrasStatus.ItsNotGonnaHappen.ToString(),
            IsTrincasPaying = itGonnaHappen.TrincaWillPay
        });

        response = req.CreateResponse();
        if (churras != null!)
        {
            response.StatusCode = churras.Status == ChurrasStatus.Confirmed.ToString() ?
                HttpStatusCode.Accepted :
                HttpStatusCode.OK;

            await response.WriteAsJsonAsync(churras);
        }
        else
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            await response.WriteAsJsonAsync(new FailureMessage("no-code", "Whoops! Something went wrong!"));
        }

        return response;
    }
    
    private static async Task<HttpResponseData> BodyValidation(HttpRequestData req, IEnumerable<string> header)
    {
        HttpResponseData response;
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
}