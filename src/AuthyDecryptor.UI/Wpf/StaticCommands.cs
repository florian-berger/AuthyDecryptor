using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using AuthyDecryptor.UI.Resources;

namespace AuthyDecryptor.UI.Wpf;
internal class StaticCommands
{
    #region Commands

    /// <summary>
    ///     Command for opening a URI
    /// </summary>
    public static DelegateCommand<string> OpenUriCommand => _openUriCommand ??= new DelegateCommand<string>(OpenUri);
    private static DelegateCommand<string>? _openUriCommand;

    public static DelegateCommand OpenThirdPartyLicensesCommand => _openThirdPartyLicensesCommand ??= new DelegateCommand(OpenThirdPartyLicenses);
    private static DelegateCommand? _openThirdPartyLicensesCommand;

    #endregion Commands

    #region Private methods

    private static void OpenUri(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo(address) { UseShellExecute = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", address);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", address);
        }
    }

    private static void OpenThirdPartyLicenses()
    {
        var thirdPartyLicenseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Third-Party Licenses.html");
        if (File.Exists(thirdPartyLicenseFile))
        {
            Process.Start(new ProcessStartInfo(thirdPartyLicenseFile) { UseShellExecute = true });
        }
        else
        {
            MessageBox.Show(AppResource.ErrorOpeningLicensesFile);
        }
    }

    #endregion Private methods
}

