namespace Bank.Transfer.Api.Applicacion.Features.Process;

public interface IProcessService
{
    Task Execute(string message);
}
