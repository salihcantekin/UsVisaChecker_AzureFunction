using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsVisaChecker.AF.Infrastructure.Extensions;
using UsVisaChecker.AF.Services;

namespace UsVisaChecker.AF;

public class VisaCheckerFunction
{
    private readonly NotificationService notificationService;
    private readonly VisaService visaService;
    private readonly IConfiguration configuration;
    private readonly ILogger<VisaCheckerFunction> logger;

    public VisaCheckerFunction(NotificationService notificationService, VisaService visaService, IConfiguration configuration, ILogger<VisaCheckerFunction> logger)
    {
        this.notificationService = notificationService;
        this.visaService = visaService;
        this.configuration = configuration;
        this.logger = logger;
    }

    [FunctionName("VisaChecker")]
    public async Task VisaChecker([TimerTrigger("%TimerInterval%", RunOnStartup = true)] TimerInfo myTimer)
    {
        await CheckVisa();
    }


    [FunctionName("InvokeChecker")]
    public async Task<IActionResult> InvokeChecker([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Invoke")] HttpRequest req)
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
    public static IActionResult RunTest([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Test")] HttpRequest req)
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

            if (availableDates is null)
            {
                logger.LogError("No available date found!");
                return;
            }

            var initialDate = configuration.GetValue<DateTime>("InitialDate");

            logger.LogInformation("initialDate: {initialDate}", initialDate.Format());
            logger.LogInformation("{AvailableCount} available dates found", availableDates.Count);

            var earliestDate = availableDates.Where(i => i.Date < initialDate).FirstOrDefault();
            logger.LogInformation("New Earliest: {NewEarliestDate}", earliestDate);

            if (availableDates.Any())
            {
                logger.LogInformation("Earliest: {Earliest}", availableDates?.Min(i => i.Date));

                if (earliestDate is not null)
                {
                    await notificationService.SendNotification(new Models.NotificationRequestModel()
                    {
                        CreateDate = DateTime.Now,
                        EarliestDate = earliestDate.Date,
                        OriginalDate = initialDate,
                        TotalAvailableDate = availableDates?.Count ?? 0,
                    });
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error..{err}", ex.ToString());
            throw;
        }
    }
}


