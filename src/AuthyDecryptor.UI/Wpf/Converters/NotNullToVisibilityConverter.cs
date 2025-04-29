using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace AuthyDecryptor.UI.Wpf.Converters;

/// <summary>
///     Converter that converts an object to <see cref="Visibility" /> based on these converters settings
/// </summary>
internal class NotNullToVisibilityConverter : IValueConverter
{
    #region Properties

    /// <summary>
    ///     True if the result should be inverted
    /// </summary>
    public bool Inverted { get; set; }

    /// <summary>
    ///     True if <see cref="Visibility.Hidden" /> should be returned instead of <see cref="Visibility.Collapsed"/>
    /// </summary>
    public bool HiddenInsteadOfCollapsed { get; set; }

    #endregion Properties

    #region IValueConverter

    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Inverted)
        {
            return value != null
                ? HiddenInsteadOfCollapsed ? Visibility.Hidden : Visibility.Collapsed
                : Visibility.Visible;
        }

        return value != null ?
            Visibility.Visible :
            HiddenInsteadOfCollapsed ? Visibility.Hidden : Visibility.Collapsed;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion IValueConverter
}