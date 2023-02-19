using System;

namespace UsVisaChecker.AF.Infrastructure.Extensions;

internal static class GeneralExtensions
{
    internal static string Format(this string value, params object[] values)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return string.Format(value, values);
    }

    internal static string Format(this DateTime value)
    {
        return value.ToString(Constants.DATE_FORMAT);
    }
}