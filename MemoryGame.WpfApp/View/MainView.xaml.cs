using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MemoryGame.WpfApp.ViewModels;

namespace MemoryGame.WpfApp.View;

/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
	public MainView()
	{
		InitializeComponent();
		DataContext = new MainViewModel(MainGrid.Children.OfType<Button>());
	}
}