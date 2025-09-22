using Tetris.WinForms.Enums;
using Tetris.WinForms.Models;

namespace Tetris.WinForms.Views;

public partial class MainForm : Form
{
	private Game? _game;
	private MainMenu _mainMenu = default!;
	private ApplicationState _applicatinState;
	private Rectangle _playfield;
	private System.Windows.Forms.Timer _animationTimer = default!;

	public MainForm()
	{
		InitializeComponent();
		SetMainForm();
		StartMenu();
		SetAnimationTimer();
	}

	private void SetMainForm()
	{
		this.Paint += Main_Paint;
		this.StartPosition = FormStartPosition.CenterScreen;
		this.DoubleBuffered = true;

		_playfield = this.ClientRectangle;
	}

	private void SetAnimationTimer()
	{
		_animationTimer = new();
		_animationTimer.Interval = 16;
		_animationTimer.Tick += AnimationTimer_Tick;
		_animationTimer.Start();
	}

	private void AnimationTimer_Tick(object? sender, EventArgs e)
		=> Refresh();

	private void StartMenu()
	{
		this.KeyDown += MainForm_MenuKeyDown;
		_applicatinState = ApplicationState.MainMenu;
		_mainMenu = new(_playfield);
	}

	private void StartNewGame()
	{
		this.KeyDown -= MainForm_MenuKeyDown;
		this.KeyDown += MainForm_GameKeyDown;

		_applicatinState = ApplicationState.ActiveGame;
		_game = new(_playfield);
		_game.GameExit += Game_GameExit;
		_game.Start();
	}

	private void Game_GameExit()
	{
		if (_game is not null)
		{
			_game.Stop();
			this.KeyDown -= MainForm_GameKeyDown;
			_game.GameExit -= Game_GameExit;
			StartMenu();
		}
	}

	private void MainForm_MenuKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Down)
		{
			_mainMenu.Next();
		}
		else if (e.KeyCode == Keys.Up)
		{
			_mainMenu.Previous();
		}
		else if (e.KeyCode == Keys.Escape)
		{
			Application.Exit();
		}
		else if (e.KeyCode == Keys.Enter)
		{
			switch (_mainMenu.ActualState)
			{
				case MainMenuState.NewGame:
					StartNewGame();
					break;
				case MainMenuState.Options:
					MessageBox.Show("Opcje");
					break;
				case MainMenuState.Exit:
					Application.Exit();
					break;
				default:
					throw new ArgumentException("Nie ma takiego stanu menu!");
			}
		}
	}

	private void MainForm_GameKeyDown(object? sender, KeyEventArgs e)
		=> _game?.DoAction(e.KeyCode);

	private void Main_Paint(object? sender, PaintEventArgs e)
	{
		switch (_applicatinState)
		{
			case ApplicationState.MainMenu:
				_mainMenu.Draw(e.Graphics);
				break;
			case ApplicationState.ActiveGame:
				_game?.Draw(e.Graphics);
				break;
			case ApplicationState.GameOver:
				break;
			case ApplicationState.Pause:
				break;
			default:
				throw new ArgumentException("Nie ma takiego stanu gry!");
		}
	}
}