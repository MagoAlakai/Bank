namespace Bank.Transaction.Api.Applicacion.Interfaces.Database;
public interface ITransactionDbContext
{
    public DbSet<TransactionEntity> Transactions { get; set; }
    Task<bool> SaveAsync();
}
