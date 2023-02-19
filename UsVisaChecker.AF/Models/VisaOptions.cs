using System.ComponentModel.DataAnnotations;

namespace UsVisaChecker.AF.Models;

public class VisaOptions
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Pass { get; set; }
}