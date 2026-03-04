using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

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

        }
    }
}
