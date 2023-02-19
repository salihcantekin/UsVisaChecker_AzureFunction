namespace UsVisaChecker.AF.Infrastructure;

internal class Constants
{
    private const string COUNTRY_CODE = "en-gb";
    private const string CONSULATE_CODE = "17"; // London

    internal const string DATE_FORMAT = "dd.MM.yyyy";

    internal const string LOGIN_PAYLOAD_F =
        "utf8=%E2%9C%93&user%5Bemail%5D={0}&user%5Bpassword%5D={1}&policy_confirmed=1&commit=Sign+In";

    internal class Endpoints
    {
        internal const string SIGN_IN = $"https://ais.usvisa-info.com/{COUNTRY_CODE}/niv/users/sign_in";

        internal const string APPOINTMENT_F =
            $"https://ais.usvisa-info.com/{COUNTRY_CODE}/niv/schedule/{{0}}/appointment/days/{CONSULATE_CODE}.json?appointments[expedite]=false";

        internal const string GETDATES_REFERER_F =
            $"https://ais.usvisa-info.com/{COUNTRY_CODE}/niv/schedule/{{0}}/appointment";
    }
}