namespace Bank.Transaction.Api.Persistence.Repositories;

public class BaseRepository<T>(TransactionDbContext transactionDbContext) : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _entities = transactionDbContext.Set<T>();

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _entities.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T?> PostAsync(T entity)
    {
        if (entity is null)
            throw new InvalidOperationException("Entity cannot be null or empty for insert.");

        bool entity_exist = await _entities.AnyAsync(x => x.Id == entity.Id);
        if (entity_exist is true) return null;

        EntityEntry<T> entry = await _entities.AddAsync(entity);

        return entry.Entity;
    }

    public async Task<T?> UpdateAsync(T entity, int id)
    {
        bool entity_exist = await _entities.AnyAsync(x => x.Id == entity.Id);
        if (entity_exist is false) return null;

        T updated_entity = _entities.Update(entity).Entity;

        return updated_entity;
    }
}
