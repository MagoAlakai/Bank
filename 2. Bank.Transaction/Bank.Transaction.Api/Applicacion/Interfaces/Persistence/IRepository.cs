namespace Bank.Transaction.Api.Applicacion.Interfaces.Persistence;
public interface IRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(int id);
    public Task<T?> PostAsync(T entity);
    public Task<T?> UpdateAsync(T entity, int id);
}
