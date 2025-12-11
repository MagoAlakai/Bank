namespace Bank.Transaction.Api.Persistence.Database;

public class TransactionDbContext : DbContext, ITransactionDbContext
{
    public TransactionDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<TransactionEntity> Transactions { get; set; }

    public async Task<bool> SaveAsync()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}