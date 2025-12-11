namespace Bank.Transfer.Api.Applicacion.Interfaces.Database;
public interface ITransferDbContext
{
    public DbSet<TransferEntity> Transfers{ get; set; }
    Task<bool> SaveAsync();
}
