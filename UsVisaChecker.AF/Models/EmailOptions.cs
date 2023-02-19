using System.ComponentModel.DataAnnotations;

namespace UsVisaChecker.AF.Models;

public class EmailOptions
{
    [Required, EmailAddress]
    public string FromAddress { get; set; }

    [Required]
    public string DefaultFromDisplayName { get; set; }
}
