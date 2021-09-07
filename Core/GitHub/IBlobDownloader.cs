using System.Threading.Tasks;

namespace Core.GitHub
{
    public interface IBlobDownloader
    {
        Task<byte[]> DownloadBlob(string uri);
    }
}