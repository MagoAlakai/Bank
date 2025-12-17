namespace Bank.Transaction.Api.Persistence.Repositories;

public class UnitOfWork(TransactionDbContext transactionDbContext) : IUnitOfWork
{
    private readonly IRepository<TransactionEntity>? _transactionRepository;
    public IRepository<TransactionEntity> transactionRepository => _transactionRepository ?? new BaseRepository<TransactionEntity>(transactionDbContext);

    void IDisposable.Dispose()
    {
        transactionDbContext.Dispose();
    }

    void IUnitOfWork.SaveChanges()
    {
        transactionDbContext.SaveChanges();
    }

    async Task IUnitOfWork.SaveChangesAsync()
    {
       await transactionDbContext.SaveChangesAsync();
    }
}
