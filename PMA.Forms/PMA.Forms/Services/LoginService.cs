using System.Net.Http;
using System.Text;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using PMA.Forms.Resources;

namespace PMA.Forms.Services
{
    public class LoginService
    {
        private HttpClient HttpClient { get; }

        public LoginService()
        {
            HttpClient = new HttpClient(new NativeMessageHandler());
        }

        public string Login(string username, string password)
        {
            var loginData = new
            {
                username,
                password
            };

            var jsonLogin = JObject.FromObject(loginData).ToString();

            var response = HttpClient.PostAsync(Url.Login, new StringContent(jsonLogin, Encoding.UTF8, "application/json")).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}