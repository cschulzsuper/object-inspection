using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;

namespace BlazorApi
{
    public class Function
    {
        private readonly IConfiguration _configuration;

        public Function(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [FunctionName("settings")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "settings/{key}")] HttpRequest request, string key)
        {

            return new OkObjectResult(_configuration[key]);
        }
    }
}
