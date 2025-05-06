using System.Globalization;
using System.Windows.Data;

namespace Robot_Garage.Convertors
{
    public class BoolToAvailabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool availability)
            {
                return availability ? "Available" : "Not Available";
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not supported in BoolToAvailabilityConverter.");
        }
    }
}
