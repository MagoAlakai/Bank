namespace Bank.Gateway.Api.Application.Features;

public class ProcessService (IServiceBusSenderService serviceBusSenderService) : IProcessService
{
    public async Task Execute(EndPointModel endPointModel)
    {
        var modelEvent = new
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Amount= endPointModel.Amount,
            SourceAccount = "00908929778493-43984",
            DestinationAccount = "32408929778493-43984",
            CustomerId = endPointModel.CustomerId
        };

        await serviceBusSenderService.Execute(modelEvent, SendSubscriptionConstants.TRANSACTION_INITIATED);
    }
}
