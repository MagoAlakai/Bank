namespace Bank.Gateway.Api.Api.Endpoint;

public static class ApigatewayEndPoint
{
    public static void GetwayEndpoint (WebApplication app)
    {
        app.MapPost("/api-gateway", async ([FromBody] EndPointModel modelRequest, [FromServices] IProcessService processService) => 
        {
            await processService.Execute(modelRequest);
            return Results.Ok(modelRequest);
        });
    }
}
