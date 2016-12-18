using System;
using System.Globalization;
using Xamarin.Forms;

namespace Ta7meel
{
	public class IntToInverseBoolConverter:IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value == 0) ? true : false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? 0 : 1;
		}
	}
}
