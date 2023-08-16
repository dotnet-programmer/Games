using System;
using System.Globalization;
using System.Windows.Data;

namespace Snake.WpfApp.Views.Converters;

[ValueConversion(typeof(int), typeof(int))]
internal class SoundValueConverter : BaseValueConverter<SoundValueConverter>
{
	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> (int)value * 10;

	public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> (int)value / 10;
}