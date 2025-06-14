﻿using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using AuthyDecryptor.Model;
using AuthyDecryptor.UI.Enums;
using AuthyDecryptor.UI.Resources;
using AuthyDecryptor.UI.Wpf;
using Microsoft.Win32;

namespace AuthyDecryptor.UI.ViewModel;

public class MainViewModel : BindableBase
{
    #region Properties

    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (SetProperty(ref _isRunning, value))
            {
                UpdateCommandsCanExecute();
            }
        }
    } private bool _isRunning;

    public string EncryptedFilePath
    {
        get => _encryptedFilePath;
        set
        {
            if (SetProperty(ref _encryptedFilePath, value))
            {
                UpdateCommandsCanExecute();
            }
        }
    } private string _encryptedFilePath = string.Empty;

    public string DecryptedFileOutput
    {
        get => _decryptedFileOutput;
        set
        {
            if (SetProperty(ref _decryptedFileOutput, value))
            {
                UpdateCommandsCanExecute();
            }
        }
    } private string _decryptedFileOutput = string.Empty;

    public string AuthyBackupPassword
    {
        get => _authyBackupPassword;
        set
        {
            if (SetProperty(ref _authyBackupPassword, value))
            {
                UpdateCommandsCanExecute();
            }
        }
    } private string _authyBackupPassword = string.Empty;

    public bool AutoSaveToFile
    {
        get => _autoSaveToFile;
        set
        {
            if (SetProperty(ref _autoSaveToFile, value))
            {
                UpdateCommandsCanExecute();
            }
        }
    } private bool _autoSaveToFile;

    public int ProgressMax
    {
        get => _progressMax;
        set => SetProperty(ref _progressMax, value);
    } private int _progressMax;

    public int ProgressCurrent
    {
        get => _progressCurrent;
        set
        {
            if (SetProperty(ref _progressCurrent, value))
            {
                ProgressPercent = ProgressMax > 0 ? (int) ((double) _progressCurrent / ProgressMax * 100) : 0;
            }
        }
    } private int _progressCurrent;

    public int ProgressPercent
    {
        get => _progressPercent;
        set => SetProperty(ref _progressPercent, value);
    } private int _progressPercent;

    public string? NewVersionDownloadLink
    {
        get => _newVersionDownloadLink;
        set => SetProperty(ref _newVersionDownloadLink, value);
    } private string? _newVersionDownloadLink;

    public string WindowTitle { get; init; } = BuildWindowTitle();

    #endregion Properties

    #region Commands
    public DelegateCommand StartDecryptionCommand => _startDecryptionCommand ??= new DelegateCommand(StartDecryption, CanStartDecryption);
    private DelegateCommand? _startDecryptionCommand;

    public DelegateCommand SelectInputFileCommand => _selectInputFileCommand ??= new DelegateCommand(SelectInputFile);
    private DelegateCommand? _selectInputFileCommand;

    public DelegateCommand SelectOutputFileCommand => _selectOutputFileCommand ??= new DelegateCommand(SelectOutputFile);
    private DelegateCommand? _selectOutputFileCommand;

    public DelegateCommand LoadDecryptedFileCommand => _loadDecryptedFileCommand ??= new DelegateCommand(LoadDecryptedFile);
    private DelegateCommand? _loadDecryptedFileCommand;

    #endregion Commands

    #region Internal methods

    internal async Task CheckForVersionUpdate()
    {
        var currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        if (currentVersion == null)
        {
            return;
        }

        try
        {
            var newestVersion = await VersionHelper.CheckForUpdate(currentVersion);
            if (newestVersion != null)
            {
                UiThreadHelper.RunOnUiThread(() =>
                {
                    NewVersionDownloadLink = newestVersion.Value.ReleaseLink;
                    var versionString = VersionHelper.GetVersionString(newestVersion.Value.Version);

                    var result = CustomMessageBox.ShowDialog(
                        string.Format(AppResource.NewVersionAvailable, versionString),
                        AppResource.NewVersionTitle, CustomMessageBoxButtons.YesNo, CustomMessageBoxImage.Question);

                    if (result == CustomMessageBoxResult.Yes)
                    {
                        StaticCommands.OpenUriCommand.Execute(newestVersion.Value.ReleaseLink);
                    }
                });
            }
        }
        catch (Exception)
        {
            // Version check failed - do nothing
        }
    }

    #endregion Internal methods

    #region Private methods

    private async void StartDecryption()
    {
        if (!CanStartDecryption())
        {
            return;
        }

        IsRunning = true;
        try
        {
            var result = await Task.Run(RunDecryption);
            if (result.Count == 0)
            {
                return;
            }

            TokensListViewModel.ShowTokensList(result);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        finally
        {
            IsRunning = false;
        }
    }

    private bool CanStartDecryption()
    {
        var isValid =
            !IsRunning &&
            !string.IsNullOrWhiteSpace(EncryptedFilePath) &&
            !string.IsNullOrWhiteSpace(AuthyBackupPassword);

        if (!isValid)
        {
            return false;
        }

        if (AutoSaveToFile && string.IsNullOrWhiteSpace(DecryptedFileOutput))
        {
            return false;
        }

        return true;
    }

    private void SelectInputFile()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = $"{AppResource.JsonFile} (*.json)|*.json|{AppResource.AllFiles} (*.*)|*.*",
            Title = AppResource.InputFileTitle,
            ForcePreviewPane = true,
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == true)
        {
            EncryptedFilePath = openFileDialog.FileName;
        }
    }

    private void SelectOutputFile()
    {
        var defaultFileName = "decrypted_tokens.json";
        if (!string.IsNullOrWhiteSpace(DecryptedFileOutput))
        {
            try
            {
                defaultFileName = Path.GetFileName(DecryptedFileOutput);
            }
            catch
            {
                // Ignore errors when "GetFileName" failed
            }
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = $"{AppResource.JsonFile} (*.json)|*.json|{AppResource.AllFiles} (*.*)|*.*",
            Title = AppResource.OutputFileTitle,
            OverwritePrompt = true,
            AddExtension = true,
            FileName = defaultFileName
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            DecryptedFileOutput = saveFileDialog.FileName;
        }
    }

    private void LoadDecryptedFile()
    {
        try
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = $"{AppResource.JsonFile} (*.json)|*.json|{AppResource.AllFiles} (*.*)|*.*",
                Title = AppResource.SelectDecryptedFile,
                ForcePreviewPane = true,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var fileContent = File.ReadAllText(openFileDialog.FileName);
                var decryptedTokens = JsonSerializer.Deserialize<DecryptedToken[]>(fileContent) ?? [];
                
                TokensListViewModel.ShowTokensList(decryptedTokens.ToList());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format(AppResource.ErrorLoadDecryptedFile, ex.Message), AppResource.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateCommandsCanExecute()
    {
        StartDecryptionCommand.RaiseCanExecuteChanged();
    }

    private List<DecryptedToken> RunDecryption()
    {
        var autoSave = AutoSaveToFile;
        
        var fileContent = Decryptor.GetDataFromFile(EncryptedFilePath);
        if (fileContent == null || fileContent.AuthenticatorTokens.Count == 0)
        {
            UiThreadHelper.RunOnUiThread(() =>
            {
                ProgressMax = 0;
                ProgressCurrent = 0;
            });

            return [];
        }

        var decryptedTokens = new List<DecryptedToken>();

        UiThreadHelper.RunOnUiThread(() =>
        {
            ProgressMax = fileContent.AuthenticatorTokens.Count;
            ProgressCurrent = 0;
        });
        
        foreach (var token in fileContent.AuthenticatorTokens)
        {
            string result;
            if (string.IsNullOrWhiteSpace(token.EncryptedSeed))
            {
                result = "ERROR: No encrypted seed";
            }
            else if (string.IsNullOrWhiteSpace(token.Salt))
            {
                result = "ERROR: No salt";
            }
            else if (string.IsNullOrWhiteSpace(token.UniqueIv))
            {
                result = "ERROR: No IV";
            }
            else
            {
                try
                {
                    result = Decryptor.DecryptToken(token.KeyDerivationIterations, token.EncryptedSeed,
                        token.Salt, token.UniqueIv, AuthyBackupPassword);
                }
                catch (Exception ex)
                {
                    result = $"ERROR: {ex.Message}";
                }
            }

            decryptedTokens.Add(new DecryptedToken
            {
                AccountType = token.AccountType,
                Name = token.Name,
                Issuer = token.Issuer,
                DecryptedSeed = result,
                Digits = token.Digits,
                Logo = token.Logo,
                UniqueId = token.UniqueId
            });

            UiThreadHelper.RunOnUiThread(() =>
            {
                ProgressCurrent = decryptedTokens.Count;
            });
        }

        if (autoSave)
        {
            var serializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
            {
                WriteIndented = true
            };
            File.WriteAllText(DecryptedFileOutput, JsonSerializer.Serialize(decryptedTokens, serializerOptions), Encoding.UTF8);
        }

        return decryptedTokens;
    }

    private static string BuildWindowTitle()
    {
        var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        if (version == null)
        {
            return "Authy Decryptor";
        }

        return $"Authy Decryptor (v{VersionHelper.GetVersionString(version)})";
    }

    #endregion Private methods
}
