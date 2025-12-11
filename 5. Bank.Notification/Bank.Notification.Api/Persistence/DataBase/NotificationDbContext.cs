namespace Bank.Notification.Api.Persistence.Database;

public class NotificationDbContext : INotificationDbContext
{
    private readonly CosmosClient cosmosClient;
    private readonly Container container;
    public NotificationDbContext(IConfiguration configuration)
    {
        string connectionString = configuration["NOTIFICATIONSDBCONNECTIONSTRING"] ?? throw new ArgumentNullException("CosmosDb connection string is not configured.");
        string databaseName = configuration["NOTIFICATIONSDBNAME"] ?? throw new ArgumentNullException("CosmosDb database name is not configured.");
        string containerName = configuration["NOTIFICATIONSDBCONTAINER"] ?? throw new ArgumentNullException("CosmosDb container name is not configured.");
    
    cosmosClient = new CosmosClient(connectionString);
    container = cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<bool> AddAsync(NotificationEntity notificationEntity)
    {
        notificationEntity.Id = Guid.NewGuid().ToString();
        notificationEntity.NotificationDate = DateTime.UtcNow;
        ItemResponse<NotificationEntity> response = await container.CreateItemAsync(notificationEntity, new PartitionKey(notificationEntity.CorrelationId));

        return response.StatusCode == System.Net.HttpStatusCode.Created;
    }

    public async Task<List<NotificationEntity>> GetAllAsync()
    {
        var query = container.GetItemQueryIterator<NotificationEntity>("SELECT * FROM c");
        List<NotificationEntity> list = new ();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            list.AddRange(response.ToList());
        }

        return list;
    }
}