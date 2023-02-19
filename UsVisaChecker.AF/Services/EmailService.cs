using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UsVisaChecker.AF.Models;

namespace UsVisaChecker.AF.Services;

public class EmailService
{
    private readonly EmailOptions emailOptions;
    private readonly ILogger<EmailService> logger;
    private readonly SmtpClient smtp;

    public EmailService(ILogger<EmailService> logger,
        IOptions<EmailOptions> options,
        SmtpClient smtp)
    {
        this.logger = logger;
        this.smtp = smtp;
        emailOptions = options.Value;
    }


    public async Task<bool> SendEmail(EmailSendRequestModel request)
    {
        try
        {
            MailAddress toAddress = new(request.Recipient, request.ToDisplayName);
            MailAddress fromAddress = new(emailOptions.FromAddress, emailOptions.DefaultFromDisplayName);

            var subject = request.Subject;
            var body = request.BodyContent;

            using var message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.Body = body;

            await smtp.SendMailAsync(message);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while sending the email. Error: " + ex.Message);
            return false;
        }
    }
}