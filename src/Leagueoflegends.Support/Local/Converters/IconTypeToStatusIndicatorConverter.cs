using Leagueoflegends.Support.Local.Services;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Leagueoflegends.Support.Local.Converters;

public class IconTypeToStatusIndicatorConverter : IValueConverter
{
    private static string RpPath = $"{ImageManager.ImagePath}/rp.png";
    private static string BlueEssencePath = $"{ImageManager.ImagePath}/rp.pngng";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string iconType)
        {
            return iconType.ToLower() switch
            {
                "rp" => RpPath,
                "blueessence" => BlueEssencePath,
                _ => string.Empty
            };
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
