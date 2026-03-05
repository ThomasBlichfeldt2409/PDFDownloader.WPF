using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;
using System.Text.Json;

namespace PDFDownloader.Infrastructure.Storage
{
    public class JsonResultWriter : IResultWriter
    {
        private readonly string _outputFolderPath;

        public JsonResultWriter(string outputFolderPath)
        {
            _outputFolderPath = outputFolderPath;
        }

        public async Task WriteAsync(List<DownloadResult> results)
        {
            // Ensure directory exists
            string filePath = Path.Combine(_outputFolderPath, "results.json");
            string? directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Configure JSON formatting
            JsonSerializerOptions options = new JsonSerializerOptions
            { 
                WriteIndented = true 
            };

            // Serialize and write file
            await using FileStream stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, results, options);
        }
    }
}
