namespace Bank.Balance.Api.Applicacion.Features.Process;

public class ProcessService(IBalanceDbContext balanceDbContext, IServiceBusSenderService serviceBusSenderService) : IProcessService
{
    public async Task Execute(string message, string subscription)
    {
        switch (subscription)
        {
            case ReceivedSubscriptionsConstants.BALANCE_INITIATED:
                await BalanceInitiated(message);
                break;
            case ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED_BALANCE:
                await TransferConfirmedBalance(message);
                break;
            case ReceivedSubscriptionsConstants.TRANSFER_FAILED_BALANCE:
                await TransferFailedBalance(message);
                break;
        }
    }
    private async Task BalanceInitiated(string message)
    {
        BalanceEntity? balanceEntity = JsonConvert.DeserializeObject<BalanceEntity>(message);
        balanceEntity?.CurrentState = CurrentStateConstants.PENDING;
        BalanceEntity savedBalanceEntity = await ProcessDatabase(balanceEntity);

        var eventModel = new
        {
            balanceEntity.CorrelationId,
            balanceEntity.CustomerId
        };


        if (savedBalanceEntity.Id is not 0)
        {
            //MS Transaction
            await serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.BALANCE_CONFIRMED);
        }
        else
        {
            //MS Transaction
            await serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.BALANCE_FAILED);
        }
    }
    private async Task TransferConfirmedBalance(string message)
    {
        BalanceEntity? balanceEntity = JsonConvert.DeserializeObject<BalanceEntity>(message);
        balanceEntity?.CurrentState = CurrentStateConstants.COMPLETED;
        BalanceEntity savedBalanceEntity = await ProcessDatabase(balanceEntity);
    }
    private async Task TransferFailedBalance(string message)
    {
        BalanceEntity? balanceEntity = JsonConvert.DeserializeObject<BalanceEntity>(message);
        balanceEntity?.CurrentState = CurrentStateConstants.CANCELED;
        BalanceEntity savedBalanceEntity = await ProcessDatabase(balanceEntity);
    }

    public async Task<BalanceEntity> ProcessDatabase(BalanceEntity balanceEntity)
    {
        BalanceEntity? existEntity = await balanceDbContext.Balances.FirstOrDefaultAsync(x => x.CorrelationId == balanceEntity.CorrelationId);

        if (existEntity is null)
        {
            await balanceDbContext.Balances.AddAsync(balanceEntity);
            await balanceDbContext.SaveAsync();
            return balanceEntity;
        }
        else
        {
            existEntity.CurrentState = balanceEntity.CurrentState;
            balanceDbContext.Balances.Update(existEntity);
            await balanceDbContext.SaveAsync();
            return existEntity;
        }
    }
}
