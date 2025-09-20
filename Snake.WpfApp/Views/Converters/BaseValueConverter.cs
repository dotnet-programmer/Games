using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Snake.WpfApp.Views.Converters;

/// <summary>
/// Base value converter to reduce rewriting a bunch of base code
/// </summary>
/// <typeparam name="T">Converter class</typeparam>
internal abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter where T : class, new()
{
	private static T? _converter;

	#region MarkupExtension Members

	/// <summary>
	/// Makes the converter available to use in xaml
	/// </summary>
	/// <param name="serviceProvider"></param>
	/// <returns></returns>
	public override object ProvideValue(IServiceProvider serviceProvider)
		=> _converter ?? (_converter = new T());

	#endregion MarkupExtension Members

	#region IValueConverter Members

	/// <summary>
	/// The convert method to override
	/// </summary>
	/// <param name="value">The value to convert</param>
	/// <param name="targetType"></param>
	/// <param name="parameter"></param>
	/// <param name="culture"></param>
	/// <returns></returns>
	public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

	public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

	#endregion IValueConverter Members
}