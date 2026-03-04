using System.Windows;
using PDFDownloader.UI.ViewModels;
using PDFDownloader.Core.Services;
using PDFDownloader.Core.Interfaces;
using PDFDownloader.Infrastructure.Excel;
using PDFDownloader.Infrastructure.Download;
using PDFDownloader.Infrastructure.Storage;

namespace PDFDownloader.UI.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            // Initializing infrastructure
            IMetadataReader metadataReader = new ExcelMetadataReader();
            IReportDownloader reportDownloader = new HttpReportDownloader();
            IResultWriter resultWriter = new JsonResultWriter();

            // Creating Service
            IReportDownloadService reportDownloadService = new ReportDownloadService(metadataReader, reportDownloader, resultWriter);

            // Setting MainViews view model
            DataContext = new MainViewModel(reportDownloadService);
        }
    }
}