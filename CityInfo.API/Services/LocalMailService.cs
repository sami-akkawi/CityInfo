namespace CityInfo.API.Services;

public class LocalMailService(string mailTo = "admin@example.com", string mailFrom = "noreply@example.com")
{
    public void Send(string subject, string message)
    {
        Console.WriteLine($"Mail from {mailFrom} to {mailTo}, with {nameof(LocalMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}