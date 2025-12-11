namespace Bank.Balance.Api.Persistence.Database;

public class TransferDbContext : DbContext, ITransferDbContext
{
    public TransferDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<TransferEntity> Transfers { get; set; }

    public async Task<bool> SaveAsync()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}