using System.Linq;
using System.Xml.Linq;

namespace PMA.Forms.Helpers
{
    public static class Login
    {
        public static string GetToken(this string response)
        {
            return XDocument.Parse(response).Descendants("token").FirstOrDefault()?.Value;
        }
    }
}