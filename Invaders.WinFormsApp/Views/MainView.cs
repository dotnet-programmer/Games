using Invaders.WinFormsApp.Models;

namespace Invaders.WinFormsApp.Views;

public partial class MainView : Form
{
	private readonly List<Keys> _keysPressed = [];

	private bool _isGameOver;
	private bool _isPaused;
	private int _frame;
	private int _animationCell;
	private Game _game = null!;

	public MainView()
	{
		InitializeComponent();
		StartNewGame();
	}

	private void Main_KeyDown(object sender, KeyEventArgs e)
	{
		// Exit
		if (e.KeyCode == Keys.Escape)
		{
			Application.Exit();
		}

		// Restart game
		if ((_isGameOver && e.KeyCode == Keys.S) || (e.KeyCode == Keys.R))
		{
			StartNewGame();
			return;
		}

		// Shot
		if (e.KeyCode == Keys.Space && !_isPaused)
		{
			_game.FireShot();
		}

		// Pause
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

		_keysPressed.Remove(e.KeyCode);
		_keysPressed.Add(e.KeyCode);
	}

	private void Main_KeyUp(object sender, KeyEventArgs e)
		=> _keysPressed.Remove(e.KeyCode);

	private void GameTimer_Tick(object sender, EventArgs e)
	{
		_game.GoNextFrame();

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

	// Update images representing individual frames of the invaders' animation
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

	private void Main_Paint(object sender, PaintEventArgs e)
		=> _game.Draw(e.Graphics, _animationCell, _isPaused);

	private void StartNewGame()
	{
		_game = new Game(this.ClientRectangle);
		_game.GameOver += Game_GameOver;
		_isPaused = _isGameOver = false;
		AnimationTimer.Start();
		GameTimer.Start();
	}

	private void Game_GameOver(object? sender, EventArgs e)
	{
		GameTimer.Stop();
		_game.IsGameOver = true;
		_isGameOver = true;
	}
}