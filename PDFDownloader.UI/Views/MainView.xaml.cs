using System.Windows;
using PDFDownloader.UI.ViewModels;

namespace PDFDownloader.UI.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            // Setting MainViews view model
            DataContext = new MainViewModel();
        }
    }
}