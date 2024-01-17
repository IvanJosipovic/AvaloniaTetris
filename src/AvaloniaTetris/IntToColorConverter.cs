using Avalonia.Data.Converters;
using Avalonia.Data;
using System;
using System.Globalization;
using Avalonia.Media;

namespace AvaloniaTetris;

public class IntToColorConverter : IValueConverter
{
    public static readonly IntToColorConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int val)
        {
            switch (val)
            {
                case 0:
                    return Brushes.Black;
                case 1:
                    return Brushes.Red;
                case 2:
                    return Brushes.Blue;
                case 3:
                    return Brushes.Green;
                case 4:
                    return Brushes.Orange;
                case 5:
                    return Brushes.Purple;
            }
        }

        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
