using PDFDownloader.UI.Commands;
using System.Windows.Input;
namespace PDFDownloader.UI.ViewModels
{
    public class ConfigsViewModel : BaseViewModel
    {
        private readonly Func<Task> _startDownload;
        public ICommand StartDownloadCommand { get; }

        public ConfigsViewModel(Func<Task> startDownloading)
        {
            _startDownload = startDownloading;

            // Commands
            StartDownloadCommand = new RelayCommand(
               async _ => await StartDownload()
            );
        }

        public async Task StartDownload()
        {
            await _startDownload();
        }
    }
}
