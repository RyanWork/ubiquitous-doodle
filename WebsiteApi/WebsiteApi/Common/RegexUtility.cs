using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WebsiteApi.Common
{
    public static class RegexExtensions
    {
        private static readonly Regex CachedEmailMatchRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
        
        private static readonly Regex CachedNormalizeRegex = new(@"(@)(.+)$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(200)); 
        
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = CachedNormalizeRegex.Replace(email, DomainMapper);

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return CachedEmailMatchRegex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}