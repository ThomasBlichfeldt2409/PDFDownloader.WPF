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
            // Delete this line later
            await Task.Delay(2000);

            // Read URL's from the data
            List<ReportMetadata> reports = await _metadataReader.ReadAsync();

            List<DownloadResult> results = new List<DownloadResult>();

            await _resultWriter.WriteAsync(results);
        }
    }
}
