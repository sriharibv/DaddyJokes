using System.Net.Http.Headers;
using DaddyJokes.Constants;
using DaddyJokes.Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using static DaddyJokes.Constants.DaddyJokeConstants;

namespace DaddyJokes.Service
{
    public class DaddyJokesService
    {
        private readonly HttpClient _httpClient;

        //We can implement distributed cache here.
        private readonly IMemoryCache _cache;

        public DaddyJokesService(IServiceScope serviceProvider)
        {
            _cache = serviceProvider.ServiceProvider.GetRequiredService<IMemoryCache>();
            _httpClient = serviceProvider.ServiceProvider.GetRequiredService<HttpClient>();
        }

        public async Task<DaddyJokesOut> GetRandomDaddyJokes()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<DaddyJokesOut>(string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching data from the server. Please check the admin for more info. Error details: {ex.Message}");
            }
        }

        public async Task<Datatable<DaddyJokesOut>> GetDaddyJokesAsync(int pageNumber = 1, int limit = 30, string searchTerm = null)
        {
            var cacheKey = string.IsNullOrEmpty(searchTerm) ? string.Format(Default, pageNumber) : $"{searchTerm}_{PageNumberConstant}_{pageNumber}";
            try
            {
                return await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(5);

                    var kvp = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>(UriConstants.Page, pageNumber.ToString()),
                        new KeyValuePair<string, string>(UriConstants.Limit, limit.ToString()),
                    };
                    if (!string.IsNullOrEmpty(searchTerm))
                        kvp.Add(new KeyValuePair<string, string>(UriConstants.Term, searchTerm));

                    var query = new QueryBuilder(kvp).ToQueryString();
                    var requestUri = UriConstants.Search + query;
                    return await _httpClient.GetFromJsonAsync<Datatable<DaddyJokesOut>>(requestUri);
                });
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
