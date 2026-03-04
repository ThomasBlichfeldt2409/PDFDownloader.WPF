using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // View Models
        public HeaderViewModel HeaderViewModel { get; }
        public ConfigsViewModel ConfigsViewModel { get; }

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
            // Creating View Models 
            HeaderViewModel = new HeaderViewModel();
            ConfigsViewModel = new ConfigsViewModel();

            _reportDownloadService = reportDownloadService;

            State = DownloadState.Ready;
        }
    }
}
