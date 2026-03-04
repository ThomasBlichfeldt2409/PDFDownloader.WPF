using PDFDownloader.Core.Interfaces;

namespace PDFDownloader.Infrastructure.Download
{
    public class HttpReportDownloader : IReportDownloader
    {
        public async Task<bool> DownloadAsync(string url, string filePath)
        {
            return false;
        }
    }
}
