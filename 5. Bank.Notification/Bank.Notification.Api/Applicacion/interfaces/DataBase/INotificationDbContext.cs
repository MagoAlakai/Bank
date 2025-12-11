namespace Bank.Notification.Api.Applicacion.Interfaces.Database;
public interface INotificationDbContext
{
    Task<bool> AddAsync(NotificationEntity notificationEntity);
    Task<List<NotificationEntity>> GetAllAsync();
}
