namespace CityInfo.API.Services;

public class CloudMailService(string mailTo = "admin@example.com", string mailFrom = "noreply@example.com") : IMailService
{
    public void Send(string subject, string message)
    {
        Console.WriteLine($"Mail from {mailFrom} to {mailTo}, with {nameof(CloudMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}