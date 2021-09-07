using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Core.KenticoKontent.Models;
using Core.KenticoKontent.Models.Management.Items;
using Core.KenticoKontent.Models.Management.References;
using Core.KenticoKontent.Services;

using Functions.Functions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Functions.Elements
{
    public class KontentDeepClone : BaseFunction
    {
        private readonly IKontentRepository kontentRepository;
        private readonly IKontentApiTracker kontentApiTracker;

        public KontentDeepClone(
            ILogger<KontentDeepClone> logger,
            IKontentRepository kontentRepository,
            IKontentApiTracker kontentApiTracker
            ) : base(logger)
        {
            this.kontentRepository = kontentRepository;
            this.kontentApiTracker = kontentApiTracker;
        }

        [FunctionName(nameof(KontentDeepClone))]
        public async Task<IActionResult> Run(
            [HttpTrigger(
                "post",
                Route = Routes.KontentDeepClone
            )] string body,
            string itemCodename,
            string languageCodename
            )
        {
            try
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();

                kontentApiTracker.ApiCalls = 0;

                var oldItemReference = new CodenameReference(itemCodename);
                var newItemReference = kontentRepository.NewExternalIdReference();
                var languageReference = new CodenameReference(languageCodename);
                var newItemVariants = new Dictionary<Reference, ItemVariant>();

                await kontentRepository.PrepareDeepClone(new PrepareDeepCloneParameters
                {
                    ItemVariant = await kontentRepository.GetNewItemVariant(new GetNewItemVariantParameters
                    {
                        OldItemReference = oldItemReference,
                        NewItemReference = newItemReference,
                        LanguageReference = languageReference,
                        NewItemVariants = newItemVariants
                    }),
                    LanguageReference = languageReference,
                    NewItemVariants = newItemVariants
                });

                var newItems = new List<ContentItem>();

                foreach (var (newItem, newVariant) in newItemVariants.Values)
                {
                    newItems.Add(await kontentRepository.UpsertContentItem(newItem));

                    await kontentRepository.UpsertLanguageVariant(new UpsertLanguageVariantParameters
                    {
                        LanguageReference = languageReference,
                        Variant = newVariant
                    });
                }

                stopwatch.Stop();

                return LogOkObject(new
                {
                    TotalApiCalls = kontentApiTracker.ApiCalls,
                    TotalMilliseconds = stopwatch.ElapsedMilliseconds,
                    NewItems = newItems
                });
            }
            catch (Exception ex)
            {
                return LogException(ex);
            }
        }
    }
}