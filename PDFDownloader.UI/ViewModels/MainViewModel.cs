using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;
using PDFDownloader.Core.Services;
using PDFDownloader.Infrastructure.Download;
using PDFDownloader.Infrastructure.Excel;
using PDFDownloader.Infrastructure.Storage;

namespace PDFDownloader.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // View Models
        public HeaderViewModel HeaderViewModel { get; }
        public ConfigsViewModel ConfigsViewModel { get; }

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

        public MainViewModel()
        {
            // Creating View Models 
            HeaderViewModel = new HeaderViewModel();
            ConfigsViewModel = new ConfigsViewModel(StartDownloadAsync);

            State = DownloadState.Ready;
        }

        private async Task StartDownloadAsync()
        {
            State = DownloadState.Downloading;

            IReportDownloadService reportDownloadService = InitializeInfrastructure();
            await reportDownloadService.ExecuteAsync();

            State = DownloadState.Finished;
        }

        private IReportDownloadService InitializeInfrastructure()
        {
            IMetadataReader metadataReader = new ExcelMetadataReader();
            IReportDownloader reportDownloader = new HttpReportDownloader();
            IResultWriter resultWriter = new JsonResultWriter();

            return new ReportDownloadService(metadataReader, reportDownloader, resultWriter);
        }
    }
}
