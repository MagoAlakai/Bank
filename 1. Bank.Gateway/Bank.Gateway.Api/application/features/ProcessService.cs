namespace Bank.Gateway.Api.Application.Features;

public class ProcessService (IServiceBusSenderService serviceBusSenderService) : IProcessService
{
    public async Task Execute(EndPointModel endPointModel)
    {
        var modelEvent = new
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Amount= endPointModel.Amount,
            SourceAccount = endPointModel.SourceAccount,
            DestinationAccount = endPointModel.DestinationAccount,
            CustomerId = endPointModel.CustomerId
        };

        await serviceBusSenderService.Execute(modelEvent, SendSubscriptionConstants.TRANSACTION_INITIATED);
    }
}
