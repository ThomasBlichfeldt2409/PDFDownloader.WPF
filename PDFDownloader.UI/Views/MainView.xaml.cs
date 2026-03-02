using System.Windows;
using PDFDownloader.UI.ViewModels;
using PDFDownloader.Core.Services;
using PDFDownloader.Core.Interfaces;

namespace PDFDownloader.UI.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            IReportDownloadService reportDownloadService = new ReportDownloadService();
            DataContext = new MainViewModel(reportDownloadService);
        }
    }
}