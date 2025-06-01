using System.Globalization;
using System.Windows.Data;

namespace WeBoard.Client.WPF.Helpers;

public class BoolToFavoriteTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b && b)
            return "Remove from Favorites";
        else
            return "Add to Favorites";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}