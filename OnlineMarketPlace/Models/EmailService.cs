using MailKit.Net.Smtp;
using MimeKit;
namespace OnlineMarketPlace.Models;
public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string recipient, string subject, string messageBody)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Your Name", emailSettings["SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(recipient));
        email.Subject = subject;

        email.Body = new TextPart("plain")
        {
            Text = messageBody
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
