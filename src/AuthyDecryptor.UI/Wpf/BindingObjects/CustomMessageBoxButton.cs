using AuthyDecryptor.UI.Enums;

namespace AuthyDecryptor.UI.Wpf.BindingObjects;

/// <summary>
///     A custom button of the MessageBox
/// </summary>
public class CustomMessageBoxButton
{
    /// <summary>
    ///     Caption of the button
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    ///     True if Enter should execute this action
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    ///     True if Escape should execute this action
    /// </summary>
    public bool IsCancel { get; set; }

    /// <summary>
    ///     Result the button should set
    /// </summary>
    public CustomMessageBoxResult BoxResult { get; set; }
}