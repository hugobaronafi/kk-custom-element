using System.Net.Http;
using System.Threading.Tasks;

using Core;
using Core.GitHub;

namespace GitHub
{
    public class BlobDownloader : IBlobDownloader
    {
        private readonly HttpClient httpClient;

        public BlobDownloader(
            HttpClient httpClient
            )
        {
            this.httpClient = httpClient;
        }

        public async Task<byte[]> DownloadBlob(string uri)
        {
            var response = await httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException($"Blob at '{uri}' is not available.");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}