namespace Bank.Notification.Api.Applicacion.Models;

public static class CreateSendGridModel
{
    public static string Create(string toEmail, string fromEmail, string status, string textPart)
    {

        string htmlTemplate = File.ReadAllText("Template/template-email.html");
        string htmlContent = htmlTemplate
            .Replace("{{STATUS}}", status)
            .Replace("{{MESSAGE}}", textPart)
            .Replace("{{DATE}}", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"));

        var emailPayload = new
        {
            personalizations = new[]
            {
                new
                {
                    to = new[]
                    {
                        new { email = toEmail }
                    },
                    subject = "Notification from Bank"
                }
            },
            from = new
            {
                email = fromEmail
            },
            content = new[]
            {
                new
                {
                    type = "text/html",
                    value = htmlContent
                }
            }
        };

        return JsonConvert.SerializeObject(emailPayload);
    }
}
