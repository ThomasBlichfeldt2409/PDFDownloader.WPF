namespace PDFDownloader.UI.ViewModels
{
    public class HeaderViewModel : BaseViewModel
    {
        private DownloadState _state;
        public DownloadState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
    }
}
