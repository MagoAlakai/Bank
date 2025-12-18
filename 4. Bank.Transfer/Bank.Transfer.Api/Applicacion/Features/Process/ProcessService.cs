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
        transferEntity?.SourceAccount = "00908929778493-43984";
        transferEntity?.DestinationAccount = "32408929778493-43984";
        await ProcessDatabase(transferEntity!);

        var eventModel = new
        {
            transferEntity?.CorrelationId,
            transferEntity?.Amount,
            transferEntity?.SourceAccount,
            transferEntity?.DestinationAccount,
            transferEntity?.CustomerId,
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
