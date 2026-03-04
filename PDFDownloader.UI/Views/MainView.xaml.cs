using System.Windows;
using PDFDownloader.UI.ViewModels;
using PDFDownloader.Core.Services;
using PDFDownloader.Core.Interfaces;
using PDFDownloader.Infrastructure.Excel;

namespace PDFDownloader.UI.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            // Initializing infrastructure
            IMetadataReader metadataReader = new ExcelMetadataReader();

            // Creating Service
            IReportDownloadService reportDownloadService = new ReportDownloadService(metadataReader);

            // Setting MainViews view model
            DataContext = new MainViewModel(reportDownloadService);
        }
    }
}