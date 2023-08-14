using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Snake.WpfApp.Views.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
internal class BoolToVisibilityConverter : BaseValueConverter<BoolToVisibilityConverter>
{
	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> (bool)value ? Visibility.Visible : Visibility.Collapsed;

	public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> (Visibility)value == Visibility.Visible;
}