using System;
using System.Globalization;
using System.Windows.Data;

namespace PL.Converter;

internal class isEnibel : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string Text = (value as string)!;
        return !string.IsNullOrWhiteSpace(Text);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


