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
                data.Add("Paula:Server", value);
            }

            value = await _httpClient.GetStringAsync("api/settings/demoidentity");
            if (value != null)
            {
                data.Add("Paula:DemoIdentity", value);
            }

            value = await _httpClient.GetStringAsync("api/settings/demopassword");
            if (value != null)
            {
                data.Add("Paula:DemoPassword", value);
            }

            value = await _httpClient.GetStringAsync("api/settings/build__hash");
            if (value != null)
            {
                data.Add("Build:Hash", value);
            }

            value = await _httpClient.GetStringAsync("api/settings/build__shorthash");
            if (value != null)
            {
                data.Add("Build:ShortHash", value);
            }

            value = await _httpClient.GetStringAsync("api/settings/build__branch");
            if (value != null)
            {
                data.Add("Build:Branch", value);
            }

            value = await _httpClient.GetStringAsync("api/settings/build__build");
            if (value != null)
            {
                data.Add("Build:Build", value);
            }

            Data = data;
        }
    }
}
