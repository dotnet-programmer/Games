using System.Windows;
using Tetris.WpfApp.ViewModels;

namespace Tetris.WpfApp.Views;

public partial class MainView : Window
{
	public MainView()
	{
		InitializeComponent();
		DataContext = new MainViewModel();
	}
}