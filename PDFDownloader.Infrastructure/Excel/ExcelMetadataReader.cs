using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Infrastructure.Excel
{
    public class ExcelMetadataReader : IMetadataReader
    {
        public Task<List<ReportMetadata>> ReadAsync()
        {
            List<ReportMetadata> reports = new List<ReportMetadata>();

            return Task.FromResult(reports);
        }
    }
}
