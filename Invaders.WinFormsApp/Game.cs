using Invaders.WinFormsApp.Enemies;
using Invaders.WinFormsApp.Direction;
using Invaders.WinFormsApp.Star;

namespace Invaders.WinFormsApp;

internal class Game
{
	private const int NumberOfStars = 300;
	private const int PlayerLives = 3;
	private const int InvadersCount = 30;
	private const int StartingWave = 1;
	private const int StartingDirectionsCount = 2;
	private const int MaxPlayerShotsCount = 5;

	private readonly Random _random = new();
	private readonly Stars _stars;
	private readonly PlayerShip _playerShip;
	private readonly List<Shot> _playerShots = [];
	private readonly List<Shot> _invaderShots = [];

	private int _score;
	private int _livesLeft = PlayerLives;
	private int _wave = StartingWave;
	private DateTime _start = DateTime.Now;
	private DateTime _end;
	private Rectangle _playfield;
	private List<Invader> _invaders = null!;
	private MoveDirection _invadersDirection;

	public event EventHandler? GameOver;
	public bool IsGameOver;

	public Game(Rectangle playfield)
	{
		_playfield = playfield;
		_stars = new(NumberOfStars, playfield);
		_playerShip = new(playfield);
		MakeInvaders();
		_invadersDirection = (MoveDirection)_random.Next(StartingDirectionsCount);
	}

	// The game clock of the form calls the GoNextFrame method of the Game object
	public void GoNextFrame()
	{
		MoveShots(_playerShots);
		MoveInvaders();
		RandomInvaderShot();
		MoveShots(_invaderShots);
		CheckForInvaderCollisions();
		CheckForPlayerCollisions();
	}

	public void MovePlayer(MoveDirection direction)
		=> _playerShip.Move(direction);

	public void FireShot()
	{
		if (_playerShots.Count < MaxPlayerShotsCount)
		{
			_playerShots.Add(new Shot(_playerShip.ShotLocation, MoveDirection.Up, _playfield));
		}
	}

	#region drawing region

	public void Draw(Graphics graphics, int animationCell, bool isPaused)
	{
		// Background
		graphics.FillRectangle(Brushes.Black, _playfield);

		_stars.Draw(graphics, animationCell);
		_invaders.ForEach(i => i.Draw(graphics, animationCell));
		_playerShip.Draw(graphics);

		_playerShots.ForEach(s => s.Draw(graphics));
		_invaderShots.ForEach(s => s.Draw(graphics));

		DrawScore(graphics);
		DrawWave(graphics);
		DrawRemainingLife(graphics);
		DrawFrameRate(graphics);
		
		if (isPaused)
		{
			DrawPause(graphics);
		}

		if (IsGameOver)
		{
			DrawGameOver(graphics);
		}
	}

	private void DrawScore(Graphics graphics)
	{
		int distanceFromEdge = 5;
		using (Font font = new("Arial", 18))
		{
			graphics.DrawString($"Wynik: {_score}", font, Brushes.Yellow, new Point(_playfield.Left + distanceFromEdge, _playfield.Top + distanceFromEdge));
		}
	}

	private void DrawWave(Graphics graphics)
	{
		int distanceFromEdge = 5;
		using (Font font = new("Arial", 18))
		{
			string waveMsg = "Fala:";
			SizeF size = graphics.MeasureString(waveMsg, font);
			graphics.DrawString($"{waveMsg} {_wave}", font, Brushes.Yellow, new Point(_playfield.Left + distanceFromEdge, _playfield.Top + (int)size.Height + distanceFromEdge));
		}
	}

	private void DrawRemainingLife(Graphics graphics)
	{
		int distanceFromEdge = 5;
		Image playerShipImage = _playerShip.Image;
		for (int i = 0; i < _livesLeft; i++)
		{
			graphics.DrawImageUnscaled(playerShipImage, (_playfield.Width - distanceFromEdge) - (playerShipImage.Width + distanceFromEdge) * i - playerShipImage.Width, _playfield.Y + distanceFromEdge);
		}
	}

	private void DrawFrameRate(Graphics graphics)
	{
		_end = DateTime.Now;
		TimeSpan frameDuration = _end - _start;
		_start = _end;

		double milliSeconds = frameDuration.TotalMilliseconds;
		string message = milliSeconds != 0.0 ?
			$"{1000 / milliSeconds:f0} fps  ({milliSeconds:f1} ms)" :
			"0";

		using (Font font = new("Arial", 7))
		{
			SizeF size = graphics.MeasureString(message, font);
			graphics.DrawString(message, font, Brushes.LightGray, new Point(_playfield.Right - (int)size.Width, _playfield.Top));
		}
	}

	private void DrawPause(Graphics graphics)
	{
		string message = "PAUSE";
		using (Font font = new("Arial", 64, FontStyle.Bold))
		{
			SizeF sizePause = graphics.MeasureString(message, font);
			graphics.DrawString(message, font, Brushes.Red, new Point(_playfield.Right / 2 - (int)sizePause.Width / 2, _playfield.Bottom / 2 - (int)sizePause.Height / 2));
		}
	}

