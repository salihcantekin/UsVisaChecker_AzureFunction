using System.Collections.Generic;

namespace UsVisaChecker.AF.Infrastructure.Providers;
internal static class HeaderProvider
{
    public static Dictionary<string, string> GetCommonWithValues(string cookie = null, string referer = null)
    {
        var headers = GetCommonHeaders();

        if (cookie is not null)
            UpSert(headers, "Cookie", cookie);

        if (referer is not null)
            UpSert(headers, "Referer", referer);

        return headers;
    }

    public static Dictionary<string, string> GetCommonHeaders()
    {
        var headers = new Dictionary<string, string>()
        {
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7" },
            { "Accept-Encoding", "gzip, deflate, br" },
            { "Connection", "keep-alive" },
            { "Host", "ais.usvisa-info.com" },
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36" },
            { "X-CSRF-Token","fhgPM8Q179Z9CslJK9Y1QOD5DT4VLFe06VKNeSJW/Pmiw47L8GWRG5Df5VJRRuRKAbazjQdwOxabvbyfUwXV3Q==" },
            { "X-Requested-With", "XMLHttpRequest" },
            { "Cookie", "_ga=GA1.2.1855477384.1676769081; _gid=GA1.2.781162165.1676769081; _gat=1; _yatri_session=Y25nMm5Lc3NKQkNaRkl2RVdyanpnL2dFOTBqdjQvdUFjaFhRRVpDWTZBVTd3dFhmeHB3SGp0dGlYTFVmZ0xDRnArSktHNk8wKzF3TEowNjlqdlR3cDd5cStjc3JYdnZHUDYrVDRGT2l5VHpBaFE4Q0NjN0ptSmZqWS9BVTBCcUxMRExzMVJ6Zk9MV2xrUFM3djBQeWpPTDJKeC9hTUJCdVo4bWJNQWx3dkNuM2h5R1VtTlhsSytYRVd5ZGRlamNCLS16aVJiT3k0ZGduN3ZUajJCK2lCdnFnPT0%3D--f6f960ae57d8da01bae021cf6ba2f01903ac0d08" }
        };

        return headers;
    }

    private static void UpSert(Dictionary<string, string> headers, string key, string value)
    {
        if (headers.ContainsKey(key))
            headers[key] = value;
        else
            headers.Add(key, value);
    }
}
