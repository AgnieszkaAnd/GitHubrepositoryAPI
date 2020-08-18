using System;
using System.Text.Json.Serialization;

namespace GitHubRepositoryAPI.Models {
    public class Repository {

        [JsonPropertyName("full_name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("clone_url")]
        public Uri GitHubCloneUrl { get; set; }

        [JsonPropertyName("stargazers_count")]
        public int StargazersNb { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
