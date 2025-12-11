namespace Bank.Transaction.Api.Applicacion.Features.Process;

public interface IProcessService
{
    Task Execute(string message, string subscription);
}
