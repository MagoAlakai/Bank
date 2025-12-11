namespace Bank.Balance.Api.Persistence.Database;

public class BalanceDbContext : DbContext, IBalanceDbContext
{
    public BalanceDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<BalanceEntity> Balances { get; set; }

    public async Task<bool> SaveAsync()
    {
        return await SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}