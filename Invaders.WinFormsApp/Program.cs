using Invaders.WinFormsApp.Views;

namespace Invaders.WinFormsApp;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();
		Application.Run(new MainView());
	}
}