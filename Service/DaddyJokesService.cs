using System.Net.Http.Headers;
using DaddyJokes.Constants;
using DaddyJokes.Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using static DaddyJokes.Constants.DaddyJokeConstants;

namespace DaddyJokes.Service
{
    public class DaddyJokesService
    {
        private readonly HttpClient _httpClient;

        public DaddyJokesService(string apiUrl, string apiUserName = null, string apiPassword = null)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(DaddyJokeConstants.UriConstants.ResponseTypeJson));
        }

        public async Task<DaddyJokesOut> GetRandomDaddyJokes()
        {
            try
            {

                var response = await _httpClient.GetAsync(string.Empty);
                EnsureSucessStatusCode(response);
                return await response?.Content?.ReadFromJsonAsync<DaddyJokesOut>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching data from the server. Please check the admin for more info. Error details: {ex.Message}");
            }
        }

        public async Task<Datatable<DaddyJokesOut>> GetDaddyJokes(int pageNumber = 1, int limit = 30, string searchTerm = null)
        {
            try
            {
                var kvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("page", pageNumber.ToString()),
                    new KeyValuePair<string, string>("limit", limit.ToString()),
                };
                if (!string.IsNullOrEmpty(searchTerm))
                    kvp.Add(new KeyValuePair<string, string>("term", searchTerm));

                var query = new QueryBuilder(kvp).ToQueryString();
                var requestUri = UriConstants.Search + query;
                var response = await _httpClient.GetAsync(requestUri);
                EnsureSucessStatusCode(response);
                var data = await response?.Content?.ReadFromJsonAsync<Datatable<DaddyJokesOut>>();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching data from the server. Please check the admin for more info. Error details: {ex.Message}");
            }
        }

        private static void EnsureSucessStatusCode(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            throw new BadHttpRequestException("Error occured while making the api request", (int)response.StatusCode);
        }
    }
}
