using System.Globalization;
using System.Windows;
using AuthyDecryptor.UI.Wpf;

namespace AuthyDecryptor.UI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
#if DEBUG
        var licenseKey = Environment.GetEnvironmentVariable("AuthyDecryptor_SyncFusion_License", EnvironmentVariableTarget.User);
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);
#else
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("%LICENSE_KEY%");
#endif
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentUICulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentUICulture;
        
        Current.Dispatcher.Invoke(UiThreadHelper.Initialize);
    }
}
