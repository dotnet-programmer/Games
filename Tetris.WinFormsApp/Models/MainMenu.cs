using Tetris.WinForms.Enums;

namespace Tetris.WinForms.Models;

internal class MainMenu(Rectangle playfield)
{
	private Rectangle _playfield = playfield;

	public MainMenuState ActualState { get; private set; } = MainMenuState.NewGame;

	public void Next()
		=> ActualState = ActualState == MainMenuState.Exit ? MainMenuState.NewGame : ++ActualState;

	public void Previous()
		=> ActualState = ActualState == MainMenuState.NewGame ? MainMenuState.Exit : --ActualState;

	public void Draw(Graphics graphics)
	{
		graphics.FillRectangle(Brushes.Black, _playfield);
		DrawText(graphics);
	}

	private void DrawText(Graphics graphics)
	{
		int middlePosX = _playfield.Right / 2;
		int middlePosY = _playfield.Bottom / 2;
		int maxWidth = 0;

		using (Font font = new("Arial", 32, FontStyle.Bold))
		{
			string message = "Nowa gra";
			SizeF size = graphics.MeasureString(message, font);
			Point newGame = new(middlePosX - (int)size.Width / 2, middlePosY - 80);
			graphics.DrawString(message, font, Brushes.Yellow, newGame);
			maxWidth = (int)size.Width > maxWidth ? (int)size.Width : maxWidth;

			message = "Opcje";
			size = graphics.MeasureString(message, font);
			Point options = new(middlePosX - (int)size.Width / 2, middlePosY);
			graphics.DrawString(message, font, Brushes.Yellow, options);
			maxWidth = (int)size.Width > maxWidth ? (int)size.Width : maxWidth;

			message = "Zakończ grę";
			size = graphics.MeasureString(message, font);
			Point exit = new(middlePosX - (int)size.Width / 2, middlePosY + 80);
			graphics.DrawString(message, font, Brushes.Yellow, exit);
			maxWidth = (int)size.Width > maxWidth ? (int)size.Width : maxWidth;

			maxWidth += 20;

			switch (ActualState)
			{
				case MainMenuState.NewGame:
					graphics.DrawRectangle(new Pen(Brushes.Red, 5), middlePosX - maxWidth / 2, newGame.Y - 10, maxWidth, size.Height + 20);
					break;
				case MainMenuState.Options:
					graphics.DrawRectangle(new Pen(Brushes.Red, 5), middlePosX - maxWidth / 2, options.Y - 10, maxWidth, size.Height + 20);
					break;
				case MainMenuState.Exit:
					graphics.DrawRectangle(new Pen(Brushes.Red, 5), middlePosX - maxWidth / 2, exit.Y - 10, maxWidth, size.Height + 20);
					break;
				default:
					break;
			}
		}
	}
}