using Felix.Common.Interface;
using Felix.Common.Model;
using Felix.Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Felix.Common.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<AppSettings> _options;

        public HttpClientHelper(IOptions<AppSettings> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = new HttpClient { BaseAddress = new Uri(options.Value.BaseUri) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-ww-form-urlencoded"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IHttpClientHelper CreateAccessToken()
        {
            var authInfo = _httpClient?.DefaultRequestHeaders?.Authorization?.Parameter ?? "";
            if (string.IsNullOrEmpty(authInfo))
            {
                var result = PostAsync<dynamic>("identity/Authenticate", new { userName = "", password = "" }).Result;

                if (!result.IsSucessful) return this;

                var accessToken = $"Bearer {result.Result.SecurityToken}";
                _httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);
            }

            return this;
        }

        public IHttpClientHelper SetAccessToken()
        {
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var authInfo = _httpClient?.DefaultRequestHeaders?.Authorization?.Parameter ?? "";
            if (string.IsNullOrEmpty(authInfo))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", accessToken ?? string.Empty);

            }

            return this;
        }

        public async Task<Envelope<T>> GetAsync<T>(string url)
        {
            Envelope<T> result = null;
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<Envelope<T>>(x.Result);
            });

            return result;
        }

        public async Task<Envelope<T>> PostAsync<T>(string url, Dictionary<string, string> values)
        {
            Envelope<T> result = null;
            var content = new FormUrlEncodedContent(values);
            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<Envelope<T>>(x.Result);
            });

            return result;
        }

        public async Task<Envelope<T>> PostAsync<T>(string url, object json)
        {
            Envelope<T> result = null;
            var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<Envelope<T>>(x.Result);
            });

            return result;
        }

        public async Task<Envelope<T>> PutAsync<T>(string url, object json)
        {
            Envelope<T> result = null;
            var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);

            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<Envelope<T>>(x.Result);
            });

            return result;
        }

        public async Task<Envelope<T>> SendAsync<T>(string url)
        {
            Envelope<T> result = null;
            var message = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await _httpClient.SendAsync(message);

            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<Envelope<T>>(x.Result);
            });

            return result;
        }

    }
}
