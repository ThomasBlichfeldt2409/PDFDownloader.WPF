using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Services
{
    public class ReportDownloadService : IReportDownloadService
    {
        private readonly IMetadataReader _metadataReader;

        public ReportDownloadService(IMetadataReader metadataReader)
        {
            _metadataReader = metadataReader;
        }

        public async Task ExecuteAsync()
        {
            // Read URL's from the data
            List<ReportMetadata> reports = await _metadataReader.ReadAsync();
        }
    }
}
