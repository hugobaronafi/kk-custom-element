using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Core;
using Core.GitHub.Models;

using Newtonsoft.Json.Serialization;

namespace GitHub
{
    public class GitHubRepository : IGitHubRepository
    {
        private readonly HttpClient httpClient;

        public GitHubRepository(
            HttpClient httpClient,
            Settings settings
            )
        {
            this.httpClient = httpClient;

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", settings.GitHub?.AuthorizationToken);
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            this.httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(settings.GitHub?.UserAgent)));
        }

        public async Task<Tree?> CreateTree(string ownerRepo, IList<Leaf>? treeStructure)
        {
            return await Post<Tree>(GetEndpoint($"{ownerRepo}/git/trees"), new
            {
                tree = treeStructure
            });
        }

        public async Task CreatePullRequest(string ownerRepo, string head, string @base, string title, string body, bool maintainerCanModify = true)
        {
            await Post<dynamic>(GetEndpoint($"{ownerRepo}/pulls"), new
            {
                head,
                @base,
                title,
                body,
                maintainer_can_modify = maintainerCanModify
            });
        }

        public async Task CreateReference(string ownerRepo, string @ref, string sha)
        {
            await Post<dynamic>(GetEndpoint($"{ownerRepo}/git/refs"), new
            {
                @ref,
                sha,
            });
        }

        public async Task<Commit?> CreateCommit(string ownerRepo, string message, string treeSha, params string[] parentShas)
        {
            return await Post<Commit>(GetEndpoint($"{ownerRepo}/git/commits"), new
            {
                message,
                tree = treeSha,
                parents = parentShas
            });
        }

        public async Task<Leaf?> CreateBlob(string ownerRepo, byte[] content)
        {
            return await Post<Leaf>(GetEndpoint($"{ownerRepo}/git/blobs"), new
            {
                content = Convert.ToBase64String(content),
                encoding = "base64"
            });
        }

        public async Task<Leaf?> CreateBlob(string ownerRepo, string content)
        {
            return await Post<Leaf>(GetEndpoint($"{ownerRepo}/git/blobs"), new
            {
                content,
                encoding = "utf-8"
            });
        }

        public async Task<Tree?> GetTree(string ownerRepo, string sha, bool recursive = true)
        {
            if (recursive)
            {
                return await Get<Tree>(GetEndpoint($"{ownerRepo}/git/trees/{sha}?recursive=1"));
            }
            else
            {
                return await Get<Tree>(GetEndpoint($"{ownerRepo}/git/trees/{sha}"));
            }
        }

        public async Task<Branch?> GetBranch(string ownerRepo, string branchName)
        {
            return await Get<Branch>(GetEndpoint($"{ownerRepo}/branches/{branchName}"));
        }

        public async Task<Repository?> CreateFork(string ownerRepo)
        {
            return await Post<Repository>(GetEndpoint($"{ownerRepo}/forks"));
        }

        public async Task<Repository?> GetRepository(string ownerRepo)
        {
            return await Get<Repository>(GetEndpoint($"{ownerRepo}"));
        }

        public async Task<IEnumerable<PullRequest>?> ListPullRequests(string ownerRepo)
        {
            return await Get<IEnumerable<PullRequest>>(GetEndpoint($"{ownerRepo}/pulls"));
        }

        private string GetEndpoint(string relativeUri) => $"https://api.github.com/repos/{relativeUri}";

        private async Task<T?> Get<T>(string uri) where T : class
        {
            var response = await httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadAsAsync<T>();
        }

        private async Task<T?> Post<T>(string uri, object? body = null) where T : class
        {
            var response = await httpClient.PostAsync(uri, body, new JsonMediaTypeFormatter()
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            });

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadAsAsync<T>();
        }
    }
}