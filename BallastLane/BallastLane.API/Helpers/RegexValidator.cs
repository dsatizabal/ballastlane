using System.Text.RegularExpressions;

namespace BallastLane.API.Helpers
{
    public static class RegexValidator
    {
        public const string FiscalNumberRegEx = @"^[VJF]-\d{8}$";
        public const string SecurePasswordRegEx = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#\$=\*]).{8,}$";

        public static bool IsValid(string input, string regex)
        {
            Regex pattern = new Regex(regex);
            return pattern.IsMatch(input);
        }
    }
}
