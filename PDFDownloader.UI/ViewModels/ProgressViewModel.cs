using PDFDownloader.Core.Models;

namespace PDFDownloader.UI.ViewModels
{
    public class ProgressViewModel : BaseViewModel
    {
        private int _total;
        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private int _succeeded;
        public int Succeeded
        {
            get => _succeeded;
            set
            {
                if (SetProperty(ref _succeeded, value))
                {
                    Percentage = Total == 0 ? 0 : (double)(Succeeded + Failed) / Total * 100;
                }
            }
        }

        private int _failed;
        public int Failed
        {
            get => _failed;
            set
            {
                if (SetProperty(ref _failed, value))
                {
                    Percentage = Total == 0 ? 0 : (double)(Succeeded + Failed) / Total * 100;
                }
            }
        }

        private double _percentage;
        public double Percentage
        {
            get => _percentage;
            set => SetProperty(ref _percentage, value);
        }
           
        public void Report(DownloadResult result)
        {
            if (result.IsDownloaded)
            {
                Succeeded++;
            }
            else
            {
                Failed++;
            }

            OnPropertyChanged(nameof(Percentage));
        }
    }
}
