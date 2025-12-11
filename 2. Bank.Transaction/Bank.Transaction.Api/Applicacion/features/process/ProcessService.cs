namespace Bank.Transaction.Api.Applicacion.Features.Process;
public class ProcessService(ITransactionDbContext transactionDbContext, IServiceBusSenderService serviceBusSenderService) : IProcessService
{
    public async Task Execute(string message, string subscription)
    {
        switch (subscription)
        {
            case ReceivedSubscriptionsConstants.TRANSACTION_INITIATED:
                await Transaction_initiated(message);
                break;
            case ReceivedSubscriptionsConstants.BALANCE_CONFIRMED:
                await BalanceConfirmed(message);
                break;
            case ReceivedSubscriptionsConstants.BALANCE_FAILED:
                await BalanceFailed(message);
                break;
            case ReceivedSubscriptionsConstants.TRANSFER_FAILED:
                await TransferFailed(message);
                break;
            case ReceivedSubscriptionsConstants.TRANSFER_CONFIRMED:
                await TransferConfirmed(message);
                break;
        }
    }

    private async Task Transaction_initiated(string message)
    {
        TransactionEntity? transactionEntity = JsonConvert.DeserializeObject<TransactionEntity>(message);

        transactionEntity?.CurrentState = CurrentStateConstants.PENDING;
        transactionEntity?.SourceAccount = "00908929778493-43984";
        transactionEntity?.DestinationAccount = "32408929778493-43984";
        TransactionEntity? savedTransactionEntity = await ProcessDatabase(transactionEntity);
        var eventModel = new
        {
            savedTransactionEntity.CorrelationId,
            savedTransactionEntity.CustomerId,
        };

        if (savedTransactionEntity.Id is not 0)
        {
            await serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.BALANCE_INITIATED);
        }
        else
        {
            await serviceBusSenderService.Execute(eventModel, SendSubscriptionConstants.TRANSACTION_FAILED);
        }  
    }

    private async Task BalanceConfirmed(string message)
    {
        throw new NotImplementedException();
    }

    private async Task BalanceFailed(string message)
    {
        throw new NotImplementedException();
    }

    private async Task TransferFailed(string message)
    {
        throw new NotImplementedException();
    }

    private async Task TransferConfirmed(string message)
    {
        throw new NotImplementedException();
    }

    public async Task<TransactionEntity> ProcessDatabase(TransactionEntity transactionEntity)
    {
        TransactionEntity? existEntity = await transactionDbContext.Transactions.FirstOrDefaultAsync(x => x.CorrelationId == transactionEntity.CorrelationId);

        if (existEntity is null)
        {
            await transactionDbContext.Transactions.AddAsync(transactionEntity);
            await transactionDbContext.SaveAsync();
            return transactionEntity;
        }
        else
        {
            existEntity.CurrentState = transactionEntity.CurrentState;
            transactionDbContext.Transactions.Update(existEntity);
            await transactionDbContext.SaveAsync();
            return existEntity;
        }
    }
}