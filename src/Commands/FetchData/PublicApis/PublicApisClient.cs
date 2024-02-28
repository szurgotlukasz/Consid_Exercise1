using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Exercise1.Commands.FetchData
{
    public class PublicApisClient : IPublicApisClient
    {
        private const string RequestUri = "https://api.publicapis.org/random?auth=null";
        private readonly HttpClient _httpClient;

        public PublicApisClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<PublicApisResponse> GetAsync()
        {
            var response = await _httpClient.GetStringAsync(RequestUri, new CancellationToken());
            var apiResponse = JsonConvert.DeserializeObject<PublicApisResponse>(response);
            return apiResponse;
        }
    }
}

