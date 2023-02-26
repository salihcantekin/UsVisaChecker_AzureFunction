using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UsVisaChecker.AF.Models;

namespace UsVisaChecker.AF.Services;
public class NotificationService
{
    private readonly EmailService emailService;
    private readonly ILogger<NotificationService> logger;
    private readonly IConfiguration configuration;

    public NotificationService(EmailService emailService, ILogger<NotificationService> logger, IConfiguration configuration)
    {
        this.emailService = emailService;
        this.logger = logger;
        this.configuration = configuration;
    }

    public async Task SendNotification(NotificationRequestModel request)
    {
        #region Email Sending

        if (request.EarliestDate < request.OriginalDate)
        {
            var emailRequest = GetEmailRequestModel(request);
            var emailRes = await emailService.SendEmail(emailRequest);
            logger.LogInformation("Email sent result: {EmailSuccess}!", emailRes);
        }

        #endregion
    }

    private EmailSendRequestModel GetEmailRequestModel(NotificationRequestModel request)
    {
        return new EmailSendRequestModel()
        {
            Recipient = configuration["EmailSend:Recipient"],
            BodyContent = $"There is an available date for visa appointment for {request.EarliestDate}",
            ToDisplayName = configuration["EmailSend:ToDisplayName"] ?? "Salih Cantekin",
            Subject = "US Visa Appointment Available"
        };
    }
}
