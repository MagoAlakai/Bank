using Bank.Notification.Api.Applicacion.Models;

namespace Bank.Notification.Api.Applicacion.Features.Process;

public class ProcessService(INotificationDbContext notificationDbContext, ISendGridEmailService sendGridEmailService, IConfiguration configuration) : IProcessService
{
    public async Task Execute(string message, string subscription)
    {
        NotificationEntity notificationEntity = JsonConvert.DeserializeObject<NotificationEntity>(message) ?? new NotificationEntity();

        string emailPayload = string.Empty;
        string fromEmail = configuration["SENDGRIDFROMEMAIL"] ?? string.Empty;
        string toEmail = configuration["SENDGRIDFROMEMAIL"] ?? string.Empty;

        if (subscription.Equals(ReceivedSubscriptionsConstants.TRANSACTION_COMPLETED)) 
        {
            notificationEntity.TransactionState = true;
            notificationEntity.Content = "The transaction was completed successfully.";

            string status = notificationEntity.TransactionState ? "successful" : "failed";
            emailPayload = CreateSendGridModel.Create(toEmail, fromEmail, status, notificationEntity.Content);
            Console.WriteLine(emailPayload);
            Console.WriteLine(emailPayload);
        }
        else
        {
            notificationEntity.TransactionState = false;
            notificationEntity.Content = "The transaction has failed.";

            string status = notificationEntity.TransactionState ? "successful" : "failed";
            emailPayload = CreateSendGridModel.Create(toEmail, fromEmail, status, notificationEntity.Content);
        }

        await sendGridEmailService.Execute(emailPayload);
        await ProcessDatabase(notificationEntity!);
    }

    public async Task ProcessDatabase(NotificationEntity notificationEntity) 
    { 
        await notificationDbContext.AddAsync(notificationEntity);
    }
}
