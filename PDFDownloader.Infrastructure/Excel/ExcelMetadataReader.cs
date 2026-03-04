using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Infrastructure.Excel
{
    public class ExcelMetadataReader : IMetadataReader
    {
        private readonly string _filePath;
        private readonly int _startRow;
        private readonly string _brNummerColumn;
        private readonly string _primaryUrlColumn;
        private readonly string _secondaryUrlColumn;

        public ExcelMetadataReader(string filePath, int startRow, string brNummerColumn, string primaryUrlColumn, string secondaryUrlColumn)
        {
            _filePath = filePath;
            _startRow = startRow;
            _brNummerColumn = brNummerColumn;
            _primaryUrlColumn = primaryUrlColumn;
            _secondaryUrlColumn = secondaryUrlColumn;
        }

        public Task<List<ReportMetadata>> ReadAsync()
        {
            List<ReportMetadata> reports = new List<ReportMetadata>();

            return Task.FromResult(reports);
        }
    }
}
