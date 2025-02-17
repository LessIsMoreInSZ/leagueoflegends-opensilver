using Leagueoflegends.Support.Local.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Leagueoflegends.Support.Local.Converters;
public class DiscountRateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int discountRate)
        {
            return $"-{discountRate}%";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ImageUrlConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return $"{ImageManager.ImagePath}{value}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}