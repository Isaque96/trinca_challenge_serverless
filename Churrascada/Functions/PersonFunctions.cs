using System.Net;
using System.Text;
using Churrascada.Extensions;
using Domain.Models.Requests;
using Domain.Models.Responses;
using Domain.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Churrascada.Functions;

public class PersonFunctions
{
    private readonly IPersonRepository _personRepository;

    public PersonFunctions(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    [Function("DidSomeoneSayBbq")]
    public async Task<HttpResponseData> GetNonAcceptedChurras(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "get",
            Route = "person/invites"
        )] HttpRequestData req
    )
    {
        var response = req.CreateResponse();
        if (!req.Headers.TryGetValues("personId", out var personIds))
            return await HeaderIsMissing(response);

        var personId = personIds.FirstOrDefault();
        var bbqs = await _personRepository.GetAllNotAcceptedChurras(personId);

        response.StatusCode = HttpStatusCode.OK;
        if (bbqs.Any())
        {
            await response.WriteAsJsonAsync(bbqs);
        }
        else
        {
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("Ual, você está por dentro! Não haviam mais churras pendentes!");
        }
        
        return response;
    }
    
    [Function("AcceptInvite")]
    public async Task<HttpResponseData> AcceptInvite(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "put",
            Route = "person/invites/{churrasId}/accept"
        )] HttpRequestData req,
        string? churrasId
    )
    {
        var response = req.CreateResponse();
        var body = req.Body;
        if (body.Length <= 0 && req.Headers.TryGetValues("Content-Type", out var header))
            if (IsVegan(ref response, body, header))
                return response;
        
        if (!req.Headers.TryGetValues("personId", out var personIds))
            return await HeaderIsMissing(response);

        var personId = personIds.FirstOrDefault();
        var accepted = await _personRepository.AcceptBbqInvite(personId, churrasId);

        response.StatusCode = HttpStatusCode.OK;
        await response.WriteAsJsonAsync(accepted);
        
        return response;
    }
    
    [Function("DeclineInvite")]
    public async Task<HttpResponseData> DeclineInvite(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "put",
            Route = "person/invites/{churrasId}/decline"
        )] HttpRequestData req,
        string? churrasId
    )
    {
        var response = req.CreateResponse();
        var body = req.Body;
        if (body.Length <= 0 && req.Headers.TryGetValues("Content-Type", out var header))
            if (IsVegan(ref response, body, header))
                return response;
        
        if (!req.Headers.TryGetValues("personId", out var personIds))
            return await HeaderIsMissing(response);

        var personId = personIds.FirstOrDefault();
        var accepted = await _personRepository.DeclineBbqInvite(personId, churrasId);

        response.StatusCode = HttpStatusCode.OK;
        await response.WriteAsJsonAsync(accepted);
        
        return response;
    }

    private static bool IsVegan(ref HttpResponseData response, Stream body, IEnumerable<string?> header)
    {
        if (!header.FirstOrDefault().ToUpper().Contains("JSON"))
            return false;
        
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        var vegan = body.ToObjectAsync<VeganBbq>().Result;

        if (!vegan.IsVegan)
            return false;

        var strB = new StringBuilder("YOU SHALL NOT PASS!!!!");
        strB.AppendLine("Apenas uma brincadeira!");
        strB.AppendLine("Ainda não sabemos como preparar um churras Vegano, está sendo implementado!! Hahahaha");
        
        response.WriteString(strB.ToString());
        
        return true;
    }
    
    private static async Task<HttpResponseData> HeaderIsMissing(HttpResponseData response)
    {
        response.StatusCode = HttpStatusCode.BadRequest;
        await response.WriteAsJsonAsync(
            new FailureMessage(
                "no-code",
                "The request Header 'personId' is required!"
            )
        );

        return response;
    }
}