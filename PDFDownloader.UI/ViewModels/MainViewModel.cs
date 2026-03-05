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
        public ProgressViewModel ProgressViewModel { get; }

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
                    ConfigsViewModel.State = value;
                }
            }
        }

        public MainViewModel()
        {
            // Creating View Models 
            HeaderViewModel = new HeaderViewModel();
            ConfigsViewModel = new ConfigsViewModel(StartDownloadAsync);
            ProgressViewModel = new ProgressViewModel();

            State = DownloadState.Ready;
        }

        private async Task StartDownloadAsync()
        {
            State = DownloadState.Downloading;

            IReportDownloadService reportDownloadService = InitializeInfrastructure();

            Progress<DownloadProgress> progress = new Progress<DownloadProgress>(p =>
            {
                if (p.Total > 0)
                {
                    ProgressViewModel.Total = p.Total;
                    return;
                }

                if (p.Result != null)
                {
                    ProgressViewModel.Report(p.Result);
                }
            });

            await reportDownloadService.ExecuteAsync(progress);

            State = DownloadState.Finished;
        }

        private IReportDownloadService InitializeInfrastructure()
        {
            IMetadataReader metadataReader = new ExcelMetadataReader(
                ConfigsViewModel.ExcelFilePath,
                ConfigsViewModel.StartingRow,
                ConfigsViewModel.BrNummer,
                ConfigsViewModel.PrimaryUrl,
                ConfigsViewModel.SecondaryUrl);

            IReportDownloader reportDownloader = new HttpReportDownloader();

            IResultWriter resultWriter = new JsonResultWriter(ConfigsViewModel.OutputFolderPath);

            return new ReportDownloadService(
                metadataReader, 
                reportDownloader, 
                resultWriter,
                ConfigsViewModel.OutputFolderPath,
                ConfigsViewModel.MaxConcurrency);
        }
    }
}
