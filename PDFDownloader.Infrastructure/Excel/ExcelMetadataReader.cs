using ClosedXML.Excel;
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

            // Open the Excel file
            using XLWorkbook workbook = new XLWorkbook(_filePath);

            // Gets the first sheet in the workbook
            IXLWorksheet worksheet = workbook.Worksheet(1);

            // RangeUsed() -> Returns the area that contains data
            // RowUsed() -> Returns only rows that contains data
            // Skip() -> Skips that amount of rows
            IEnumerable<IXLRangeRow> rows = worksheet.RangeUsed()!.RowsUsed().Skip(_startRow - 1);

            foreach (IXLRangeRow row in rows)
            {
                string brNummer = row.Cell(_brNummerColumn).GetString().Trim();
                string primaryUrl = row.Cell(_primaryUrlColumn).GetString().Trim();
                string secondaryUrl = row.Cell(_secondaryUrlColumn).GetString().Trim();

                if (string.IsNullOrWhiteSpace(brNummer))
                    continue;

                // If there is no secondaryUrl in the Excel worksheet
                // SecondaryUrl will be null, else it will be secondaryUrl
                reports.Add(new ReportMetadata
                {
                    BRNummer = brNummer,
                    PrimaryUrl = primaryUrl,
                    SecondaryUrl = string.IsNullOrWhiteSpace(secondaryUrl) ? null : secondaryUrl
                });
            }

            return Task.FromResult(reports);
        }
    }
}
