using System.Threading.Tasks;

using Core.AzureTranslator.Models;

namespace Core.AzureTranslator.Services
{
    public interface ITranslationService
    {
        Task<TranslationResult> Translate(string text, string language);
    }
}