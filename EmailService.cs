using Microsoft.Extensions.Configuration;

interface IEmailService
{
    void Email(string to, string from, string subject, string body);
}

public class EmailService : IEmailService
{
    IConfiguration configuration;
    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration; // to pick up SMTP details for a real email server
    }

    public void Email(string to, string from, string subject, string body)
    {
        Console.WriteLine($"-== Begin pretend email ==-");
        Console.WriteLine($"Pretending to send email {to} from {from}:\nSubject: {subject}\n{body}");
        Console.WriteLine($"-== End pretend email ==-");
    }
}