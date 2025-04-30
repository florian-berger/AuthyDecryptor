using AuthyDecryptor.Model;

namespace AuthyDecryptor.UI.Wpf.BindingObjects
{
    public class DecryptedTokenBinding : BindableBase
    {
        #region Properties

        public DecryptedToken OriginalToken { get; }

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value, RegenerateQrCodeData);
        } private string? _name;

        public string? Issuer
        {
            get => _issuer;
            set => SetProperty(ref _issuer, value, RegenerateQrCodeData);
        } private string? _issuer;

        public string? DecryptedSeed
        {
            get => _decryptedSeed;
            set => SetProperty(ref _decryptedSeed, value, RegenerateQrCodeData);
        } private string? _decryptedSeed;

        public int Digits
        {
            get => _digits;
            set => SetProperty(ref _digits, value, RegenerateQrCodeData);
        } private int _digits;

        public string QrCodeData
        {
            get => _qrCodeData;
            set => SetProperty(ref _qrCodeData, value);
        } private string _qrCodeData = string.Empty;

        #endregion Properties

        #region Constructor

        public DecryptedTokenBinding(DecryptedToken originalToken)
        {
            OriginalToken = originalToken;

            _name = originalToken.Name;
            _issuer = originalToken.Issuer;
            _decryptedSeed = originalToken.DecryptedSeed;
            Digits = originalToken.Digits;
            
            RegenerateQrCodeData();
        }

        #endregion Constructor

        #region Public methods

        public DecryptedToken ToToken()
        {
            return new DecryptedToken
            {
                AccountType = OriginalToken.AccountType,
                DecryptedSeed = DecryptedSeed,
                Digits = Digits,
                Issuer = Issuer,
                Logo = OriginalToken.Logo,
                Name = Name,
                UniqueId = OriginalToken.UniqueId
            };
        }

        #endregion Public methods

        #region Private methods

        private void RegenerateQrCodeData()
        {
            QrCodeData = $"otpauth://totp/{Uri.EscapeDataString(Name ?? "")}" +
                         $"?secret={DecryptedSeed}&issuer={Uri.EscapeDataString(Issuer ?? "")}&digits={Digits}";
        }

        #endregion Private methods
    }
}
