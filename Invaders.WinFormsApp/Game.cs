using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Invaders.WinFormsApp.Enemies;
using Invaders.WinFormsApp.Properties;
using Invaders.WinFormsApp.Star;

namespace Invaders.WinFormsApp;
internal class Game
{
	private int _score;
	private int _livesLeft;
	private int _wave;

	private DateTime start = DateTime.Now;
	private DateTime end;

	private readonly Random _random;
	private Rectangle _playfield;

	private readonly Stars _stars;

	private readonly PlayerShip _playerShip;
	private readonly List<Shot> _playerShots;
	private readonly Image _playerShipImage = ImageUtil.ResizeImage(Resources.player_ship, 60, 50);

	private List<Invader> _invaders;
	private List<Shot> _invaderShots;
	private MoveDirection _invaderDirection;

	//public event GameOverDelegate GameOver;
	public event Action GameOver;
	public bool _isGameOver;

	public Game(Rectangle playfield)
	{
		_playfield = playfield;
		_stars = new Stars(300, playfield);
		_random = new();
		_livesLeft = 2;
		_score = 0;
		_wave = 1;
		_isGameOver = false;

		_playerShip = new(playfield);
		_playerShots = new();

		MakeInvaders();
	}

	private void MakeInvaders()
	{
		_invaders = new List<Invader>(30);
		int x = _playfield.Width / 2 - 220;
		int y = 40;
		for (int i = 5; i > 0; i--)
		{
			for (int j = 0; j < 6; j++)
			{
				Invader invader = new Invader((InvaderType)i, new Point(x + j * 80, y), i * 10);
				_invaders.Add(invader);
			}
			y += 60;
		}

		_invaderDirection = (MoveDirection)_random.Next(2);
		_invaderShots = new();
	}

	#region drawing region

	public void Draw(Graphics graphics, int animationCell, bool isPaused)
	{
		// czarny prostokąt
		graphics.FillRectangle(Brushes.Black, _playfield);

		// gwiazdy
		_stars.Draw(graphics, animationCell);

		// najeźdźcy 
		_invaders.ForEach(i => i.Draw(graphics, animationCell));

		// statek gracza
		_playerShip.Draw(graphics);

		// pociski
		_playerShots.ForEach(ps => ps.Draw(graphics, Brushes.DodgerBlue));
		_invaderShots.ForEach(ps => ps.Draw(graphics, Brushes.Red));

		// zdobyte punkty
		DrawScore(graphics);

		// numer fali
		DrawWave(graphics);

		// liczba żyć
		DrawRemainingLife(graphics);

		// liczba klatek grafiki / s
		DrawFrameRate(graphics);

		// Pauza
		if (isPaused)
		{
			DrawPause(graphics);
		}

		// "GAME OVER"
		if (_isGameOver)
		{
			DrawGameOver(graphics);
		}
	}

	private void DrawScore(Graphics graphics)
	{
		using (Font font = new("Arial", 18))
		{
			graphics.DrawString($"Wynik: {_score}", font, Brushes.Yellow, new Point(_playfield.Left + 5, _playfield.Top + 5));
		}
	}

	private void DrawWave(Graphics graphics)
	{
		using (Font font = new("Arial", 18))
		{
			string waveMsg = "Fala:";
			SizeF size = graphics.MeasureString(waveMsg, font);
			graphics.DrawString($"{waveMsg} {_wave}", font, Brushes.Yellow, new Point(_playfield.Left + 5, _playfield.Top + (int)size.Height + 5));
		}
	}

	private void DrawRemainingLife(Graphics graphics)
	{
		for (int i = 0; i < _livesLeft; i++)
		{
			graphics.DrawImageUnscaled(_playerShipImage, (_playfield.Width - 5) - (_playerShipImage.Width + 10) * i - _playerShipImage.Width, _playfield.Y + 10);
		}
	}

	private void DrawFrameRate(Graphics graphics)
	{
		end = DateTime.Now;
		TimeSpan frameDuration = end - start;
		start = end;

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
			graphics.DrawString(message, font, Brushes.Red, new Point(_playfield.Right / 2 - (int)sizePause.Width / 2, _playfield.Bottom / 2 - 30 - (int)sizePause.Height / 2));
		}
	}

	private void DrawGameOver(Graphics graphics)
	{
		int pointX = _playfield.Right / 2;
		int pointY = _playfield.Bottom / 2 - 30;

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

	// zegar gry formularza wywołuje metodę Go obiektu Game
	public void Go()
	{
		// przesunięcie pocisków
		MoveShots(_playerShots);

		// wykonanie ruchu każdego invadersa i oddanie ich strzałów
		MoveInvaders();
		InvaderFire();
		MoveShots(_invaderShots);

		// sprawdzanie zdarzeń:
		CheckForInvaderCollisions();
		CheckForPlayerCollisions();
	}

	// tworzy następny nalot najeźdźców
	private void NextWave()
	{
		_wave++;
		_playerShots.Clear();
		_invaderShots.Clear();
		MakeInvaders();
	}

	// przesuwa statek gracza
	public void MovePlayer(MoveDirection direction) => _playerShip.Move(direction);

	// strzela pociskami gracza
	public void FireShot()
	{
		if (_playerShots.Count < 5)
		{
			_playerShots.Add(new Shot(_playerShip.ShotLocation, MoveDirection.Up, _playfield));
		}
	}

	// porusza pociskami
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

	// sprawdza, czy gracz został trafiony
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
			GameOver.Invoke();
		}
	}

	// sprawdza czy najeźdźca został trafiony
	private void CheckForInvaderCollisions()
	{
		// usuń najeźdźcę i strzał
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

		// następna fala jeśli wszyscy invadersi zginęli
		if (_invaders.Count == 0)
		{
			NextWave();
		}

		// sprawdzenie czy najeźdźca nie osiągnął dolnej krawędzi ekranu - jeśli tak zakończ grę
		if (_invaders.Max(x => x.Location.Y) > _playfield.Bottom - 120)
		{
			_playerShip.IsAlive = false;
			GameOver.Invoke();
		}
	}

	// porusza całą grupą najeźdźców
	private void MoveInvaders()
	{
		_invaders.ForEach(i => i.Move(_invaderDirection));

		if (_invaderDirection == MoveDirection.Left)
		{
			if (_invaders.Min(x => x.Location.X) < 20)
			{
				_invaders.ForEach(i => i.Move(MoveDirection.Down));
				_invaderDirection = MoveDirection.Right;
			}
		}
		else
		{
			if (_invaders.Max(x => x.Location.X) > _playfield.Right - 60)
			{
				_invaders.ForEach(i => i.Move(MoveDirection.Down));
				_invaderDirection = MoveDirection.Left;
			}
		}
	}

	// strzela pociskami invadersa
	private void InvaderFire()
	{
		if (_invaderShots.Count < _wave + 2 && _random.Next(10) < 5)
		{
			Invader invader = _invaders[_random.Next(_invaders.Count)];
			_invaderShots.Add(new Shot(invader.ShotLocation, MoveDirection.Down, _playfield));
		}
	}
}
