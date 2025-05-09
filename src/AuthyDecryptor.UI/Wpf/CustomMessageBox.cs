using System.Windows;
using AuthyDecryptor.UI.Enums;
using AuthyDecryptor.UI.Resources;

namespace AuthyDecryptor.UI.Wpf;

public static class CustomMessageBox
{
    #region Public methods

    public static CustomMessageBoxResult ShowDialog(string message, string title, CustomMessageBoxButtons buttons, CustomMessageBoxImage icon, Window? owner = null)
    {
        var windowOwner = GetOwnerWindow(owner);

        var instance = new CustomMessageBoxWindow(buttons, icon, title, message, windowOwner);
        instance.ShowDialog();

        return instance.Result;
    }

    public static void ShowErrorDialog(Exception ex, Window? owner = null)
    {
        ShowErrorDialog(null, null, ex, owner);
    }

    public static void ShowErrorDialog(string? message, string? title, Exception ex, Window? owner = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            message = ex.ToString();
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            title = AppResource.Error;
        }

        ShowDialog(message, title, CustomMessageBoxButtons.Ok, CustomMessageBoxImage.Error, owner);
    }

    #endregion Public methods

    #region Private methods

    private static Window GetOwnerWindow(Window? window)
    {
        var resultWindow = window ?? Application.Current.MainWindow;
        if (resultWindow == null)
        {
            throw new InvalidOperationException("Can't find an owner window");
        }

        return resultWindow;
    }

    #endregion Private methods
}