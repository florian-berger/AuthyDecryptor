using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace AuthyDecryptor.UI.Wpf.Converters;

internal class BoolVisibilityConverter : IValueConverter
{
    #region Properties

    /// <summary>
    /// Set to true if you want your item Hidden instead of Collapsed
    /// </summary>
    public bool HiddenInsteadOfCollapsed { get; set; } = false;

    /// <summary>
    /// Set to true if you want to inverse the converter
    /// </summary>
    public bool Inverse { get; set; } = false;

    #endregion Properties

    #region IValueConverter

    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolVal)
        {
            throw new ArgumentException("Parameter must be a boolean to be used with this converter.", nameof(value));
        }

        if (Inverse)
        {
            return boolVal
                ? (HiddenInsteadOfCollapsed ? Visibility.Hidden : Visibility.Collapsed)
                : Visibility.Visible;
        }

        return boolVal
            ? Visibility.Visible
            : (HiddenInsteadOfCollapsed ? Visibility.Hidden : Visibility.Collapsed);
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Visibility visibilityVal)
        {
            throw new ArgumentException("Parameter must be of type Visibility to be able to be converted back.", nameof(value));
        }

        if (Inverse)
        {
            return visibilityVal != Visibility.Visible;
        }

        return visibilityVal == Visibility.Visible;
    }

    #endregion IValueConverter
}
