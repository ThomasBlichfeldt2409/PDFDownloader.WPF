namespace PDFDownloader.Core.Models
{
    public class DownloadProgress
    {
        public int Total { get; set; }
        public DownloadResult? Result { get; set; }
    }
}
