namespace Bank.Notification.Api.External.ServiceBusRecieve;
public class ServiceBusRecieveService : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly List<ServiceBusProcessor> _serviceBusProcessors;
    private readonly ServiceBusClient _serviceBusClient;
    public ServiceBusRecieveService(IConfiguration configuration, IMediator mediator)
    {
        _mediator = mediator;
        _serviceBusClient = new ServiceBusClient(configuration["SERVICEBUSCONSTR"]);

        string[] subscriptions = new[]
        {
            ReceivedSubscriptionsConstants.TRANSACTION_COMPLETED,
            ReceivedSubscriptionsConstants.TRANSACTION_FAILED
        };

        _serviceBusProcessors = subscriptions.Select(subscription =>
        {
            ServiceBusProcessor processor = _serviceBusClient.CreateProcessor(configuration["SERVICEBUSTOPIC"], subscription);

            processor.ProcessMessageAsync += async args => await Process(args, subscription);
            processor.ProcessErrorAsync += ProcessError;

            return processor;
        }).ToList();
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.WhenAll(_serviceBusProcessors.Select(p => p.StartProcessingAsync()));
        await Task.Run(() => stoppingToken.WaitHandle.WaitOne(), stoppingToken);
    }

    private async Task Process(ProcessMessageEventArgs args, string subscription)
    {
        string body = args.Message.Body.ToString();

        await _mediator.Publish(new ProcessEvent(body, subscription));
        await args.CompleteMessageAsync(args.Message);
    }

    private Task ProcessError(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"[ERROR] Entity={args.EntityPath} Reason={args.Exception.Message}");
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.WhenAll(_serviceBusProcessors.Select(p => p.StopProcessingAsync()));
        await base.StopAsync(cancellationToken);
    }

    public override async void Dispose()
    {
        await Task.WhenAll(_serviceBusProcessors.Select(p => p.DisposeAsync().AsTask()));
        await _serviceBusClient.DisposeAsync();
        base.Dispose();
    }
}