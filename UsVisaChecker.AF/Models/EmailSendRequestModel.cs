namespace UsVisaChecker.AF.Models;

public class EmailSendRequestModel
{
    public string Recipient { get; set; }

    public string ToDisplayName { get; set; }

    public string Subject { get; set; }

    public string BodyContent { get; set; }
}