namespace Bank.Transaction.Api.Persistence.Repositories;

public class BaseRepository<T>(TransactionDbContext transactionDbContext) : IRepository<T> where T : TransactionEntity
{
    private readonly DbSet<T> _entities = transactionDbContext.Set<T>();

    public async Task<T?> GetByCorrelationIdAsync(string correlationId)
    {
        return await _entities.FirstOrDefaultAsync(e => e.CorrelationId == correlationId);
    }

    public async Task<T?> PostAsync(T entity)
    {
        if (entity is null)
            throw new InvalidOperationException("Entity cannot be null or empty for insert.");

        bool entity_exist = await _entities.AnyAsync(x => x.CorrelationId == entity.CorrelationId);
        if (entity_exist is true) return null;

        EntityEntry<T> entry = await _entities.AddAsync(entity);

        return entry.Entity;
    }

    public async Task<T?> UpdateAsync(T entity, string correlationId)
    {
        var existing = await _entities.FirstOrDefaultAsync(x => x.CorrelationId == correlationId);
        if (existing is null) return null;

        T updated_entity = _entities.Update(entity).Entity;

        return updated_entity;
    }
}
