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

        // Excel Path
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

        // Output Folder Path
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

        // Starting Row 
        private int _startingRow;
        public int StartingRow => _startingRow;

        private bool _isStartingRowValid;
        public bool IsStartingRowValid
        {
            get => _isStartingRowValid;
            set => SetProperty(ref _isStartingRowValid, value);
        }

        private string _startingRowStr = string.Empty;
        public string StartingRowStr
        {
            get => _startingRowStr;
            set
            {
                if (SetProperty(ref _startingRowStr, value))
                {
                    ValidateStartingRow();

                    // Re-evaluate buttons
                    (StartDownloadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // BR Nummer
        private bool _isBrNummerValid;
        public bool IsBrNummerValid
        {
            get => _isBrNummerValid;
            set => SetProperty(ref _isBrNummerValid, value);
        }

        private string _brNummer = string.Empty;
        public string BrNummer
        {
            get => _brNummer;
            set
            {
                if (SetProperty(ref _brNummer, value))
                {
                    IsBrNummerValid = ValidateColumn(BrNummer);

                    // Re-evaluate buttons
                    (StartDownloadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Primary URL
        private bool _isPrimaryUrlValid;
        public bool IsPrimaryUrlValid
        {
            get => _isPrimaryUrlValid;
            set => SetProperty(ref _isPrimaryUrlValid, value);
        }

        private string _primaryUrl = string.Empty;
        public string PrimaryUrl
        {
            get => _primaryUrl;
            set
            {
                if (SetProperty(ref _primaryUrl, value))
                {
                    IsPrimaryUrlValid = ValidateColumn(PrimaryUrl);

                    // Re-evaluate buttons
                    (StartDownloadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Secondary URL
        private bool _isSecondaryUrlValid;
        public bool IsSecondaryUrlValid
        {
            get => _isSecondaryUrlValid;
            set => SetProperty(ref _isSecondaryUrlValid, value);
        }

        private string _secondaryUrl = string.Empty;
        public string SecondaryUrl
        {
            get => _secondaryUrl;
            set
            {
                if (SetProperty(ref _secondaryUrl, value))
                {
                    IsSecondaryUrlValid = ValidateColumn(SecondaryUrl);

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
            return State == DownloadState.Ready
                && !string.IsNullOrEmpty(ExcelFilePath)
                && !string.IsNullOrEmpty(OutputFolderPath)
                && IsStartingRowValid
                && IsBrNummerValid
                && IsPrimaryUrlValid
                && IsSecondaryUrlValid;
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

        private void ValidateStartingRow()
        {
            if (int.TryParse(StartingRowStr, out int newRow) && newRow >= 1)
            {
                _startingRow = newRow;
                IsStartingRowValid = true;
            }
            else
            {
                IsStartingRowValid = false;
            }
        }

        private bool ValidateColumn(string column)
        {
            if (!string.IsNullOrWhiteSpace(column) && column.All(char.IsLetter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
