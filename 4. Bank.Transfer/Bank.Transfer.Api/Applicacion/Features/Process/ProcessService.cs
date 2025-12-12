namespace Bank.Transfer.Api.Applicacion.Features.Process;
public class ProcessService(ITransferDbContext transferDbContext, IServiceBusSenderService serviceBusSenderService) : IProcessService
{
    public async Task Execute(string message)
    {
        await TransferInitiated(message);
    }

    private async Task TransferInitiated(string message)
    {
        TransferEntity? transferEntity = JsonConvert.DeserializeObject<TransferEntity>(message);
        await ProcessDatabase(transferEntity!);

        var eventModel = new
        {
            transferEntity?.CorrelationId,
            transferEntity?.CustomerId
        };

        if (transferEntity?.Id is not 0)
        {
            await serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSFER_CONFIRMED);
        }
        else
        {
            await serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSFER_FAILED);
        }
    }

    public async Task<TransferEntity> ProcessDatabase(TransferEntity transferEntity)
    {
        transferEntity.CurrentState = CurrentStateConstants.COMPLETED;
        await transferDbContext.Transfers.AddAsync(transferEntity);
        await transferDbContext.SaveAsync();
        return transferEntity;
    }
}
