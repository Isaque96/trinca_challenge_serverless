using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace Churrascada.Middlewares;

public class OnErrorMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            var requestData = await context.GetHttpRequestDataAsync();

            if (requestData != null)
            {
                var response = requestData.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                await response.WriteStringAsync(e.ToString());
                
                context.GetInvocationResult().Value = response;
            }
        }
    }
}