	private void DrawGameOver(Graphics graphics)
	{
		int pointX = _playfield.Right / 2;
		int pointY = _playfield.Bottom / 2;

		SizeF sizeGameOver;
		Rectangle background;
		string message = "GAME OVER";
		using (Font font = new("Arial", 64, FontStyle.Bold))
		{
			sizeGameOver = graphics.MeasureString(message, font);
			background = new Rectangle(pointX - (int)sizeGameOver.Width / 2, pointY - (int)sizeGameOver.Height / 2, (int)sizeGameOver.Width + 1, (int)sizeGameOver.Height);
			graphics.FillRectangle(Brushes.Black, background);
			graphics.DrawString(message, font, Brushes.Red, background);
		}

		SizeF sizeNewGame;
		message = "S - nowa gra";
		using (Font font = new("Arial", 18))
		{
			sizeNewGame = graphics.MeasureString(message, font);
			background = new Rectangle(pointX - (int)sizeNewGame.Width / 2, pointY + (int)sizeGameOver.Height / 2, (int)sizeNewGame.Width + 1, (int)sizeNewGame.Height);
			graphics.FillRectangle(Brushes.Black, background);
			graphics.DrawString(message, font, Brushes.Yellow, background);
		}

		message = "ESC - wyjście";
		using (Font font = new("Arial", 18))
		{
			SizeF sizeEsc = graphics.MeasureString(message, font);
			background = new Rectangle(pointX - (int)sizeEsc.Width / 2, pointY + (int)(sizeGameOver.Height / 2) + (int)sizeNewGame.Height + (int)sizeEsc.Height / 2, (int)sizeEsc.Width + 1, (int)sizeEsc.Height);
			graphics.FillRectangle(Brushes.Black, background);
			graphics.DrawString(message, font, Brushes.Yellow, background);
		}
	}

	#endregion

	private void MakeInvaders()
	{
		_invaders = new(InvadersCount);

		int x = _playfield.Width / 2 - 220;
		int y = 40;
		int verticalOffset = 60;
		int horizontallOffset = 80;
		int invaderTypesCount = Enum.GetNames<InvaderType>().Length;
		int invadersInRow = 6;

		for (int i = invaderTypesCount; i > 0; i--)
		{
			for (int j = 0; j < invadersInRow; j++)
			{
				_invaders.Add(new Invader((InvaderType)i, new Point(x + j * horizontallOffset, y), i * 10));
			}
			y += verticalOffset;
		}
	}

	private void NextInvadersWave()
	{
		_wave++;
		_playerShots.Clear();
		_invaderShots.Clear();
		MakeInvaders();
	}

	private void MoveShots(List<Shot> shots)
	{
		for (int i = shots.Count - 1; i >= 0; i--)
		{
			if (!shots[i].Move())
			{
				shots.RemoveAt(i);
			}
		}
	}

	private void CheckForPlayerCollisions()
	{
		for (int i = _invaderShots.Count - 1; i >= 0; i--)
		{
			if (_playerShip.Area.Contains(_invaderShots[i].Location))
			{
				_invaderShots.RemoveAt(i);
				_livesLeft--;
			}
		}

		if (_livesLeft < 0)
		{
			_playerShip.IsAlive = false;
			OnGameOver(EventArgs.Empty);
		}
	}

	private void CheckForInvaderCollisions()
	{
		// Remove the invader and the shot that hit him
		for (int i = _invaders.Count - 1; i >= 0; i--)
		{
			for (int j = _playerShots.Count - 1; j >= 0; j--)
			{
				if (_invaders[i].Area.Contains(_playerShots[j].Location))
				{
					_score += _invaders[i].Score;
					_invaders.RemoveAt(i);
					i--;
					if (i < 0)
					{
						break;
					}
					_playerShots.RemoveAt(j);
					j--;
				}
			}
		}

		// Next wave if all invaders killed
		if (_invaders.Count == 0)
		{
			NextInvadersWave();
		}

		// Check if the invader has reached the bottom edge of the screen - if so, end the game
		if (_invaders.Max(x => x.Location.Y) > _playfield.Bottom - 120)
		{
			_playerShip.IsAlive = false;
			OnGameOver(EventArgs.Empty);
		}
	}

	private void OnGameOver(EventArgs e)
		=> GameOver?.Invoke(this, e);

	private void MoveInvaders()
	{
		_invaders.ForEach(i => i.Move(_invadersDirection));

		if (_invadersDirection == MoveDirection.Left)
		{
			if (_invaders.Min(x => x.Location.X) < 20)
			{
				_invaders.ForEach(i => i.Move(MoveDirection.Down));
				_invadersDirection = MoveDirection.Right;
			}
		}
		else
		{
			if (_invaders.Max(x => x.Location.X) > _playfield.Right - 60)
			{
				_invaders.ForEach(i => i.Move(MoveDirection.Down));
				_invadersDirection = MoveDirection.Left;
			}
		}
	}

	private void RandomInvaderShot()
	{
		// With each wave there can be more active missiles at once
		// In addition, a 50% chance of a shot, so that not every shot appears immediately after the previous one disappears
		if (_invaderShots.Count < _wave + 2 && _random.Next(10) < 5)
		{
			Invader invader = _invaders[_random.Next(_invaders.Count)];
			_invaderShots.Add(new Shot(invader.ShotLocation, MoveDirection.Down, _playfield));
		}
	}
}