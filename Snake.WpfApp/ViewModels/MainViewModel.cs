using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Snake.WpfApp.Commands;
using Snake.WpfApp.Models;
using Snake.WpfApp.Models.Enums;

namespace Snake.WpfApp.ViewModels;

internal class MainViewModel : BaseViewModel
{
	private readonly Random _random = new();
	private readonly MusicHelper _music = new();
	private readonly DispatcherTimer _gameTimer = new();
	private readonly LinkedList<MoveDirection> _directionChanges = new();
	private static readonly string _pathToSnakeFolder = Path.Combine(Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%"), "Snake");
	private readonly FileHelper<List<HighScore>> _fileHighScore = new(Path.Combine(_pathToSnakeFolder, "HighScores.json"));
	private readonly FileHelper<OptionsViewModel> _fileOptions = new(Path.Combine(_pathToSnakeFolder, "Options.json"));

	private readonly Dictionary<MoveDirection, int> _directionRotation = new()
	{
		{ MoveDirection.Up, 0 },
		{ MoveDirection.Right, 90 },
		{ MoveDirection.Down, 180 },
		{ MoveDirection.Left, 270 }
	};

	private MoveDirection _moveDirection;

	public MainViewModel()
	{
		SetCommands();
		ReadOptions();
		ReadHighScores();
		SetWindowState();
		Snake = new();
		_gameTimer.Tick += GameTimer_Tick;
	}

	#region Property binding

	/// <summary>
	/// Flag to set game over status
	/// </summary>
	public bool IsGameOver { get; set; }

	/// <summary>
	/// Property that represents the game options
	/// </summary>
	public OptionsViewModel Options { get; set; }

	/// <summary>
	/// An observable collection that represents the snake on the screen
	/// </summary>
	public ObservableCollection<SegmentViewModel> Snake { get; set; }

	/// <summary>
	/// An observable collection that represents the high score list
	/// </summary>
	public ObservableCollection<HighScore> HighScoreList { get; set; }

	/// <summary>
	/// Property that represents the food on the screen
	/// </summary>
	private SegmentViewModel _food;
	public SegmentViewModel Food
	{
		get => _food;
		set { _food = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Property that represents the countdown text on the screen before the game start
	/// </summary>
	private string _countdownText;
	public string CountdownText
	{
		get => _countdownText;
		set { _countdownText = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Property that represents the player name
	/// </summary>
	private string _currentPlayerName;
	public string CurrentPlayerName
	{
		get => _currentPlayerName;
		set { _currentPlayerName = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Property that stores the current score
	/// </summary>
	private int _currentScore;
	public int CurrentScore
	{
		get => _currentScore;
		set { _currentScore = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the countdown text
	/// </summary>
	private bool _countdownVisible;
	public bool CountdownVisible
	{
		get => _countdownVisible;
		set { _countdownVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the main menu
	/// </summary>
	private bool _mainMenuVisible;
	public bool MainMenuVisible
	{
		get => _mainMenuVisible;
		set { _mainMenuVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the main menu
	/// </summary>
	private bool _playGameVisible;
	public bool PlayGameVisible
	{
		get => _playGameVisible;
		set { _playGameVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the high scores
	/// </summary>
	private bool _highScoresVisible;
	public bool HighScoresVisible
	{
		get => _highScoresVisible;
		set { _highScoresVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the new high score
	/// </summary>
	private bool _newHighScoreVisible;
	public bool NewHighScoreVisible
	{
		get => _newHighScoreVisible;
		set { _newHighScoreVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the options menu
	/// </summary>
	private bool _optionsMenuVisible;
	public bool OptionsMenuVisible
	{
		get => _optionsMenuVisible;
		set { _optionsMenuVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the game over menu
	/// </summary>
	private bool _gameOverMenuVisible;
	public bool GameOverMenuVisible
	{
		get => _gameOverMenuVisible;
		set { _gameOverMenuVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the text with window title
	/// </summary>
	private bool _topTextTitleVisible;
	public bool TopTextTitleVisible
	{
		get => _topTextTitleVisible;
		set { _topTextTitleVisible = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the text with actual score
	/// </summary>
	private bool _topTextScoreVisible;
	public bool TopTextScoreVisible
	{
		get => _topTextScoreVisible;
		set { _topTextScoreVisible = value; OnPropertyChanged(); }
	}

	#endregion Property binding

	#region Commands

	public ICommand StartNewGameCommand { get; private set; }
	public ICommand ShowMainMenuCommand { get; private set; }
	public ICommand ShowHighScoresCommand { get; private set; }
	public ICommand SaveNewHighScoreCommand { get; private set; }
	public ICommand ShowOptionsCommand { get; private set; }
	public ICommand IncreaseSnakeSpeedCommand { get; private set; }
	public ICommand DecreaseSnakeSpeedCommand { get; private set; }
	public ICommand IncreaseGameGridSizeCommand { get; private set; }
	public ICommand DecreaseGameGridSizeCommand { get; private set; }
	public ICommand IncreaseSoundVolumeCommand { get; private set; }
	public ICommand DecreaseSoundVolumeCommand { get; private set; }
	public ICommand SaveOptionsCommand { get; private set; }
	public ICommand GameOverCommand { get; private set; }
	public ICommand ChangeSnakeDirectionCommand { get; private set; }
	public ICommand MinimizeWindowCommand { get; private set; }

	#endregion Commands

	private void SetCommands()
	{
		StartNewGameCommand = new RelayCommandAsync(StartNewGameAsync);
		ShowMainMenuCommand = new RelayCommand(ShowMainMenu);
		ShowHighScoresCommand = new RelayCommand(ShowHighScores);
		SaveNewHighScoreCommand = new RelayCommand(SaveNewHighScore);
		ShowOptionsCommand = new RelayCommand(ShowOptions);
		IncreaseSnakeSpeedCommand = new RelayCommand(IncreaseSnakeSpeed);
		DecreaseSnakeSpeedCommand = new RelayCommand(DecreaseSnakeSpeed);
		IncreaseGameGridSizeCommand = new RelayCommand(IncreaseGameGridSize);
		DecreaseGameGridSizeCommand = new RelayCommand(DecreaseGameGridSize);
		IncreaseSoundVolumeCommand = new RelayCommand(IncreaseSoundVolume);
		DecreaseSoundVolumeCommand = new RelayCommand(DecreaseSoundVolume);
		SaveOptionsCommand = new RelayCommand(SaveOptions);
		GameOverCommand = new RelayCommand(GameOver);
		ChangeSnakeDirectionCommand = new RelayCommand(ChangeSnakeDirection);
		MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
	}

	private async Task StartNewGameAsync(object commandParameter)
	{
		SetTimerInterval();
		SetGameState();
		SetNewSnake();
		MakeNewFood();
		ShowPlayGamePanel();
		_music.PlayMusic(MusicKind.Game);
		await ShowCountDown();
		_gameTimer.Start();
	}

	private void ShowMainMenu(object commandParameter) => ShowMainMenuPanel();

	private void ShowHighScores(object commandParameter) => ShowHighScoresPanel();

	private void SaveNewHighScore(object commandParameter)
	{
		int index = 0;
		if (HighScoreList.Any())
		{
			index = HighScoreList.IndexOf(HighScoreList.First(x => x.Score <= CurrentScore));
		}
		if (string.IsNullOrWhiteSpace(CurrentPlayerName))
		{
			CurrentPlayerName = "Snake Player";
		}
		HighScoreList.Insert(index, new() { PlayerName = CurrentPlayerName, Score = CurrentScore });
		if (HighScoreList.Count > 5)
		{
			HighScoreList.Remove(HighScoreList.Last());
		}
		SaveHighScores();
		ShowHighScoresPanel();
	}

	private void ShowOptions(object commandParameter) => ShowOptionsMenuPanel();

	private void IncreaseSnakeSpeed(object commandParameter) => Options.SnakeSpeed++;

	private void DecreaseSnakeSpeed(object commandParameter) => Options.SnakeSpeed--;

	private void IncreaseGameGridSize(object commandParameter)
	{
		if (Options.GameGridSize < 1000)
		{
			Options.GameGridSize += 200;
			Window window = commandParameter as Window;
			window.Left = window.Left - 100;
			window.Top = window.Top - 100;
		}
	}

	private void DecreaseGameGridSize(object commandParameter)
	{
		if (Options.GameGridSize > 400)
		{
			Options.GameGridSize -= 200;
			Window window = commandParameter as Window;
			window.Left = window.Left + 100;
			window.Top = window.Top + 100;
		}
	}

	private void IncreaseSoundVolume(object commandParameter) => _music.SetVolume(Options.SoundVolume++);

	private void DecreaseSoundVolume(object commandParameter) => _music.SetVolume(Options.SoundVolume--);

	private void SaveOptions(object commandParameter)
	{
		_fileOptions.SerializeToJSON(Options);
		ShowMainMenuPanel();
	}

	private void GameOver(object commandParameter) => IsGameOver = true;

	private void ChangeSnakeDirection(object commandParameter)
	{
		if (IsGameOver)
		{
			return;
		}

		if (commandParameter is Key key)
		{
			MoveDirection moveDirection = key switch
			{
				Key.Up => MoveDirection.Up,
				Key.Down => MoveDirection.Down,
				Key.Left => MoveDirection.Left,
				Key.Right => MoveDirection.Right
			};
			ChangeDirection(moveDirection);
		}
	}

	private void SetTimerInterval()
	{
		var interval = (10 - Options.SnakeSpeed) * 100 / 3;
		_gameTimer.Interval = TimeSpan.FromMilliseconds(interval);
	}

	private void SetWindowState()
	{
		_music.PlayMusic(MusicKind.Menu);
		ShowMainMenuPanel();
	}

	private void ReadOptions()
	{
		Options = _fileOptions.DeserializeFromJSON();
		_music.SetVolume(Options.SoundVolume);
	}

	private void ReadHighScores() => HighScoreList = new(_fileHighScore.DeserializeFromJSON());

	private void SaveHighScores() => _fileHighScore.SerializeToJSON(HighScoreList.ToList());

	private async Task ShowCountDown()
	{
		CountdownVisible = true;
		for (int i = 3; i > 0; i--)
		{
			CountdownText = i.ToString();
			await Task.Delay(600);
		}
		CountdownVisible = false;
	}

	private void SetGameState()
	{
		IsGameOver = false;
		CurrentScore = 0;
		_directionChanges.Clear();
		_moveDirection = MoveDirection.Right;
	}

	private void SetNewSnake()
	{
		int startPos = Options.GameGridSize / 2 - Options.SegmentSize;
		Snake.Clear();
		Snake.Add(new SegmentViewModel(startPos, startPos, Options.SegmentSize, SegmentKind.Head));
		Snake.Add(new SegmentViewModel(startPos - Options.SegmentSize, startPos, Options.SegmentSize, SegmentKind.Body));
		Snake.Add(new SegmentViewModel(startPos - 2 * Options.SegmentSize, startPos, Options.SegmentSize, SegmentKind.Tail));
	}

	private void ChangeDirection(MoveDirection direction)
	{
		if (CanChangeDirection(direction))
		{
			_directionChanges.AddLast(direction);
		}
	}

	private bool CanChangeDirection(MoveDirection newDirection)
	{
		if (_directionChanges.Count == 2)
		{
			return false;
		}

		MoveDirection lastDirection = GetLastDirection();
		return newDirection != lastDirection && newDirection != OppositeDirection(lastDirection);
	}

	private MoveDirection GetLastDirection() => _directionChanges.Count == 0 ? _moveDirection : _directionChanges.Last.Value;

	private MoveDirection OppositeDirection(MoveDirection moveDirection) => moveDirection switch
	{
		MoveDirection.Up => MoveDirection.Down,
		MoveDirection.Down => MoveDirection.Up,
		MoveDirection.Left => MoveDirection.Right,
		MoveDirection.Right => MoveDirection.Left,
		_ => throw new InvalidOperationException("Wrong direction!"),
	};

	private async void GameTimer_Tick(object? sender, EventArgs e)
	{
		if (IsGameOver)
		{
			_gameTimer.Stop();

			await DrawDeadSnake();

			if (CheckIfNewHighScore())
			{
				ShowNewHighScorePanel();
			}
			else
			{
				ShowGameOverPanel();
			}

			_music.PlayMusic(MusicKind.Menu);

			return;
		}

		MoveSnake();
	}

	private async Task DrawDeadSnake()
	{
		await Task.Delay(100);
		Snake.First().ImageSource = Images.DeadHead;
		for (int i = 1; i < Snake.Count - 1; i++)
		{
			await Task.Delay(100);
			Snake.ElementAt(i).ImageSource = Images.DeadBody;
		}
		await Task.Delay(100);
		Snake.Last().ImageSource = Images.DeadTail;
		await Task.Delay(1000);
	}

	private bool CheckIfNewHighScore() => HighScoreList.Count == 0 || (CurrentScore >= HighScoreList.Min(x => x.Score));

	private void MakeNewFood()
	{
		int xPos;
		int yPos;
		bool isSnakeSegment = false;
		do
		{
			xPos = _random.Next(Options.GridColumns) * Options.SegmentSize;
			yPos = _random.Next(Options.GridColumns) * Options.SegmentSize;
			isSnakeSegment = Snake.FirstOrDefault(s => s.XPos == xPos && s.YPos == yPos) != null;
		} while (isSnakeSegment);
		Food = new SegmentViewModel(xPos, yPos, Options.SegmentSize, SegmentKind.Food);
	}

	private void MoveSnake()
	{
		SetMoveDirection();
		(int newXPos, int newYPos) = GetNewPosition();
		CheckSnakeCollision(newXPos, newYPos);

		if (IsGameOver)
		{
			return;
		}

		Snake.First().ImageSource = Images.Body;
		Snake.Insert(0, new SegmentViewModel(newXPos, newYPos, Options.SegmentSize, SegmentKind.Head) { Rotation = _directionRotation[_moveDirection] });

		if (newXPos == Food.XPos && newYPos == Food.YPos)
		{
			CurrentScore += Options.SnakeSpeed;
			MakeNewFood();
		}
		else
		{
			Snake.Remove(Snake.Last());
			Snake.Last().ImageSource = Images.Tail;
		}
	}

	private void SetMoveDirection()
	{
		if (_directionChanges.Count > 0)
		{
			_moveDirection = _directionChanges.First.Value;
			_directionChanges.RemoveFirst();
		}
	}

	private (int, int) GetNewPosition()
	{
		var snakeHead = Snake.First();
		int xPos = snakeHead.XPos;
		int yPos = snakeHead.YPos;

		switch (_moveDirection)
		{
			case MoveDirection.Left:
				xPos -= Options.SegmentSize;
				break;
			case MoveDirection.Right:
				xPos += Options.SegmentSize;
				break;
			case MoveDirection.Up:
				yPos -= Options.SegmentSize;
				break;
			case MoveDirection.Down:
				yPos += Options.SegmentSize;
				break;
		}

		if (!Options.IsFieldHaveBorder)
		{
			int maxGameGridSize = Options.GameGridSize - Options.SegmentSize;
			if (xPos < 0)
			{
				xPos = maxGameGridSize;
			}
			else if (xPos > maxGameGridSize)
			{
				xPos = 0;
			}

			if (yPos < 0)
			{
				yPos = maxGameGridSize;
			}
			else if (yPos > maxGameGridSize)
			{
				yPos = 0;
			}
		}

		return (xPos, yPos);
	}

	private void CheckSnakeCollision(int xPos, int yPos)
	{
		for (int i = 0; i < Snake.Count - 1; i++)
		{
			if (Snake[i].XPos == xPos && Snake[i].YPos == yPos)
			{
				IsGameOver = true;
				return;
			}
		}

		if (Options.IsFieldHaveBorder)
		{
			if (xPos < 0 || xPos >= Options.GameGridSize || yPos < 0 || yPos >= Options.GameGridSize)
			{
				IsGameOver = true;
				return;
			}
		}
	}

	private void MinimizeWindow(object commandParameter) => (commandParameter as Window).WindowState = WindowState.Minimized;

	#region Show/Hide Panels

	private void ShowPlayGamePanel()
	{
		PlayGameVisible = TopTextScoreVisible = true;
		MainMenuVisible = GameOverMenuVisible = TopTextTitleVisible = false;
	}

	private void ShowHighScoresPanel()
	{
		HighScoresVisible = TopTextTitleVisible = true;
		GameOverMenuVisible = MainMenuVisible = NewHighScoreVisible = false;
	}

	private void ShowNewHighScorePanel()
	{
		NewHighScoreVisible = TopTextTitleVisible = true;
		PlayGameVisible = TopTextScoreVisible = false;
	}

	private void ShowOptionsMenuPanel()
	{
		OptionsMenuVisible = true;
		MainMenuVisible = false;
	}

	private void ShowMainMenuPanel()
	{
		MainMenuVisible = TopTextTitleVisible = true;
		GameOverMenuVisible = OptionsMenuVisible = HighScoresVisible = false;
	}

	private void ShowGameOverPanel()
	{
		GameOverMenuVisible = TopTextTitleVisible = true;
		PlayGameVisible = TopTextScoreVisible = false;
	}

	#endregion Show/Hide Panels
}