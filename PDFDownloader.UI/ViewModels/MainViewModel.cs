using PDFDownloader.Core.Interfaces;

namespace PDFDownloader.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IReportDownloadService _reportDownloadService;

        public MainViewModel(IReportDownloadService reportDownloadService)
        {
            _reportDownloadService = reportDownloadService;
        }

    }
}
