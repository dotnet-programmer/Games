using System.Windows;
using Snake.WpfApp.ViewModels;

namespace Snake.WpfApp.Views;

public partial class MainView : Window
{
	public MainView()
	{
		InitializeComponent();
		DataContext = new MainViewModel();
	}
}