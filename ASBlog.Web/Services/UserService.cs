using ASBlog.Web.Resources;
using ASBlog.Web.Settings;
using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASBlog.Web.Services
{
    public class UserService : IUserService
    {

        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }
        public UserService(HttpClient httpClient, IOptions<AppSettings> appSettings, ILocalStorageService localStorageService)
        {
            _appSettings = appSettings.Value;
            _localStorageService = localStorageService;

            httpClient.BaseAddress = new Uri(_appSettings.APIUri);

            _httpClient = httpClient;          
        }

        public async Task<LoginRs> LoginAsync(LoginRq loginRq)
        {
            LoginRs result = new LoginRs();
            string json = JsonConvert.SerializeObject(loginRq);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Auth/Login");

            requestMessage.Content = new StringContent(json);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();


            if(responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                result.status = 1;
                result.accessToken = responseBody;
            }
            else
            {
                result.status = 0;
                result.errorMessage = responseBody;
            }
            return result;
        }

        public async Task<GeneralRs> RegisterAsync(RegisterRq registerRq)
        {
            GeneralRs result = new GeneralRs();
            string json = JsonConvert.SerializeObject(registerRq);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Auth/Register");

            requestMessage.Content = new StringContent(json);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            

            if (responseStatusCode == System.Net.HttpStatusCode.Created)
            {
                result.status = 1;
                
            }
            else
            {
                var responseObj = JsonConvert.DeserializeObject<ErrorRs>(responseBody);
                result.status = 0;
                result.errorMessage = responseObj.detail;
            }
            return result;
        }


        public async Task<UserRs> GetUserAsync()
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "Auth/User");


            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if(responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<UserRs>(responseBody));
            }
            else
            {
                return null;
            }
        }

        public async Task<List<UserRs>> GetAllUsersAsync()
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "Auth/AllUsers");


            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<List<UserRs>>(responseBody));
            }
            else
            {
                return new List<UserRs>();
            }
        }




    }
}
