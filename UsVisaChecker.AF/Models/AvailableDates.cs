using System;
using System.Text.Json.Serialization;
using UsVisaChecker.AF.Infrastructure.Extensions;

namespace UsVisaChecker.AF.Models;

public class AvailableDates
{
    [JsonPropertyName("date")] public DateTime Date { get; set; }

    public override string ToString()
    {
        return Date.Format();
    }
}