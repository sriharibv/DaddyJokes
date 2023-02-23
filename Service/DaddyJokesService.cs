using System.Net.Http.Headers;
using DaddyJokes.Constants;
using DaddyJokes.Data;

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

        public async Task<RandomDaddyJokesOut> GetRandomDaddyJokes()
        {
            try
            {

                var response = await _httpClient.GetAsync(string.Empty);
                EnsureSucessStatusCode(response);
                var data = await response?.Content?.ReadFromJsonAsync<RandomDaddyJokesOut>();
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

            //TODO:
            //throw new System.Web.HttpException((int)response.StatusCode, uAndBCustomError ?? responseBody);
        }
    }
}
