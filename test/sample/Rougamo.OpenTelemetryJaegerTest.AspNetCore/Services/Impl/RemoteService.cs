using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services.Impl
{
    public class RemoteService : IRemoteService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RemoteService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> LocalPostAsync(int timeout)
        {
            using var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
            var content = new StringContent(string.Empty);
            var response = await client.PostAsync("http://127.0.0.1:5000/Test", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
