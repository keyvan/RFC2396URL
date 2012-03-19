
namespace Rfc2396Url
{
    /// <summary>
    /// This class represents an RFC2396 URL.
    /// </summary>
    public class RFC2396Url
    {
        private string stringUrl = string.Empty;

        /// <summary>
        /// Public constructor of the class.
        /// </summary>
        /// <param name="url">URL to use with class.</param>
        public RFC2396Url(string url)
        {
            this.stringUrl = url;
        }

        /// <summary>
        /// Canonicalizes the URL.
        /// </summary>
        /// <returns>Canonicalized URL.</returns>
        public string Canonicalize()
        {
            Canonicalization canonicalization = new Canonicalization();
            return canonicalization.GetCanonicalizedUrl(this.stringUrl);
        }
    }
}
