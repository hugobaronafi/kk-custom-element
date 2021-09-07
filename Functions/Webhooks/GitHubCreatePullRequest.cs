using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core;
using Core.GitHub;
using Core.GitHub.Models;
using Core.KenticoKontent.Models.Management.Elements;
using Core.KenticoKontent.Models.Management.References;
using Core.KenticoKontent.Models.Management.Types;
using Core.KenticoKontent.Services;

using Functions.Functions;
using Functions.Models;

using GitHub;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Functions.Webhooks
{
    public class GitHubCreatePullRequest : BaseFunction
    {
        private readonly IWebhookValidator webhookValidator;
        private readonly IKontentRepository kontentRepository;
        private readonly IBlobDownloader blobDownloader;
        private readonly IGitHubRepository gitHubRepository;

        public GitHubCreatePullRequest(
            ILogger<GitHubCreatePullRequest> logger,
            IWebhookValidator webhookValidator,
            IKontentRepository kontentRepository,
            IBlobDownloader blobDownloader,
            IGitHubRepository gitHubRepository
            ) : base(logger)
        {
            this.webhookValidator = webhookValidator;
            this.kontentRepository = kontentRepository;
            this.blobDownloader = blobDownloader;
            this.gitHubRepository = gitHubRepository;
        }

        [FunctionName(nameof(GitHubCreatePullRequest))]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                "post",
                Route = Routes.GitHubCreatePullRequest
            )] string body,
            IDictionary<string, string> headers,
            string typeCodename,
            string baseName,
            string repositoryName,
            string headName
            )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseName)) throw new ApiException("Base name is null or empty.");
                if (string.IsNullOrWhiteSpace(repositoryName)) throw new ApiException("Repository name is null or empty.");
                if (string.IsNullOrWhiteSpace(headName)) throw new ApiException("Head name is null or empty.");

                var (valid, getWebhook) = webhookValidator.ValidateWebhook(body, headers);

                //if (!valid) return LogUnauthorized();

                var (data, message) = getWebhook();

                switch (message.Type)
                {
                    case "content_item_variant":
                        switch (message.Operation)
                        {
                            case "change_workflow_step":
                                if (data.Items == null) throw new ApiException("Webhook does not have items.");

                                foreach (var item in data.Items)
                                {
                                    if (item.ItemReference == null) throw new ApiException("Item not available.");

                                    var contentItem = await kontentRepository.RetrieveContentItem(item.ItemReference);

                                    if (contentItem.TypeReference == null) throw new ApiException("Item type not available.");

                                    var contentType = await kontentRepository.RetrieveContentType(contentItem.TypeReference);

                                    if (contentType.Codename != typeCodename)
                                    {
                                        continue;
                                    }

                                    var languageVariant = await kontentRepository.RetrieveLanguageVariant(new RetrieveLanguageVariantParameters
                                    {
                                        ItemReference = item.ItemReference,
                                        LanguageReference = item.LanguageReference,
                                        TypeReference = new CodenameReference(typeCodename)
                                    });

                                    if (languageVariant == null) throw new ApiException("Variant could not be retrieved.");
                                    if (languageVariant.Elements == null) throw new ApiException("Variant does not have elements.");
                                    if (contentType.Elements == null) throw new ApiException("Type does not have elements.");

                                    var (title, description, motivation, readmeUrl, categories, thumbnail, thumbnailType) = await ParseElements(languageVariant.Elements, contentType.Elements);

                                    var branch = ToKebabCase(title);

                                    await RunPullRequest(
                                        $"{baseName}/{repositoryName}",
                                        $"{headName}:{branch}",
                                        $"{headName}/{repositoryName}",
                                        branch,
                                        title,
                                        description,
                                        motivation,
                                        readmeUrl,
                                        categories,
                                        thumbnail,
                                        thumbnailType
                                        );
                                }
                                break;
                        }
                        break;
                }

                return LogOk();
            }
            catch (ApiException ex) when (ex.Skip)
            {
                return LogOkException(ex);
            }
            catch (Exception ex)
            {
                return LogException(ex);
            }
        }

        public async Task<(string title, string description, string motivation, string readmeUrl, IList<string> categories, byte[] thumbnail, string thumbnailType)> ParseElements(
            IList<IElement> languageVariantElements,
            IList<ElementType> contentTypeElements
            )
        {
            string? title = null;
            string? description = null;
            string? motivation = null;
            string? readme_url = null;

            var categories = new List<string>();

            byte[]? thumbnail = null;
            string? thumbnailType = null;

            foreach (var element in languageVariantElements)
            {
                var elementType = contentTypeElements.First(typeElement => typeElement.Id == element.Element?.Value);

                switch (element)
                {
                    case TextElement textElement when elementType.Codename == nameof(title):
                        title = textElement.Value;

                        break;

                    case TextElement textElement when elementType.Codename == nameof(description):
                        description = textElement.Value;

                        break;

                    case TextElement textElement when elementType.Codename == nameof(motivation):
                        motivation = textElement.Value;

                        break;

                    case TextElement textElement when elementType.Codename == nameof(readme_url):
                        readme_url = textElement.Value;

                        break;

                    case AssetElement assetElement when elementType.Codename == nameof(thumbnail):
                        var thumbnailAsset = await kontentRepository.RetrieveAsset(assetElement.Value[0]);

                        if (string.IsNullOrWhiteSpace(thumbnailAsset.FileName)) throw new ApiException("Asset does not have a file name.");
                        if (string.IsNullOrWhiteSpace(thumbnailAsset.Url)) throw new ApiException("Asset does not have a url.");

                        thumbnailType = thumbnailAsset.FileName;
                        thumbnailType = thumbnailType[thumbnailType.LastIndexOf('.')..];

                        thumbnail = await blobDownloader.DownloadBlob(thumbnailAsset.Url);

                        break;

                    case MultipleChoiceElement multipleChoiceElement when elementType.Codename == nameof(categories):
                        if (elementType.Options is null) throw new ApiException("Element does not have options.");

                        foreach (var optionReference in multipleChoiceElement.Value)
                        {
                            var option = elementType.Options.First(elementTypeOption => elementTypeOption.Id == optionReference.Value);

                            if (string.IsNullOrWhiteSpace(option.Name)) throw new ApiException("Option does not have a name.");

                            categories.Add(option.Name);
                        }

                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(title)) throw new ApiException("Title is null or empty.");
            if (string.IsNullOrWhiteSpace(description)) throw new ApiException("Description is null or empty.");
            if (string.IsNullOrWhiteSpace(motivation)) throw new ApiException("Motivation is null or empty.");
            if (string.IsNullOrWhiteSpace(readme_url)) throw new ApiException("Readme URL is null or empty.");
            if (string.IsNullOrWhiteSpace(thumbnailType)) throw new ApiException("Thumbnail type is null or empty.");
            if (thumbnail is null) throw new ApiException("Thumbnail does not exist.");

            return (title, description, motivation, readme_url, categories, thumbnail, thumbnailType);
        }

        public async Task RunPullRequest(
            string parent,
            string head,
            string fork,
            string branch,
            string title,
            string description,
            string motivation,
            string readmeUrl,
            IList<string> categories,
            byte[] thumbnail,
            string thumbnailType
            )
        {
            var pullRequests = await gitHubRepository.ListPullRequests(parent);

            if (!(pullRequests is null))
            {
                foreach (var pullRequest in pullRequests)
                {
                    if (pullRequest.Head == null) throw new ApiException("Pull request does not have a head.");

                    if (pullRequest.Head.Label == head)
                    {
                        throw new ApiException("Pull request already exists.", true);
                    }
                }
            }

            var forkRepository = await gitHubRepository.GetRepository(fork);

            if (forkRepository is null)
            {
                await gitHubRepository.CreateFork(parent);
            }

            var attempt = 0;
            var maxAttempts = 10;

            while (forkRepository is null)
            {
                if (attempt == maxAttempts)
                {
                    throw new ApiException("Fork failed to be made.");
                }

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt++ / 2d)));

                forkRepository = await gitHubRepository.GetRepository(fork);
            }

            var parentMainBranch = await gitHubRepository.GetBranch(parent, "master");

            if (parentMainBranch is null) throw new ApiException("Branch does not exist.");
            if (parentMainBranch.Commit is null) throw new ApiException("Branch does not have a commit.");
            if (parentMainBranch.Commit.Sha is null) throw new ApiException("Branch commit does not have a SHA.");

            var parentMainBranchTree = await gitHubRepository.GetTree(parent, parentMainBranch.Commit.Sha);

            if (parentMainBranchTree is null) throw new ApiException("Tree does not exist.");
            if (parentMainBranchTree.Sha is null) throw new ApiException("Tree does not have a SHA.");
            if (parentMainBranchTree.TreeStructure is null) throw new ApiException("Tree does not have a structure.");

            var fileName = ToPascalCase(title);

            var elementJsonBlob = await gitHubRepository.CreateBlob(fork, JsonConvert.SerializeObject(new
            {
                title,
                description,
                thumbnailUrl = $"../assets/{fileName}{thumbnailType}",
                readmeUrl,
                categories,
            }));

            if (elementJsonBlob is null) throw new ApiException("Blob does not exist.");

            elementJsonBlob.Path = $"src/data/elements/{fileName}.json";
            elementJsonBlob.Mode = "100644";
            elementJsonBlob.Type = "blob";

            parentMainBranchTree.TreeStructure.Add(elementJsonBlob);

            var thumbnailBlob = await gitHubRepository.CreateBlob(fork, thumbnail);

            if (thumbnailBlob is null) throw new ApiException("Blob does not exist.");

            thumbnailBlob.Path = $"src/data/assets/{fileName}{thumbnailType}";
            thumbnailBlob.Mode = "100644";
            thumbnailBlob.Type = "blob";
            thumbnailBlob.Size = thumbnail.Length;

            parentMainBranchTree.TreeStructure.Add(thumbnailBlob);

            var commitTree = await gitHubRepository.CreateTree(fork, parentMainBranchTree.TreeStructure);

            if (commitTree is null) throw new ApiException("Tree does not exist.");
            if (commitTree.Sha is null) throw new ApiException("Tree does not have a SHA.");

            var commit = await gitHubRepository.CreateCommit(fork, $"Add {title}", commitTree.Sha, parentMainBranch.Commit.Sha);

            if (commit is null) throw new ApiException("Commit does not exist.");
            if (commit.Sha is null) throw new ApiException("Commit does not have a SHA.");

            await gitHubRepository.CreateReference(fork, $"refs/heads/{branch}", commit.Sha);

            await gitHubRepository.CreatePullRequest(
                parent,
                head,
                "master",
                $"Add {title}",
@$"# {title}

## {description}

## Motivation

{motivation}

## Categories

{string.Join(", ", categories)}");
        }

        private string ToPascalCase(string source)
        {
            var lowercaseSource = source.Aggregate(
                new StringBuilder(),
                (result, next) =>
                {
                    if (char.IsLetterOrDigit(next))
                    {
                        result.Append(char.ToLower(next));
                    }
                    else
                    {
                        result.Append('_');
                    }

                    return result;
                }
            );

            var pascalCaseSource = lowercaseSource
                .ToString()
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => $"{char.ToUpper(part[0])}{part[1..]}");

            return string.Join("", pascalCaseSource);
        }

        private string ToKebabCase(string source)
        {
            var lowercaseSource = source.Aggregate(
                new StringBuilder(),
                (result, next) =>
                {
                    if (char.IsLetterOrDigit(next))
                    {
                        result.Append(char.ToLower(next));
                    }
                    else
                    {
                        result.Append('-');
                    }

                    return result;
                }
            );

            return lowercaseSource
                .ToString();
        }
    }
}