using Leagueoflegends.Support.Local.Services;
using System;
using System.Globalization;
using System.Windows.Data;
namespace Leagueoflegends.Support.Local.Converters;

public class PercentToBadgeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int percent)
        {
            return percent >= 50
                ? $"{ImageManager.ImagePath}/badge_champion2.png"
                : $"{ImageManager.ImagePath}/badge_champion1.png";
        }
        return $"{ImageManager.ImagePath}/badge_champion1.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
