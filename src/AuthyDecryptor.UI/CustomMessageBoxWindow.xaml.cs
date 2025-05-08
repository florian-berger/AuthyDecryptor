using System.Windows;
using AuthyDecryptor.UI.Enums;
using AuthyDecryptor.UI.Resources;
using AuthyDecryptor.UI.Wpf;
using AuthyDecryptor.UI.Wpf.BindingObjects;

namespace AuthyDecryptor.UI;

/// <summary>
/// Interaction logic for CustomMessageBoxWindow.xaml
/// </summary>
public partial class CustomMessageBoxWindow
{
    #region Properties

    /// <summary>
    ///     Buttons that should be displayed
    /// </summary>
    public List<CustomMessageBoxButton> MessageBoxButtons { get; init; }

    /// <summary>
    ///     Title of the MessageBox
    /// </summary>
    public string MessageBoxTitle { get; init; }

    /// <summary>
    ///     Content of the MessageBox
    /// </summary>
    public string MessageBoxContent { get; init; }

    /// <summary>
    ///     Image that should be displayed
    /// </summary>
    public CustomMessageBoxImage MessageBoxIcon { get; init; }

    /// <summary>
    ///     Information if an icon should be displayed
    /// </summary>
    public bool MessageBoxShowIcon => MessageBoxIcon != CustomMessageBoxImage.None;

    /// <summary>
    ///     Result of the MessageBox
    /// </summary>
    public CustomMessageBoxResult Result { get; set; } = CustomMessageBoxResult.None;

    #endregion Properties

    #region Constructor

    /// <summary>
    ///     Should NEVER be called directly! Always use <see cref="CustomMessageBox" />
    /// </summary>
    internal CustomMessageBoxWindow(CustomMessageBoxButtons buttons, CustomMessageBoxImage icon, string title, string content, Window owner)
    {
        Owner = owner;

        MessageBoxButtons = BuildButtons(buttons);
        MessageBoxTitle = title;
        MessageBoxContent = content;
        MessageBoxIcon = icon;

        DataContext = this;
        InitializeComponent();
    }

    #endregion Constructor

    #region Private methods

    private List<CustomMessageBoxButton> BuildButtons(CustomMessageBoxButtons buttons)
    {
        switch (buttons)
        {
            case CustomMessageBoxButtons.Ok:
                return [BuildOkButton()];
            case CustomMessageBoxButtons.Cancel:
                return [BuildCancelButton()];
            case CustomMessageBoxButtons.OkCancel:
                return [BuildOkButton(), BuildCancelButton()];
            case CustomMessageBoxButtons.Yes:
                return [BuildYesButton()];
            case CustomMessageBoxButtons.YesNo:
                return [BuildYesButton(), BuildNoButton()];
            case CustomMessageBoxButtons.YesCancel:
                return [BuildYesButton(), BuildCancelButton()];
            case CustomMessageBoxButtons.YesNoCancel:
                return [BuildYesButton(false), BuildNoButton(false), BuildCancelButton(false)];
            default:
                throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
        }
    }

    private CustomMessageBoxButton BuildOkButton(bool isDefault = true)
    {
        return new CustomMessageBoxButton
        {
            BoxResult = CustomMessageBoxResult.Ok,
            Caption = AppResource.Ok,
            IsDefault = isDefault,
            IsCancel = false
        };
    }

    private CustomMessageBoxButton BuildCancelButton(bool isCancel = true)
    {
        return new CustomMessageBoxButton
        {
            BoxResult = CustomMessageBoxResult.Cancel,
            Caption = AppResource.Cancel,
            IsDefault = false,
            IsCancel = isCancel
        };
    }

    private CustomMessageBoxButton BuildYesButton(bool isDefault = true)
    {
        return new CustomMessageBoxButton
        {
            BoxResult = CustomMessageBoxResult.Yes,
            Caption = AppResource.Yes,
            IsDefault = isDefault,
            IsCancel = false
        };
    }

    private CustomMessageBoxButton BuildNoButton(bool isCancel = true)
    {
        return new CustomMessageBoxButton
        {
            BoxResult = CustomMessageBoxResult.No,
            Caption = AppResource.No,
            IsDefault = false,
            IsCancel = isCancel
        };
    }

    private void SetResult(CustomMessageBoxResult? result)
    {
        if (result != null)
        {
            Result = result.Value;
        }

        DialogResult = true;
    }

    #endregion Private methods

    #region Commands

    public DelegateCommand<CustomMessageBoxResult?> SetResultCommand => _setResultCommand ??= new DelegateCommand<CustomMessageBoxResult?>(SetResult);
    private DelegateCommand<CustomMessageBoxResult?>? _setResultCommand;

    #endregion Commands
}
