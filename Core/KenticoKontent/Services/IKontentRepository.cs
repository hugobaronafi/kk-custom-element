using System.Collections.Generic;
using System.Threading.Tasks;

using Core.KenticoKontent.Models;
using Core.KenticoKontent.Models.Management.Items;
using Core.KenticoKontent.Models.Management.References;
using Core.KenticoKontent.Models.Management.Types;

namespace Core.KenticoKontent.Services
{
    public interface IKontentRepository
    {
        Task PrepareChangeType(PrepareChangeTypeParameters prepareItemParameters);

        Task PrepareDeepClone(PrepareDeepCloneParameters prepareDeepCloneParameters);

        Task<ItemVariant> GetItemVariant(GetItemVariantParameters getItemVariantParameters);

        Task<ItemVariant> GetNewItemVariant(GetNewItemVariantParameters getNewItemVariantParameters);

        Task<IEnumerable<ContentItem>> ListContentItems();

        Task<ContentItem> RetrieveContentItem(Reference itemReference);

        Task<IEnumerable<LanguageVariant>> ListVariantsByType(Reference typeReference);

        Task<LanguageVariant?> RetrieveLanguageVariant(RetrieveLanguageVariantParameters retrieveLanguageVariantParameters);

        ExternalIdReference NewExternalIdReference();

        Task<ContentItem> UpsertContentItem(ContentItem contentItem);

        Task UpsertLanguageVariant(UpsertLanguageVariantParameters upsertLanguageVariantParameters);

        Task<IEnumerable<ContentType>> ListContentTypes();

        Task<ContentType> RetrieveContentType(Reference typeReference);

        Task<ContentType> RetrieveContentTypeSnippet(Reference typeReference);

        Task<IEnumerable<WorkflowStep>> RetrieveWorkflowSteps();

        Task CreateNewVersionLanguageVariant(UpsertLanguageVariantParameters upsertLanguageVariantParameters);

        Task PublishLanguageVariant(UpsertLanguageVariantParameters upsertLanguageVariantParameters);

        Task ChangeWorkflowStepLanguageVariant(ChangeWorkflowStepParameters changeWorkflowStepParameters);

        Task<Asset> RetrieveAsset(Reference assetReference);
    }
}