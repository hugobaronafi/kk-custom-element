using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Core.KenticoKontent.Models.Management.Elements;
using Core.KenticoKontent.Models.Management.Items;
using Core.KenticoKontent.Models.Management.References;
using Core.KenticoKontent.Services;

using Functions.Functions;
using Functions.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Functions.Elements
{
    public class KontentChangeType : BaseFunction
    {
        private readonly IKontentRepository kontentRepository;
        private readonly IKontentApiTracker kontentApiTracker;

        public KontentChangeType(
            ILogger<KontentChangeType> logger,
            IKontentRepository kontentRepository,
            IKontentApiTracker kontentApiTracker
            ) : base(logger)
        {
            this.kontentRepository = kontentRepository;
            this.kontentApiTracker = kontentApiTracker;
        }

        [FunctionName(nameof(KontentChangeType))]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                "post",
                Route = Routes.KontentChangeType
            )] ChangeTypeRequest changeTypeRequest,
            string itemCodename,
            string languageCodename
            )
        {
            try
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();

                kontentApiTracker.ApiCalls = 0;

                var (elementMappings, selectedType) = changeTypeRequest;

                var oldItemReference = new CodenameReference(itemCodename);
                var languageReference = new CodenameReference(languageCodename);
                var newTypeReference = new IdReference(selectedType.Id!);

                var newItemReference = kontentRepository.NewExternalIdReference();

                var oldItemVariant = await kontentRepository.GetItemVariant(new GetItemVariantParameters
                {
                    ItemReference = oldItemReference,
                    LanguageReference = languageReference
                });

                var (oldItem, oldVariant) = oldItemVariant;

                var newElements = new List<IElement>();

                foreach (var pair in elementMappings)
                {
                    var newTypeElementReferenceId = pair.Key;
                    var newElement = selectedType.Elements.First(element => element.Id == newTypeElementReferenceId);

                    var oldElementReference = pair.Value;
                    var oldElement = oldVariant.Elements.First(element => element.Element?.Value == oldElementReference);
                    var oldElementValue = (oldElement as dynamic).Value;

                    var newTypeElementReference = new IdReference(newTypeElementReferenceId);

                    switch (newElement.Type)
                    {
                        case UrlSlugElement.Type:
                            newElements.Add(new UrlSlugElement { Element = newTypeElementReference, Value = oldElementValue?.ToString(), Mode = "custom" });
                            break;

                        case "guidelines":
                        case TextElement.Type:
                            newElements.Add(new TextElement { Element = newTypeElementReference, Value = oldElementValue?.ToString() });
                            break;

                        default:
                            oldElement.Element = newTypeElementReference;
                            newElements.Add(oldElement);
                            break;
                    }
                }

                oldVariant.Elements = newElements;

                var newVariants = new Dictionary<Reference, LanguageVariant>();

                await kontentRepository.PrepareChangeType(new PrepareChangeTypeParameters
                {
                    ItemVariant = oldItemVariant,
                    LanguageReference = languageReference,
                    NewItemReference = newItemReference,
                    NewVariants = newVariants
                });

                oldItem.Codename = null;
                oldItem.ExternalId = newItemReference.Value;
                oldItem.TypeReference = newTypeReference;
                oldVariant.ItemReference = newItemReference;

                var newItem = await kontentRepository.UpsertContentItem(oldItem);

                await kontentRepository.UpsertLanguageVariant(new UpsertLanguageVariantParameters
                {
                    LanguageReference = languageReference,
                    Variant = oldVariant
                });

                var allWorkflowSteps = await kontentRepository.RetrieveWorkflowSteps();

                var draftStep = allWorkflowSteps.First(step => step.Name == "Draft");
                var publishedStep = allWorkflowSteps.First(step => step.Name == "Published");
                var archivedStep = allWorkflowSteps.First(step => step.Name == "Archived");

                foreach (var newVariant in newVariants.Values)
                {
                    var languageVariantParameters = new UpsertLanguageVariantParameters
                    {
                        LanguageReference = languageReference,
                        Variant = newVariant
                    };

                    if (newVariant.WorkflowStep?.Value! == publishedStep.Id)
                    {
                        await kontentRepository.CreateNewVersionLanguageVariant(languageVariantParameters);
                    }

                    if (newVariant.WorkflowStep?.Value! == archivedStep.Id)
                    {
                        await kontentRepository.ChangeWorkflowStepLanguageVariant(new ChangeWorkflowStepParameters
                        {
                            WorkflowStepReference = new IdReference(draftStep.Id!),
                            LanguageReference = languageReference,
                            Variant = newVariant
                        });
                    }

                    await kontentRepository.UpsertLanguageVariant(languageVariantParameters);

                    if (newVariant.WorkflowStep?.Value! == publishedStep.Id)
                    {
                        await kontentRepository.PublishLanguageVariant(languageVariantParameters);
                    }
                    else
                    {
                        await kontentRepository.ChangeWorkflowStepLanguageVariant(new ChangeWorkflowStepParameters
                        {
                            WorkflowStepReference = newVariant.WorkflowStep,
                            LanguageReference = languageReference,
                            Variant = newVariant
                        });
                    }
                }

                var allItems = await kontentRepository.ListContentItems();

                var updatedItems = allItems.Where(item => newVariants.Keys.Any(reference => reference.Value == item.Id));

                stopwatch.Stop();

                return LogOkObject(new
                {
                    TotalApiCalls = kontentApiTracker.ApiCalls,
                    TotalMilliseconds = stopwatch.ElapsedMilliseconds,
                    NewItem = newItem,
                    UpdatedItems = updatedItems
                });
            }
            catch (Exception ex)
            {
                return LogException(ex);
            }
        }
    }
}