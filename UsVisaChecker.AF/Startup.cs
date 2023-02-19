using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading;
using UsVisaChecker.AF;
using UsVisaChecker.AF.Models;
using UsVisaChecker.AF.Services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace UsVisaChecker.AF;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var context = builder.GetContext();

        builder.Services.AddApplicationInsightsTelemetry(context.Configuration);

        builder.Services.AddOptions<EmailOptions>()
                        .Bind(context.Configuration.GetSection("EmailOptions"))
                        .ValidateDataAnnotations();

        builder.Services.AddOptions<VisaOptions>()
                        .Bind(context.Configuration.GetSection("Visa"))
                        .ValidateDataAnnotations();

        builder.Services.AddSingleton<VisaService>();
        builder.Services.AddScoped<EmailService>();

        builder.Services.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptions<EmailOptions>>();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var fromAddress = new MailAddress(options.Value.FromAddress, options.Value.DefaultFromDisplayName);

            string fromPassword = configuration["EmailProviderPassword"];

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            return smtp;
        });
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        builder.ConfigurationBuilder
            .SetBasePath(Environment.CurrentDirectory)
#if DEBUG
            .AddJsonFile("local.settings.json", true, true)
#endif
            .AddEnvironmentVariables();
    }
}
