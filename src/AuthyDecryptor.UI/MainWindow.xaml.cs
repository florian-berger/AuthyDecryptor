using System.Windows.Controls;

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
}