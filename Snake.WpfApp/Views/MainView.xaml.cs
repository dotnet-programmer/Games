using System.Windows;
using Snake.WpfApp.ViewModels;

namespace Snake.WpfApp.Views;

/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
	public MainView()
	{
		InitializeComponent();
		DataContext = new MainViewModel();
	}
}