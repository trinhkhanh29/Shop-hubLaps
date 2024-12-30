using SendGrid.Helpers.Mail;
using SendGrid;

public class EmailService
{
    private readonly string _apiKey;

    public EmailService(IConfiguration configuration)
    {
        // Fetch API key from configuration
        _apiKey = configuration["EmailSettings:SmtpPassword"];  // The correct key from appsettings.json
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress("test@test.com", "test@test.com");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            // Handle failure if needed
        }
    }
}
