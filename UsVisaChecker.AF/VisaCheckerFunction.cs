using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UsVisaChecker.AF.Infrastructure.Extensions;
using UsVisaChecker.AF.Models;
using UsVisaChecker.AF.Services;

namespace UsVisaChecker.AF;

public class VisaCheckerFunction
{
    private readonly IConfiguration configuration;
    private readonly EmailService emailService;
    private readonly ILogger<VisaCheckerFunction> logger;
    private readonly VisaService visaService;

    public VisaCheckerFunction(EmailService emailService, VisaService visaService, IConfiguration configuration,
        ILogger<VisaCheckerFunction> logger)
    {
        this.emailService = emailService;
        this.visaService = visaService;
        this.configuration = configuration;
        this.logger = logger;
    }

    [FunctionName("VisaChecker")]
    public async Task VisaChecker([TimerTrigger("0 0 */1 * * *", RunOnStartup = true)] TimerInfo myTimer)
    {
        await CheckVisa();
    }


    [FunctionName("InvokeChecker")]
    public async Task<IActionResult> InvokeChecker(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Invoke")] HttpRequest req)
    {
        try
        {
            await CheckVisa();
            return new OkResult();
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.ToString());
        }
    }

    [FunctionName("Test")]
    public static IActionResult RunTest(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Test")] HttpRequest req)
    {
        return new OkObjectResult("OK");
    }


    private async Task CheckVisa()
    {
        try
        {
            logger.LogInformation("VisaCheckerFunction worked at {date}", DateTime.Now.Format());

            var loginCookie = await visaService.LoginRequest();
            var availableDates = await visaService.GetAvailableDates(loginCookie);
            var initialDate = configuration.GetValue<DateTime>("InitialDate");

            logger.LogInformation("{AvailableCount} available dates found", availableDates.Count);
            logger.LogInformation("initialDate: {initialDate}", initialDate.Format());

            var earliestDate = availableDates.Where(i => i.Date < initialDate).FirstOrDefault();

            logger.LogInformation("Earliest: {Earliest}", availableDates.Min(i => i.Date).Format());
            logger.LogInformation("New Earliest: {NewEarliestDate}", earliestDate?.ToString());

            if (earliestDate is not null)
            {
                var emailRes = await emailService.SendEmail(new EmailSendRequestModel
                {
                    Recipient = configuration["EmailSend:Recipient"],
                    BodyContent = $"There is an available date for visa appointment for {earliestDate}",
                    ToDisplayName = "Salih Cantekin",
                    Subject = "US Visa Appointment Available"
                });

                logger.LogInformation("Email sent result: {EmailSuccess}!", emailRes);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error..{err}", ex.ToString());
            throw;
        }
    }
}