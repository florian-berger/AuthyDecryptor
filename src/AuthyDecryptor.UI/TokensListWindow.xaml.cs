using System.Windows;
using System.Windows.Controls;
using AuthyDecryptor.UI.ViewModel;
using AuthyDecryptor.UI.Wpf.BindingObjects;

namespace AuthyDecryptor.UI
{
    /// <summary>
    /// Interaction logic for TokensListWindow.xaml
    /// </summary>
    public partial class TokensListWindow
    {
        public TokensListWindow(DecryptedTokenBinding[] tokens)
        {
            Owner = Application.Current.MainWindow;
            DataContext = new TokensListViewModel(tokens);

            InitializeComponent();
        }

        private void WindowRibbon_OnRibbonContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }
    }
}
