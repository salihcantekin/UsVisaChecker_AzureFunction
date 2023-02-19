using System.Collections.Generic;

namespace UsVisaChecker.AF.Infrastructure.Extensions;
internal static class HttpClientExtensions
{
    internal static void SetHeaders(this System.Net.Http.HttpClient client, Dictionary<string, string> headers)
    {
        foreach (var header in headers)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
    }
}
