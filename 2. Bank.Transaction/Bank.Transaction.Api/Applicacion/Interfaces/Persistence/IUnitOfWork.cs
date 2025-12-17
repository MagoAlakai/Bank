namespace Bank.Transaction.Api.Applicacion.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IRepository<TransactionEntity> transactionRepository { get; }
    void SaveChanges();
    Task SaveChangesAsync();
}
