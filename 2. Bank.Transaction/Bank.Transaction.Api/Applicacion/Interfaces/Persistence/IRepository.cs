namespace Bank.Transaction.Api.Applicacion.Interfaces.Persistence;
public interface IRepository<T> where T : TransactionEntity
{
    public Task<T?> GetByCorrelationIdAsync(string correlationId);
    public Task<T?> PostAsync(T entity);
    public Task<T?> UpdateAsync(T entity, string id);
}
