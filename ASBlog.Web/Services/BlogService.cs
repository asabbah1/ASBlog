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
    public class BlogService : IBlogService
    {
        public HttpClient _httpClient { get; }
        public AppSettings _appSettings { get; }
        public ILocalStorageService _localStorageService { get; }

        public BlogService(HttpClient httpClient, IOptions<AppSettings> appSettings, ILocalStorageService localStorageService)
        {
            _appSettings = appSettings.Value;
            _localStorageService = localStorageService;

            httpClient.BaseAddress = new Uri(_appSettings.APIUri);

            _httpClient = httpClient;
        }

        public async Task<List<StoryRs>> GetAllStoriesAsync()
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "Blog/Story/GetAllStories");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<List<StoryRs>>(responseBody));
            }
            else
            {
                return new List<StoryRs>();
            }
        }

        public async Task<List<StoryRs>> GetAllStoriesByStatusAsync(int status)
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"Blog/Story/GetAllStoriesByStatus/{status}");

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<List<StoryRs>>(responseBody));
            }
            else
            {
                return new List<StoryRs>();
            }
        }

        public async Task<StoryRs> GetStory(int id)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"Blog/Story/{id}");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<StoryRs>(responseBody));
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteStory(int id)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"Blog/Story/{id}");

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CreateStory(StoryRq storyRq)
        {
            string json = JsonConvert.SerializeObject(storyRq);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Blog/Story/Create");

            requestMessage.Content = new StringContent(json);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();


            if (responseStatusCode == System.Net.HttpStatusCode.Created)
            {
                return await Task.FromResult(true);
            }


            return await Task.FromResult(false);
        }


        public async Task<StoryRs> UpdateStory(int id, StoryRq storyRq)
        {
            string json = JsonConvert.SerializeObject(storyRq);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"Blog/Story/Update/{id}");

            requestMessage.Content = new StringContent(json);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();


            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<StoryRs>(responseBody));
            }


            return null;
        }

        public async Task<bool> ApproveStory(int id)
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"Blog/Story/Approve/{id}");

            requestMessage.Content = new StringContent("");

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();


            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                return await Task.FromResult(true);
            }


            return await Task.FromResult(false);
        }


        public async Task<bool> AddComment(CommentRq commentRq)
        {
            string json = JsonConvert.SerializeObject(commentRq);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Blog/Comment/Create");

            requestMessage.Content = new StringContent(json);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();


            if (responseStatusCode == System.Net.HttpStatusCode.Created)
            {
                return await Task.FromResult(true);
            }


            return await Task.FromResult(false);
        }

    }
}
