namespace Bank.Notification.Api.External.SendGridEmail;

public class SendGridEmailService(IConfiguration configuration) : ISendGridEmailService
{
    public async Task<bool> Execute(string emailPayload)
    {
        string SendGridApiKey = configuration["SENDGRIDAPIKEY"];
        string SendGridApiUrl = configuration["SENDGRIDAPIURL"];

        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SendGridApiKey);
        StringContent content = new(emailPayload, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage response = await httpClient.PostAsync(SendGridApiUrl, content);

        return response.IsSuccessStatusCode;
    }
}
