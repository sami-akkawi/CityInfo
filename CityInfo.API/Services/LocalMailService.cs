namespace CityInfo.API.Services;

public class LocalMailService(IConfiguration configuration) : IMailService
{
    private readonly string mailFrom = configuration["mailSettings:mailFromAddress"] ?? string.Empty;
    private readonly string mailTo = configuration["mailSettings:mailToAddress"] ?? string.Empty;
    
    public void Send(string subject, string message)
    {
        Console.WriteLine($"Mail from {mailFrom} to {mailTo}, with {nameof(LocalMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}