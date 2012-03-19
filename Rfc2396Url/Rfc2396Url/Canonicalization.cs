// Algorithm: http://wiki.mozilla.org/Phishing_Protection:_Server_Spec#Canonical_Hostname_Creation
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rfc2396Url
{
    /// <summary>
    /// This class is responsible to provide some methods to convert a 
    /// normal URL to RFC2396 valid URL.
    /// </summary>
    internal class Canonicalization
    {
        /// <summary>
        /// Public constrcutor
        /// </summary>
        public Canonicalization()
        {
        }

        /// <summary>
        /// Gets a url in general form and returns a canonicalized URL.
        /// </summary>
        /// <param name="url">A URL in general form.</param>
        /// <returns>RFC2396 valid URL.</returns>
        public string GetCanonicalizedUrl(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url can't be null");

            string hostName = GetHostName(url);
            string remainder = GetRemainder(url);

            hostName = RemoveSpecialCharacters(hostName);
            hostName = ReplaceConsecutiveDots(hostName);
            hostName = NormalizeIPAddress(hostName);
            hostName = EscapeSpecialChars(hostName);
            hostName = hostName.ToLower();

            remainder = ResolveSequences(remainder);
            remainder = RemoveFragmentIdentifier(remainder);

            if (string.IsNullOrEmpty(remainder))
                hostName += "/";

            return (hostName + remainder);
        }

        /// <summary>
        /// Gets the hostname of the URL.
        /// </summary>
        /// <param name="url">A URL in general form.</param>
        /// <returns>Hostname.</returns>
        private string GetHostName(string url)
        {
            string result = url;
            Regex regEx = new Regex
                (@"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[a-zA-Z0-9\-]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?");

            Match matches = regEx.Match(url);

            return matches.Groups[2].Value;
        }

        /// <summary>
        /// Gets the remainder of the URL.
        /// </summary>
        /// <param name="url">A URL in general form.</param>
        /// <returns>Remainder.</returns>
        private string GetRemainder(string url)
        {
            string result = url;
            Regex regEx = new Regex
                (@"^(?=[^&])(?:(?<scheme>[^:/?#]+):)?(?://(?<authority>[^/?#]*))?(?<path>[a-zA-Z0-9\-]*)(?:\?(?<query>[^#]*))?(?:#(?<fragment>.*))?");

            Match matches = regEx.Match(url);

            return matches.Groups[3].Value;
        }

        /// <summary>
        /// Removes special characters from the hostname.
        /// </summary>
        /// <param name="hostname">The hostname of the URL.</param>
        /// <returns>Escaped hostname.</returns>
        private string RemoveSpecialCharacters(string hostname)
        {
            string result = hostname;

            Regex regEx1 = new Regex("[\x00-\x1f\x7f-\xff]+");
            if (regEx1.IsMatch(result))
                result = regEx1.Replace(result, "");

            Regex regEx2 = new Regex("^\\.+|\\.+$");
            if (regEx2.IsMatch(result))
                result = regEx2.Replace(result, "");

            return result;
        }

        /// <summary>
        /// Replaces consecutive dots with a single dot.
        /// </summary>
        /// <param name="hostname">The hostname of the URL.</param>
        /// <returns>Escaped hostname.</returns>
        private string ReplaceConsecutiveDots(string hostname)
        {
            string result = hostname;

            Regex regEx = new Regex(@"[\.]+");
            if (regEx.IsMatch(result))
                result = regEx.Replace(result, ".");

            return result;
        }

        /// <summary>
        /// Normalizes the hostname if it is an IP address.
        /// </summary>
        /// <param name="hostname">The hostname of the URL.</param>
        /// <returns>Normalized hostname.</returns>
        private string NormalizeIPAddress(string hostname)
        {
            return hostname;
        }

        /// <summary>
        /// Escapes special characters from URL.
        /// </summary>
        /// <param name="hostname">Hostname of the URL.</param>
        /// <returns>Hostname with escaped URL.</returns>
        private string EscapeSpecialChars(string hostname)
        {
            string result = hostname;

            Regex regEx = new Regex(@"[^0-9A-Za-z\.\-]+");

            if (regEx.IsMatch(result))
                result = regEx.Replace(result, "");

            return result;
        }

        /// <summary>
        /// Resolves sequences of dots.
        /// </summary>
        /// <param name="remainder">Remained of the URL.</param>
        /// <returns>Remainder without sequences of dots.</returns>
        private string ResolveSequences(string remainder)
        {
            string result = remainder;

            Regex regEx = new Regex(@"(/\./)+");

            if (regEx.IsMatch(result))
                result = regEx.Replace(result, "/");

            regEx = new Regex(@"/[0-9A-Za-z(\-)(\.)]+");

            if (regEx.IsMatch(result))
            {
                List<string> validComponents = new List<string>();

                MatchCollection matches = regEx.Matches(result);

                foreach (Match match in matches)
                {
                    Match nextMatch = match.NextMatch();

                    if ((match.Value != "/..") && (nextMatch.Value != "/.."))
                        validComponents.Add(match.Value);
                }

                result = string.Join("", validComponents.ToArray());
            }

            return result;
        }

        /// <summary>
        /// Removes fragment identifier (#) and everything after it.
        /// </summary>
        /// <param name="remainder">The remainder of the URL.</param>
        /// <returns>The remainder of the URL after removing fragment identifier.</returns>
        private string RemoveFragmentIdentifier(string remainder)
        {
            int index = remainder.IndexOf("#");

            if (index > -1)
                remainder = remainder.Remove(index);

            return remainder;
        }
    }
}
