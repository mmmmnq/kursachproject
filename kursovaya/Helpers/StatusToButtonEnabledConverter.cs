using System;
using System.Globalization;
using System.Windows.Data;

namespace kursovaya.Helpers
{
    public class StatusToButtonEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status != "Отменен" && status != "Завершен";
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}