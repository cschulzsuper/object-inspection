using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Super.Paula.Client.Configuration
{
    public class BlazorApiConfigurationProvider : ConfigurationProvider
    {
        private readonly HttpClient _httpClient;

        public BlazorApiConfigurationProvider(string functionsUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(functionsUrl)
            };
        }
        public async Task LoadAsync()
        {
            var data = new Dictionary<string, string>();

            var value = await _httpClient.GetStringAsync("api/settings/server");
            if (value != null)
            {
                data.Add("Server", value);
            }

            Data = data;
        }
    }
}
