using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using GitHubRepositoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GitHubRepositoryAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class RepositoriesController : ControllerBase {

        private readonly ILogger<RepositoriesController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public RepositoriesController(  ILogger<RepositoriesController> logger,
                                        IHttpClientFactory factory) {
            _logger = logger;
            _httpClientFactory = factory;
        }

        [HttpGet]
        public async Task<IEnumerable<Repository>> Get()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".Net Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);

            foreach (var repo in repositories)
            {
                Console.WriteLine(repo.Name);
            }

            return repositories;

        }
    }
}
