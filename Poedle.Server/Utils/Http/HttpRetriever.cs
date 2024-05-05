namespace Poedle.Utils.Http
{
    public class HttpRetriever
    {
        private readonly HttpClient _client;

        public HttpRetriever() => _client = new HttpClient();

        public HttpRetriever(HttpClient client) => _client = client;

        public string Get(string pUri)
        {
            return GetAsync(pUri).GetAwaiter().GetResult();
        }

        public async Task<string> GetAsync(string pUri)
        {
            using HttpResponseMessage response = await _client.GetAsync(pUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
