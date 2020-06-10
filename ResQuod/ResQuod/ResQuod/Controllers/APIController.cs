using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ResQuod.Controllers
{
    class APIController
    {
        static HttpClient client = new HttpClient();
        static string token;

        public static string Token { get; private set; }

        public enum Response
        {
            Success, IncorrectCredentials, BadRequest, InternetConnectionProblem, ServerProblem, UnknowError
        }

        public APIController()
        {
            client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public static async Task<Tuple<Response, string>> Login(LoginModel item)
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection");

            var uri = new Uri(string.Format(Constants.API_LoginUrl, string.Empty));

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);


            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.ServerProblem, _error.Message);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.BadRequest, _error.Message);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var _data = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<Token>(_data).Value;
                Debug.WriteLine(token, "Token");
                return Tuple.Create(Response.Success, token);
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.UnknowError, error.Message);
        }

        public static async Task<Tuple<Response, string>> Register(RegisterModel item)
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection");

            var uri = new Uri(string.Format(Constants.API_RegisternUrl, string.Empty));

           // var serializerSettings = new JsonSerializerSettings();
            //serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            Debug.WriteLine(response.StatusCode.ToString() + " " + response.RequestMessage.ToString());


            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                return Tuple.Create(Response.ServerProblem, "Server problem");

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.BadRequest, _error.Message);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Tuple.Create(Response.Success, "Succesfully created");
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.UnknowError, error.Message);
        }

        public static async Task<Tuple<Response, string>> Logout()
        {
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection");

            token = "";
            return Tuple.Create(Response.Success, token);
        }

        public static async Task<Tuple<Response, string, User>> GetUser()
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection", new User());

            var uri = new Uri(string.Format(Constants.API_GetUserUrl, string.Empty));

            var json = JsonConvert.SerializeObject("");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                return Tuple.Create(Response.ServerProblem, "Server problem", new User());

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.BadRequest, _error.Message, new User());
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(_data);
                return Tuple.Create(Response.Success, "Succesfully created", user);
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.UnknowError, error.Message, new User());
        }

        public static async Task<Tuple<Response, string>> UserPatch(UserPatchModel item)
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection");

            var uri = new Uri(string.Format(Constants.API_UserPatchUrl, string.Empty));

            var json = JsonConvert.SerializeObject(item);
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            Debug.WriteLine("PATCHING - " + response.StatusCode.ToString() + " " + response.RequestMessage.ToString());

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                return Tuple.Create(Response.ServerProblem, "Server problem");

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.BadRequest, _error.Message);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Tuple.Create(Response.Success, "Data updated succesfully");
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.UnknowError, error.Message);
        }
    }
}
