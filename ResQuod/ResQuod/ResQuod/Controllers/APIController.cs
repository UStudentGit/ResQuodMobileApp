﻿using Newtonsoft.Json;
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
    static class APIController
    {
        static HttpClient client = new HttpClient();
        static string token;

        public static string Token { get; private set; }

        public enum Response
        {
            Success, IncorrectCredentials, BadRequest, InternetConnectionProblem, ServerProblem, Forbidden, UnknowError
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

        #region UserPanel
        public static async Task<Tuple<Response, string>> Logout()
        {
            // TODO
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

            var uri = new Uri(Constants.API_UserPatchUrl);

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
        #endregion

        #region NFC
        public static async Task<Tuple<Response, string, RoomPosition[]>> GetPositionsWithoutTag()
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection", new RoomPosition[0]);

            var uri = new Uri(string.Format(Constants.API_GetNullTagPositions, string.Empty));

            var json = JsonConvert.SerializeObject("");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                return Tuple.Create(Response.ServerProblem, "Server problem", new RoomPosition[0]);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.BadRequest, _error.Message, new RoomPosition[0]);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var positions = JsonConvert.DeserializeObject<RoomPosition[]>(_data);
                return Tuple.Create(Response.Success, "Succesfully downloaded", positions);
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.UnknowError, error.Message, new RoomPosition[0]);
        }

        public static async Task<Tuple<Response, string, PresenceResponse>> ReportPresence(string tag)
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection", new PresenceResponse());

            var uri = new Uri(string.Format(Constants.API_ReportPresence + "?tagId=" + tag, string.Empty));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var req = new { tagId = tag};
            //var json = JsonConvert.SerializeObject(req);
            var content = new StringContent("", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                return Tuple.Create(Response.ServerProblem, "Server problem", new PresenceResponse());

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.BadRequest, _error.Message, new PresenceResponse());
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var presenceResponse = JsonConvert.DeserializeObject<PresenceResponse>(_data);
                return Tuple.Create(Response.Success, "Succesfully reported presence", presenceResponse);
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.BadRequest, error.Message, new PresenceResponse());
        }

        public static async Task<Tuple<Response, string>> AssignTagToPosition(string tag, int positionId)
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection");

            var uri = new Uri(string.Format(Constants.API_AssignTagToPosition));

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var req = new { tagId = tag, id = positionId};
            var json = JsonConvert.SerializeObject(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, uri)
            {
                Content = content
            };            

            HttpResponseMessage response = await client.SendAsync(request);

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
                var _data = await response.Content.ReadAsStringAsync();
                return Tuple.Create(Response.Success, "Succesfully assigned tag " + tag);
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.BadRequest, error.Message);
        }
        #endregion

        #region Events
        public static async Task<Tuple<Response, string, List<Event>>> GetUserEvents()
        {
            var events = new List<Event>();
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection", events);

            var uri = new Uri(string.Format(Constants.API_GetUserEvents, string.Empty));

            //var json = JsonConvert.SerializeObject("");
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                return Tuple.Create(Response.ServerProblem, "Server problem", events);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.BadRequest, _error.Message, events);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                var _data = await response.Content.ReadAsStringAsync();
                var _error = JsonConvert.DeserializeObject<ErrorResponse>(_data);
                return Tuple.Create(Response.Forbidden, _error.Message, events);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var _data = await response.Content.ReadAsStringAsync();
                events = JsonConvert.DeserializeObject<List<Event>>(_data);
                return Tuple.Create(Response.Success, "Succesfully loaded events", events);
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.UnknowError, error.Message, events);
        }

        public static async Task<Tuple<Response, string>> JoinEvent(string password)
        {
            //First check internet connection
            if (!InternetController.IsInternetActive())
                return Tuple.Create(Response.InternetConnectionProblem, "You have no internet connection");

            var uri = new Uri(Constants.API_JoinEvent + "/" + password);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent("", Encoding.UTF8, "application/json");

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
                return Tuple.Create(Response.Success, "Successfully joined an event!");
            }

            var data = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ErrorResponse>(data);
            return Tuple.Create(Response.UnknowError, error.Message);
        }
        #endregion
    }
}
