namespace Bank.Balance.Api.Applicacion.Features.Process;
public interface IProcessService
{
    Task Execute(string message, string subscription);
}
