namespace Bank.Transfer.Api.External.ServiceBusSender;

public class ServiceBusSenderService : IServiceBusSenderService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly string? _topicName;

    public ServiceBusSenderService(IConfiguration configuration)
    {
        _serviceBusClient = new ServiceBusClient(configuration["SERVICEBUSCONSTR"]);
        _topicName = configuration["SERVICEBUSTOPIC"];
    }

    public async Task Execute(object eventModel, string subscription)
    {
        if (string.IsNullOrEmpty(_topicName))
        {
            throw new ArgumentNullException("Service Bus topic name is not configured.");
        }

        await using var sender = _serviceBusClient.CreateSender(_topicName);
        string messageBody = System.Text.Json.JsonSerializer.Serialize(eventModel);

        ServiceBusMessage busMessage = new ServiceBusMessage(messageBody);
        busMessage.ContentType = "application/json";
        busMessage.Subject = subscription;

        await sender.SendMessageAsync(busMessage);
    }
}
