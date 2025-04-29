using System.Windows;
using AuthyDecryptor.Model;
using AuthyDecryptor.UI.ViewModel;

namespace AuthyDecryptor.UI
{
    /// <summary>
    /// Interaction logic for TokensListWindow.xaml
    /// </summary>
    public partial class TokensListWindow
    {
        public TokensListWindow(DecryptedToken[] tokens)
        {
            Owner = Application.Current.MainWindow;
            DataContext = new TokensListViewModel(tokens);

            InitializeComponent();
        }
    }
}
