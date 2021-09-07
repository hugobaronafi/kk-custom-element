using System.Collections.Generic;
using System.Threading.Tasks;

using Core.GitHub.Models;

namespace GitHub
{
    public interface IGitHubRepository
    {
        Task CreatePullRequest(string ownerRepo, string head, string @base, string title, string body, bool maintainerCanModify = true);

        Task CreateReference(string ownerRepo, string @ref, string sha);

        Task<Commit?> CreateCommit(string ownerRepo, string message, string treeSha, params string[] parentShas);

        Task<Leaf?> CreateBlob(string ownerRepo, byte[] content);

        Task<Leaf?> CreateBlob(string ownerRepo, string content);

        Task<Tree?> GetTree(string ownerRepo, string sha, bool recursive = true);

        Task<Branch?> GetBranch(string ownerRepo, string branchName);

        Task<Repository?> CreateFork(string ownerRepo);

        Task<IEnumerable<PullRequest>?> ListPullRequests(string ownerRepo);

        Task<Repository?> GetRepository(string ownerRepo);

        Task<Tree?> CreateTree(string ownerRepo, IList<Leaf>? treeStructure);
    }
}