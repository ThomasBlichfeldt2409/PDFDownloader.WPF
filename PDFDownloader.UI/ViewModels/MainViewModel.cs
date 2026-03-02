using PDFDownloader.Core.Interfaces;

namespace PDFDownloader.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // View Models
        HeaderViewModel HeaderViewModel { get; }

        // Application Service
        private readonly IReportDownloadService _reportDownloadService;

        // Application State
        private DownloadState _state;
        public DownloadState State
        {
            get => _state;
            set
            {
                if (SetProperty(ref _state, value))
                {
                    HeaderViewModel.State = value;
                }
            }
        }
        
        public MainViewModel(IReportDownloadService reportDownloadService)
        {
            HeaderViewModel = new HeaderViewModel();

            _reportDownloadService = reportDownloadService;

            State = DownloadState.Ready;
        }
    }
}
