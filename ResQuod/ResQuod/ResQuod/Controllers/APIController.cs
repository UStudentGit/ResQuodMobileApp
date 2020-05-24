using Newtonsoft.Json;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ResQuod.Controllers
{
    class APIController
    {
        static HttpClient client = new HttpClient();
        static string token;

        public static string Token { get; private set; }

        public APIController()
        {
            client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public static async Task<bool> Login(LoginModel item)
        {
            var uri = new Uri(string.Format(Constants.API_LoginUrl, string.Empty));
           
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            if (client == null)
                return false;
            response = await client.PostAsync(uri, content);

            if (response == null)
                return false;

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<string>(data);
                Debug.WriteLine("Token", token);
                return true;
            }

            return false;
            
}
    }
}
