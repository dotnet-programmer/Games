namespace Invaders.WinFormsApp;

public partial class Main : Form
{
	private readonly List<Keys> _keysPressed = new();
	private bool _isGameOver = false;
	private bool _isPaused = false;
	private int _frame = 0;
	private int _animationCell = 0;
	private Game _game;

	public Main()
	{
		InitializeComponent();
		StartGame();
	}

	private void StartGame()
	{
		_game = new Game(this.ClientRectangle);
		_game.GameOver += Game_GameOver;

		AnimationTimer.Start();
		GameTimer.Start();

		_isPaused = false;
		_isGameOver = false;
	}

	private void Game_GameOver()
	{
		GameTimer.Stop();
		_game._isGameOver = true;
		_isGameOver = true;
		//Invalidate();
	}

	private void Main_KeyDown(object sender, KeyEventArgs e)
	{
		// wyjœcie
		if (e.KeyCode == Keys.Escape)
		{
			Application.Exit();
		}

		// ponowne uruchomienie gry i restart zegarów
		if ((_isGameOver && e.KeyCode == Keys.S) || (e.KeyCode == Keys.R))
		{
			StartGame();
			return;
		}

		// strza³
		if (e.KeyCode == Keys.Space && !_isPaused)
		{
			_game.FireShot();
		}

		// pauza
		if (e.KeyCode == Keys.P)
		{
			if (GameTimer.Enabled)
			{
				GameTimer.Stop();
				_isPaused = true;
			}
			else
			{
				GameTimer.Start();
				_isPaused = false;
			}
		}

		if (_keysPressed.Contains(e.KeyCode))
		{
			_keysPressed.Remove(e.KeyCode);
		}

		_keysPressed.Add(e.KeyCode);
	}

	private void Main_KeyUp(object sender, KeyEventArgs e)
	{
		if (_keysPressed.Contains(e.KeyCode))
		{
			_keysPressed.Remove(e.KeyCode);
		}
	}

	// zegar gry
	private void GameTimer_Tick(object sender, EventArgs e)
	{
		_game.Go();

		foreach (var item in _keysPressed)
		{
			if (item == Keys.Left)
			{
				_game.MovePlayer(MoveDirection.Left);
				return;
			}
			else if (item == Keys.Right)
			{
				_game.MovePlayer(MoveDirection.Right);
				return;
			}
		}
	}

	// aktualizacja obrazków reprezentuj¹cych poszczególne klatki animacji najeŸdŸców
	private void AnimationTimer_Tick(object sender, EventArgs e)
	{
		_frame++;
		switch (_frame)
		{
			case 0: _animationCell = 0; break;
			case 1: _animationCell = 1; break;
			case 2: _animationCell = 2; break;
			case 3: _animationCell = 3; break;
			case 4: _animationCell = 2; break;
			case 5: _animationCell = 1; break;
			case 6: _animationCell = 0; _frame = 0; break;
		}
		Refresh();
	}

	private void Main_Paint(object sender, PaintEventArgs e) => _game.Draw(e.Graphics, _animationCell, _isPaused);
}
