namespace Bank.Transaction.Api.Applicacion.Interfaces.Database;
public interface IBalanceDbContext
{
    public DbSet<BalanceEntity> Balances{ get; set; }
    Task<bool> SaveAsync();
}
