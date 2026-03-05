using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Services
{
    public class ReportDownloadService : IReportDownloadService
    {
        private readonly IMetadataReader _metadataReader;
        private readonly IReportDownloader _reportDownloader;
        private readonly IResultWriter _resultWriter;
        private readonly string _outputFolderPath;
        private readonly int _maxConcurrency;

        private readonly object _resultsLock = new object();

        public ReportDownloadService(
            IMetadataReader metadataReader, 
            IReportDownloader reportDownloader, 
            IResultWriter resultWriter, 
            string outputFolderPath, 
            int maxConcurrency)
        {
            _metadataReader = metadataReader;
            _reportDownloader = reportDownloader;
            _resultWriter = resultWriter;
            _outputFolderPath = outputFolderPath;
            _maxConcurrency = maxConcurrency;
        }

        public async Task ExecuteAsync(IProgress<DownloadProgress>? progress = null)
        {
            // Read URL's from the data
            List<ReportMetadata> reports = await _metadataReader.ReadAsync();

            progress?.Report(new DownloadProgress
            {
                Total = reports.Count
            });

            List<DownloadResult> results = new List<DownloadResult>();

            // Concurrency Controller - max of _maxConcurrency at a time
            using SemaphoreSlim semaphore = new SemaphoreSlim(_maxConcurrency);

            // Storage for all running tasks
            List<Task> tasks = new List<Task>();

            foreach (ReportMetadata report in reports)
            {
                // Wait for available slot
                await semaphore.WaitAsync();

                // create task
                Task task = DownloadReportAsync(report, semaphore, results, progress);
                
                tasks.Add(task);
            }

            // Waits until every download is finished
            await Task.WhenAll(tasks);

            // Create JSON file with results
            await _resultWriter.WriteAsync(results);
        }

        private async Task DownloadReportAsync(
            ReportMetadata report, 
            SemaphoreSlim semaphore, 
            List<DownloadResult> results, 
            IProgress<DownloadProgress>? progress)
        {
            try
            {
                bool isDownloaded = false;

                string filePath = Path.Combine(_outputFolderPath, $"{report.BRNummer}.pdf");

                // Try Primary URL
                if (!string.IsNullOrWhiteSpace(report.PrimaryUrl))
                {
                    isDownloaded = await _reportDownloader.DownloadAsync(report.PrimaryUrl, filePath);
                }

                // If Primary URL failed, try Secondary URL
                if (!string.IsNullOrWhiteSpace(report.SecondaryUrl) && !isDownloaded)
                {
                    isDownloaded = await _reportDownloader.DownloadAsync(report.SecondaryUrl, filePath);
                }

                // Create result
                DownloadResult result = new DownloadResult
                {
                    BRNummer = report.BRNummer,
                    IsDownloaded = isDownloaded
                };

                lock (_resultsLock)
                {
                    results.Add(result);
                }

                progress?.Report(new DownloadProgress
                {
                    Result = result
                });
            }
            finally
            {
                // Release task to make room for new one
                semaphore.Release();
            }
        }
    }
}
