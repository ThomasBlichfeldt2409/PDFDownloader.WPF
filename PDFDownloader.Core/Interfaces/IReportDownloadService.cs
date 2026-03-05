using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Interfaces
{
    public interface IReportDownloadService
    {
        Task ExecuteAsync(IProgress<DownloadProgress>? progress = null);
    }
}
