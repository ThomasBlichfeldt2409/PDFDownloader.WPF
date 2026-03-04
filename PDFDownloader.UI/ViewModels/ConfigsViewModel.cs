using Microsoft.Win32;
using System.Windows.Input;
using PDFDownloader.Core.Models;
using PDFDownloader.UI.Commands;

namespace PDFDownloader.UI.ViewModels
{
    public class ConfigsViewModel : BaseViewModel
    {
        private readonly Func<Task> _startDownload;
        public ICommand StartDownloadCommand { get; }
        public ICommand BrowseExcelCommand { get; }
        public ICommand BrowseFolderCommand { get; }

        private DownloadState _state;
        public DownloadState State
        {
            get => _state;
            set
            {
                if (SetProperty(ref _state, value))
                {
                    // Re-evaluate buttons
                    (StartDownloadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (BrowseExcelCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (BrowseFolderCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _excelFilePath = string.Empty;
        public string ExcelFilePath
        {
            get => _excelFilePath;
            set
            {
                if (SetProperty(ref _excelFilePath, value))
                {
                    // Re-evaluate buttons
                    (StartDownloadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _outputFolderPath = string.Empty;
        public string OutputFolderPath
        {
            get => _outputFolderPath;
            set
            {
                if (SetProperty(ref _outputFolderPath, value))
                {
                    // Re-evaluate buttons
                    (StartDownloadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ConfigsViewModel(Func<Task> startDownloading)
        {
            _startDownload = startDownloading;

            // Commands
            StartDownloadCommand = new RelayCommand(
               async _ => await StartDownload(),
               _ => CanStartDownload()
            );

            BrowseExcelCommand = new RelayCommand(
                _ => BrowseExcel(),
                _ => CanBrowseExcel()
            );

            BrowseFolderCommand = new RelayCommand(
                _ => BrowseFolder(),
                _ => CanBrowseFolder()
            );
        }

        public async Task StartDownload()
        {
            await _startDownload();
        }

        private void BrowseExcel()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select Excel File",
                Filter = "Excel Files (*.xlsx;*.xls;*.xlsm)|*.xlsx;*.xls;*.xlsm",
                Multiselect = false
            };

            bool? result = dialog.ShowDialog();

            if ( result == true )
            {
                ExcelFilePath = dialog.FileName;
            }
        }

        private void BrowseFolder()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();

            bool? result = dialog.ShowDialog();

            if ( result == true )
            {
                OutputFolderPath = dialog.FolderName;
            }
        }

        private bool CanStartDownload()
        {
            if (State != DownloadState.Ready | ExcelFilePath == string.Empty | OutputFolderPath == string.Empty)
                return false;

            return true;
        }

        private bool CanBrowseExcel()
        {
            if (State != DownloadState.Ready)
                return false;

            return true;
        }

        private bool CanBrowseFolder()
        {
            if (State != DownloadState.Ready)
                return false;

            return true;
        }
    }
}
