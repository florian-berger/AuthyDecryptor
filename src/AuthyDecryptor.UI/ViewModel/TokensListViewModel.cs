using System.IO;
using AuthyDecryptor.Model;
using AuthyDecryptor.UI.Wpf.BindingObjects;
using System.Text.Json;
using AuthyDecryptor.UI.Resources;
using Microsoft.Win32;

namespace AuthyDecryptor.UI.ViewModel;

internal class TokensListViewModel(DecryptedTokenBinding[] tokens) : BindableBase
{
    #region Variables

    private string? _lastFilePath;

    #endregion Variables
    
    #region Properties

    public DecryptedTokenBinding[] Tokens { get; init; } = tokens;

    public DecryptedTokenBinding? SelectedToken
    {
        get => _selectedToken;
        set => SetProperty(ref _selectedToken, value);
    } private DecryptedTokenBinding? _selectedToken;

    public bool AllowEditing
    {
        get => _allowEditing;
        set => SetProperty(ref _allowEditing, value);
    } private bool _allowEditing;

    public bool SaveIndented
    {
        get => _saveIndented;
        set => SetProperty(ref _saveIndented, value);
    } private bool _saveIndented;

    #endregion Properties

    #region Commands

    public DelegateCommand SaveToFileCommand => _saveToFileCommand ??= new DelegateCommand(SaveToFile);
    private DelegateCommand? _saveToFileCommand;

    #endregion Commands

    #region Private methods

    private void SaveToFile()
    {
        var defaultFileName = "decrypted_tokens.json";
        var defaultFilePath = string.Empty;

        if (!string.IsNullOrWhiteSpace(_lastFilePath))
        {
            try
            {
                defaultFileName = Path.GetFileName(_lastFilePath);
                defaultFilePath = Path.GetDirectoryName(_lastFilePath);
            }
            catch
            {
                // Ignore here
            }
        }
        
        var saveFileDialog = new SaveFileDialog
        {
            Filter = $"{AppResource.JsonFile} (*.json)|*.json|{AppResource.AllFiles} (*.*)|*.*",
            Title = AppResource.OutputFileTitle,
            OverwritePrompt = true,
            AddExtension = true,
            FileName = defaultFileName,
            InitialDirectory = defaultFilePath
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            _lastFilePath = saveFileDialog.FileName;
            
            var tokensToSave = Tokens.Select(b => b.ToToken()).ToList();
            var options = new JsonSerializerOptions
            {
                WriteIndented = SaveIndented
            };

            var json = System.Text.Json.JsonSerializer.Serialize(tokensToSave, options);
            File.WriteAllText(_lastFilePath, json);
        }
    }

    #endregion Private methods

    #region Static helper methods

    internal static void ShowTokensList(List<DecryptedToken> decryptedTokens)
    {
        new TokensListWindow(decryptedTokens.Select(t => new DecryptedTokenBinding(t)).OrderBy(r => r.Name).ToArray()).Show();
    }

    #endregion Static helper methods
}
