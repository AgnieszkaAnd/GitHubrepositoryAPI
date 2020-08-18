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
            client = ConfigureClientAndGetRequest(client);

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);

            return repositories;
        }

        [HttpGet]
        [Route("{owner}")]
        public async Task<IEnumerable<Repository>> GetUserRepos(string owner) {
            HttpClient client = _httpClientFactory.CreateClient();
            client = ConfigureClientAndGetRequest(client);

            var streamTask = client.GetStreamAsync($"https://api.github.com/users/{owner}/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);

            return repositories;
        }

        [HttpGet]
        [Route("{owner}/{repositoryName}")]
        public async Task<Repository> GetUserRepos(string owner, string repositoryName) {
            HttpClient client = _httpClientFactory.CreateClient();
            client = ConfigureClientAndGetRequest(client);

            var streamTask = client.GetStreamAsync($"https://api.github.com/repos/{owner}/{repositoryName}");
            var repository = await JsonSerializer.DeserializeAsync<Repository>(await streamTask);

            return repository;
        }

        private HttpClient ConfigureClientAndGetRequest(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".Net Foundation Repository Reporter");

            return client;
        }
    }
}
