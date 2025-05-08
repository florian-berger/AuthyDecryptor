using System.Windows.Controls;
using AuthyDecryptor.UI.ViewModel;

namespace AuthyDecryptor.UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void WindowRibbon_OnRibbonContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        e.Handled = true;
    }

    private void MainWindow_OnContentRendered(object? sender, EventArgs e)
    {
        (DataContext as MainViewModel)?.CheckForVersionUpdate();
    }
}