using Leagueoflegends.Collection.Local.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Leagueoflegends.Settings.Local.Converters
{
    public class InputModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is InputMode selectedMode && parameter is string targetModeString)
            {
                // OpenSilver에서의 Enum.Parse 사용
                InputMode targetMode = (InputMode)Enum.Parse(typeof(InputMode), targetModeString);
                return selectedMode.Equals(targetMode) ? "Visible" : "Collapsed";
            }
            return "Collapsed";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}