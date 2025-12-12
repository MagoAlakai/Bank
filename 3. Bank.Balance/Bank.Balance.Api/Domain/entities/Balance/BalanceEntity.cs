namespace Bank.Balance.Api.Domain.Entities.Balance;
public class BalanceEntity
{
    public int Id { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime BalanceDate { get; set; } = DateTime.UtcNow;
    public string? CurrentState { get; set; }
    public int CustomerId { get; set; }
}
