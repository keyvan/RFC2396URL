using System;
using Rfc2396Url;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RFC2396 URL .NET";

            string url = "http://One.2.nayyeri$.net/1/gholi/./3/test/keyvan/../nayyeri.html#something";
            RFC2396Url RfcUrl = new RFC2396Url(url);

            Console.WriteLine(string.Format("Original URL: {0}", url));
            Console.WriteLine(string.Format("Canonicalized URL: {0}", RfcUrl.Canonicalize()));

            Console.ReadLine();
        }
    }
}
