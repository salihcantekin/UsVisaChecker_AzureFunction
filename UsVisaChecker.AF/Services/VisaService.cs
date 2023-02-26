﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UsVisaChecker.AF.Infrastructure;
using UsVisaChecker.AF.Infrastructure.Extensions;
using UsVisaChecker.AF.Infrastructure.Providers;
using UsVisaChecker.AF.Models;

namespace UsVisaChecker.AF.Services;
public class VisaService
{
    private readonly IConfiguration configuration;
    private readonly IOptions<VisaOptions> options;
    private readonly ILogger<VisaService> logger;

    public VisaService(IConfiguration configuration, IOptions<VisaOptions> options, ILogger<VisaService> logger)
    {
        this.configuration = configuration;
        this.options = options;
        this.logger = logger;
    }

    public async Task<string> LoginRequest()
    {
        var email = HttpUtility.UrlEncode(options.Value.Email);
        var pass = HttpUtility.UrlEncode(options.Value.Pass);

        var payload = Constants.LOGIN_PAYLOAD_F.Format(HttpUtility.UrlEncode(options.Value.Email),
                                                       HttpUtility.UrlEncode(options.Value.Pass));

        var stringContent = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");
        var headers = HeaderProvider.GetCommonWithValues(referer: Constants.Endpoints.SIGN_IN);

        using var client = new HttpClient();
        client.SetHeaders(headers);

        logger.LogDebug("Login Url: {Url}", Constants.Endpoints.SIGN_IN);
        logger.LogDebug("Payload: {Payload}", payload);

        var httpRes = await client.PostAsync(Constants.Endpoints.SIGN_IN, stringContent);

        if (!httpRes.IsSuccessStatusCode)
        {
            var ex = await httpRes.Content.ReadAsStringAsync();
            throw new System.Exception(ex);
        }

        logger.LogDebug("Login Response Headers: {Headers}",
            string.Join(',', httpRes.Headers.Select(i => $"{i.Key}:{i.Value}")));

        var responseBody = await httpRes.Content.ReadAsStringAsync();

        logger.LogDebug("Login ResponseBody: {ResponseBody}", responseBody);

        var cookie = httpRes
                        .Headers
                        .FirstOrDefault(i => i.Key == "Set-Cookie")
                        .Value.FirstOrDefault();

        logger.LogDebug("loginCookie {cookie}", cookie);
        
        return cookie;
    }

    public async Task<List<AvailableDates>> GetAvailableDates(string cookie)
    {
        var appUrl = Constants.Endpoints.APPOINTMENT_F.Format(configuration["ApplicationId"]);
        var refererUrl = Constants.Endpoints.GETDATES_REFERER_F.Format(configuration["ApplicationId"]);
        var headers = HeaderProvider.GetCommonWithValues(cookie: cookie,
                                                         referer: refererUrl);

        using var client = new HttpClient();
        client.SetHeaders(headers);

        logger.LogDebug("GetAvailableDates Url: {Url}", appUrl);

        var httpRes = await client.GetAsync(appUrl);

        logger.LogDebug("Dates Response Headers: {Headers}",
            string.Join(',', httpRes.Headers.Select(i => $"{i.Key}:{i.Value}")));

        var responseBody = await httpRes.Content.ReadAsStringAsync();

        logger.LogDebug("GetAvailableDates ResponseBody: {ResponseBody}", responseBody);

        var result = await client.GetFromJsonAsync<List<AvailableDates>>(appUrl);

        if (result != null && result.Any())
            logger.LogInformation("Available dates: " + string.Join(',', result.OrderBy(i => i.Date).Select(i => i.Date.ToString("yyyy-MM-dd"))));

        return result ?? new();
    }
}
