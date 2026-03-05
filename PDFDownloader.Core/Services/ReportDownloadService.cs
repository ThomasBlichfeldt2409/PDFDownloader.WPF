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

        public async Task ExecuteAsync()
        {
            // Read URL's from the data
            List<ReportMetadata> reports = await _metadataReader.ReadAsync();
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
                Task task = Task.Run(async () =>
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

                        // Add result - lock ensures only one thread may enter this block at a time
                        lock (_resultsLock)
                        {
                            results.Add(new DownloadResult
                            {
                                BRNummer = report.BRNummer,
                                IsDownloaded = isDownloaded
                            });
                        }

                    }
                    finally
                    {
                        // Release task to make room for new one
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            // Waits until every download is finished
            await Task.WhenAll(tasks);

            // Create JSON file with results
            await _resultWriter.WriteAsync(results);
        }
    }
}
