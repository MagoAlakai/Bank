namespace Bank.Notification.Api.Applicacion.External.SendGridEmail;

public interface ISendGridEmailService
{
    Task<bool> Execute(string emailPayload);
}
