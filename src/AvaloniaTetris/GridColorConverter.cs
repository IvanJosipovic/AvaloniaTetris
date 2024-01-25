using Avalonia.Data.Converters;
using Avalonia.Data;
using System;
using System.Globalization;
using Avalonia.Media;

namespace AvaloniaTetris;

public class GridColorConverter : IValueConverter
{
    public static readonly GridColorConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is GridFill val)
        {
            switch (val)
            {
                case GridFill.Blank:
                    return Brushes.Black;

                case GridFill.AsStraight:
                    return Brushes.Red;

                case GridFill.AsSquare:
                    return Brushes.Blue;

                case GridFill.AsL1:
                    return Brushes.Green;

                case GridFill.AsL2:
                    return Brushes.Cyan;

                case GridFill.AsT:
                    return Brushes.Purple;

                case GridFill.AsS1:
                    return Brushes.Yellow;

                case GridFill.AsS2:
                    return Brushes.PaleGoldenrod;

                case GridFill.Blink:
                    return Brushes.BlanchedAlmond;
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
