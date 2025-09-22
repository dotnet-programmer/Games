using System.Windows.Input;
using Tetris.WpfApp.Commands;
using Tetris.WpfApp.Models;

namespace Tetris.WpfApp.ViewModels;

internal class MainViewModel : BaseViewModel
{
	private GameState _gameState = new();

	public MainViewModel()
	{
		SetCommands();
	}

	public int Score => _gameState.Score;

	public ICommand KeyDownCommand { get; private set; } = default!;
	public ICommand GameOverCommand { get; private set; } = default!;

	private void SetCommands()
	{
		KeyDownCommand = new RelayCommand(KeyDown);
		GameOverCommand = new RelayCommand(GameOver);
	}

	// keystroke detection
	private void KeyDown(object? commandParameter)
	{
		// jeśli koniec gry, to naciśnięcia klawiszy nic nie robią
		if (_gameState.GameOver)
		{
			return;
		}

		if (commandParameter is Key key)
		{
			switch (key)
			{
				case Key.Left:
					_gameState.MoveBlockLeft();
					break;
				case Key.Right:
					_gameState.MoveBlockRight();
					break;
				case Key.Down:
					_gameState.MoveBlockDown();
					break;
				case Key.Space:
					_gameState.RotateBlockClockwise();
					break;
				case Key.Z:
					_gameState.RotateBlockCounterClockwise();
					break;
				case Key.C:
					_gameState.HoldBlock();
					break;
				case Key.LeftShift:
					_gameState.DropBlock();
					break;
				default:
					return;
			}
		}

		//Draw(_gameState);
	}

	private void GameOver()
	{ }
	//=> IsGameOver = true;
}