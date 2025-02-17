using Leagueoflegends.Support.Local.Services;
using System;
using System.Globalization;
using System.Windows.Data;
namespace Leagueoflegends.Support.Local.Converters;

public class ConceptToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string concept)
        {
            return $"{ImageManager.ImagePath}/Concepts/{concept}.png";
        }
        return $"{ImageManager.ImagePath}/Concepts/warrior.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
