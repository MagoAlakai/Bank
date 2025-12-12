namespace Bank.Transfer.Api.Applicacion.Features.Handlers;
public class ProcessHandler(IServiceProvider serviceProvider) : INotificationHandler<ProcessEvent>
{
    public async Task Handle(ProcessEvent processEvent, CancellationToken cancellationToken)
    {

        if (processEvent.Message is null || processEvent.Subscription is null)
        {
            throw new ArgumentNullException("ProcessEvent properties cannot be null");
        }

        using IServiceScope scope = serviceProvider.CreateScope();
        IProcessService processService = scope.ServiceProvider.GetRequiredService<IProcessService>();

        await processService.Execute(processEvent.Message);
    }
}
