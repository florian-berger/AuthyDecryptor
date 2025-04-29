using AuthyDecryptor.Model;

namespace AuthyDecryptor.UI.ViewModel;

internal class TokensListViewModel : BindableBase
{
    #region Properties

    public DecryptedToken[] Tokens { get; private set; }

    public DecryptedToken? SelectedToken
    {
        get => _selectedToken;
        set
        {
            if (SetProperty(ref _selectedToken, value) && _selectedToken != null)
            {
                GenerateQrCodeDataIfNecessary(_selectedToken);
            }
        }
    } private DecryptedToken? _selectedToken;

    public string QrCodeDisplayData
    {
        get => _qrCodeDisplayData;
        set => SetProperty(ref _qrCodeDisplayData, value);
    } private string _qrCodeDisplayData = string.Empty;

    #endregion Properties

    #region Constructor

    public TokensListViewModel(DecryptedToken[] tokens)
    {
        Tokens = tokens;
    }

    #endregion Constructor

    #region Private methods

    private void GenerateQrCodeDataIfNecessary(DecryptedToken token)
    {
        if (string.IsNullOrEmpty(token.QrCodeData))
        {
            token.QrCodeData = $"otpauth://totp/{Uri.EscapeDataString(token.Name ?? "")}" +
                            $"?secret={token.DecryptedSeed}&issuer={Uri.EscapeDataString(token.Issuer ?? "")}&digits={token.Digits}";
        }
        
        QrCodeDisplayData = token.QrCodeData;
    }

    #endregion Private methods

    #region Static helper methods

    internal static void ShowTokensList(List<DecryptedToken> decryptedTokens)
    {
        new TokensListWindow(decryptedTokens.OrderBy(r => r.Name).ToArray()).Show();
    }

    #endregion Static helper methods
}
