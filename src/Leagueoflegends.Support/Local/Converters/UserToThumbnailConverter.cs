using Leagueoflegends.Support.Local.Services;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Leagueoflegends.Support.Local.Converters;
internal class UserToThumbnailConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return $"{ImageManager.ImagePath}/thumb-{value}.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
