using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Snake.WpfApp.Commands;
using Snake.WpfApp.Models;
using Snake.WpfApp.Models.Enums;

namespace Snake.WpfApp.ViewModels;

internal class MainViewModel : BaseViewModel
{
	private const int SEGMENT_SIZE = 20;

	private readonly Random _random = new();
	private readonly MediaPlayer _mediaPlayer = new();
	private readonly DispatcherTimer _gameTimer = new();
	private readonly LinkedList<MoveDirection> _directionChanges = new();
	private readonly static string _pathToSnakeFolder = Path.Combine(Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%"), "Snake");
	private readonly FileHelper<List<HighScore>> _fileHighScore = new(Path.Combine(_pathToSnakeFolder, "HighScores.json"));
	private readonly FileHelper<Options> _fileOptions = new(Path.Combine(_pathToSnakeFolder, "Options.json"));
	private readonly Dictionary<MoveDirection, int> _directionRotation = new()
	{
		{ MoveDirection.Up, 0 },
		{ MoveDirection.Right, 90 },
		{ MoveDirection.Down, 180 },
		{ MoveDirection.Left, 270 }
	};

	private MoveDirection _moveDirection;
	private Options _options;
	private int _gridColumns;

	public MainViewModel()
	{
		SetCommands();
		ReadOptions();
		ReadHighScores();
		SetWindowState();
		Snake = new();
		_gameTimer.Tick += GameTimer_Tick;
		_mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
	}

	/// <summary>
	/// Flag to set game over status
	/// </summary>
	public bool IsGameOver { get; set; }

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
	/// Flag to set the visibility of the countdown text
	/// </summary>
	private bool _countdownVisibile;
	public bool CountdownVisibile
	{
		get => _countdownVisibile;
		set { _countdownVisibile = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the main menu
	/// </summary>
	private bool _mainMenuVisibile;
	public bool MainMenuVisibile
	{
		get => _mainMenuVisibile;
		set { _mainMenuVisibile = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the main menu
	/// </summary>
	private bool _playGameVisibile;
	public bool PlayGameVisibile
	{
		get => _playGameVisibile;
		set { _playGameVisibile = value; OnPropertyChanged(); }
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
	private bool _gameOverMenuVisibile;
	public bool GameOverMenuVisibile
	{
		get => _gameOverMenuVisibile;
		set { _gameOverMenuVisibile = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the text with window title
	/// </summary>
	private bool _topTextTitleVisibile;
	public bool TopTextTitleVisibile
	{
		get => _topTextTitleVisibile;
		set { _topTextTitleVisibile = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Flag to set the visibility of the text with actual score
	/// </summary>
	private bool _topTextScoreVisibile;
	public bool TopTextScoreVisibile
	{
		get => _topTextScoreVisibile;
		set { _topTextScoreVisibile = value; OnPropertyChanged(); }
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
	/// Property to set current snake speed
	/// </summary>
	public int SnakeSpeed
	{
		get => _options.SnakeSpeed;
		set { _options.SnakeSpeed = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Property to set current game grid size
	/// </summary>
	public int GameGridSize
	{
		get => _options.GameGridSize;
		set { _options.GameGridSize = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Property to set current sound volume
	/// </summary>
	public double SoundVolume
	{
		get => _options.SoundVolume;
		set { _options.SoundVolume = value; OnPropertyChanged(); }
	}

	/// <summary>
	/// Property to set playfield border
	/// </summary>
	public bool IsFieldHaveBorder
	{
		get => _options.IsFieldHaveBorder;
		set { _options.IsFieldHaveBorder = value; OnPropertyChanged(); }
	}

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
	public ICommand WindowMouseDownCommand { get; private set; }
	public ICommand MinimizeWindowCommand { get; private set; }
	public ICommand CloseWindowCommand { get; private set; }

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
		WindowMouseDownCommand = new RelayCommand(WindowMouseDown);
		MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
		CloseWindowCommand = new RelayCommand(CloseWindow);
	}

	private void SetTimerInterval()
	{
		var interval = (10 - SnakeSpeed) * 100 / 3;
		_gameTimer.Interval = TimeSpan.FromMilliseconds(interval);
	}

	private void SetWindowState()
	{
		PlayMusic("menu");
		ShowMainMenuPanel();
		SetGridColumns();
	}

	private void MediaPlayer_MediaEnded(object? sender, EventArgs e) => _mediaPlayer.Position = TimeSpan.Zero;

	private void ReadOptions()
	{
		_options = _fileOptions.DeserializeFromJSON();
		if (_options.SnakeSpeed == 0)
		{
			_options.SnakeSpeed = 1;
			_options.GameGridSize = 600;
			_options.SoundVolume = 0.5;
			_options.IsFieldHaveBorder = false;
		}
		_mediaPlayer.Volume = SoundVolume;
	}

	private void ReadHighScores() => HighScoreList = new(_fileHighScore.DeserializeFromJSON());

	private void SaveHighScores() => _fileHighScore.SerializeToJSON(HighScoreList.ToList());

	private async Task StartNewGameAsync(object obj)
	{
		SetTimerInterval();
		SetGameState();
		SetNewSnake();
		MakeNewFood();
		ShowPlayGamePanel();
		await ShowCountDown();
		_gameTimer.Start();
	}

	private async Task ShowCountDown()
	{
		CountdownVisibile = true;
		for (int i = 3; i > 0; i--)
		{
			CountdownText = i.ToString();
			await Task.Delay(600);
		}
		CountdownVisibile = false;
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
		int startPos = GameGridSize / 2 - SEGMENT_SIZE;
		Snake.Clear();
		Snake.Add(new SegmentViewModel(startPos, startPos, SegmentKind.Head));
		Snake.Add(new SegmentViewModel(startPos - SEGMENT_SIZE, startPos, SegmentKind.Body));
		Snake.Add(new SegmentViewModel(startPos - 2 * SEGMENT_SIZE, startPos, SegmentKind.Tail));
	}

	private void ShowPlayGamePanel()
	{
		PlayGameVisibile = TopTextScoreVisibile = true;
		MainMenuVisibile = GameOverMenuVisibile = TopTextTitleVisibile = false;
		PlayMusic("game");
	}

	private void ShowHighScoresPanel()
	{
		HighScoresVisible = TopTextTitleVisibile = true;
		GameOverMenuVisibile = MainMenuVisibile = NewHighScoreVisible = false;
	}

	private void ShowNewHighScorePanel()
	{
		NewHighScoreVisible = TopTextTitleVisibile = true;
		PlayGameVisibile = TopTextScoreVisibile = false;
		PlayMusic("menu");
	}

	private void ShowOptionsMenuPanel()
	{
		OptionsMenuVisible = true;
		MainMenuVisibile = false;
	}

	private void ShowMainMenuPanel()
	{
		MainMenuVisibile = TopTextTitleVisibile = true;
		GameOverMenuVisibile = OptionsMenuVisible = HighScoresVisible = false;
	}

	private void ShowGameOverPanel()
	{
		GameOverMenuVisibile = TopTextTitleVisibile = true;
		PlayGameVisibile = TopTextScoreVisibile = false;
		PlayMusic("menu");
	}

	private void ShowMainMenu(object obj) => ShowMainMenuPanel();

	private void ShowHighScores(object obj) => ShowHighScoresPanel();

	private void SaveNewHighScore(object obj)
	{
		int index = 0;
		if (HighScoreList.Any())
		{
			var test = HighScoreList.First(x => x.Score <= CurrentScore);
			index = HighScoreList.IndexOf(test);
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

	private void ShowOptions(object obj) => ShowOptionsMenuPanel();

	private void IncreaseSnakeSpeed(object obj)
	{
		if (SnakeSpeed < 9)
		{
			SnakeSpeed++;
		}
	}

	private void DecreaseSnakeSpeed(object obj)
	{
		if (SnakeSpeed > 1)
		{
			SnakeSpeed--;
		}
	}

	private void IncreaseGameGridSize(object obj)
	{
		if (GameGridSize < 1000)
		{
			GameGridSize += 200;
			Window window = obj as Window;
			window.Left = window.Left - 100;
			window.Top = window.Top - 100;
			SetGridColumns();
		}
	}

	private void DecreaseGameGridSize(object obj)
	{
		if (GameGridSize > 400)
		{
			GameGridSize -= 200;
			Window window = obj as Window;
			window.Left = window.Left + 100;
			window.Top = window.Top + 100;
			SetGridColumns();
		}
	}

	private void IncreaseSoundVolume(object obj)
	{
		if (_mediaPlayer.Volume < 1)
		{
			SoundVolume += 0.1;
			SoundVolume = Math.Round(SoundVolume, 1);
			_mediaPlayer.Volume = SoundVolume;
		}
	}

	private void DecreaseSoundVolume(object obj)
	{
		if (_mediaPlayer.Volume > 0)
		{
			SoundVolume -= 0.1;
			SoundVolume = Math.Round(SoundVolume, 1);
			_mediaPlayer.Volume = SoundVolume;
		}
	}

	private void SaveOptions(object obj)
	{
		_fileOptions.SerializeToJSON(_options);
		ShowMainMenuPanel();
	}

	private void GameOver(object obj) => IsGameOver = true;

	private void ChangeSnakeDirection(object obj)
	{
		if (IsGameOver)
		{
			return;
		}

		if (obj is Key key)
		{
			switch (key)
			{
				case Key.Up:
					ChangeDirection(MoveDirection.Up);
					break;
				case Key.Down:
					ChangeDirection(MoveDirection.Down);
					break;
				case Key.Left:
					ChangeDirection(MoveDirection.Left);
					break;
				case Key.Right:
					ChangeDirection(MoveDirection.Right);
					break;
				default:
					break;
			}
		}
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
			xPos = _random.Next(_gridColumns) * SEGMENT_SIZE;
			yPos = _random.Next(_gridColumns) * SEGMENT_SIZE;
			isSnakeSegment = Snake.FirstOrDefault(s => s.XPos == xPos && s.YPos == yPos) != null;
		} while (isSnakeSegment);
		Food = new SegmentViewModel(xPos, yPos, SegmentKind.Food);
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
		Snake.Insert(0, new SegmentViewModel(newXPos, newYPos, SegmentKind.Head) { Rotation = _directionRotation[_moveDirection] });

		if (newXPos == Food.XPos && newYPos == Food.YPos)
		{
			CurrentScore += SnakeSpeed;
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
				xPos -= SEGMENT_SIZE;
				break;
			case MoveDirection.Right:
				xPos += SEGMENT_SIZE;
				break;
			case MoveDirection.Up:
				yPos -= SEGMENT_SIZE;
				break;
			case MoveDirection.Down:
				yPos += SEGMENT_SIZE;
				break;
		}

		if (!IsFieldHaveBorder)
		{
			int maxGameGridSize = GameGridSize - SEGMENT_SIZE;
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

		if (IsFieldHaveBorder)
		{
			if (xPos < 0 || xPos >= GameGridSize || yPos < 0 || yPos >= GameGridSize)
			{
				IsGameOver = true;
				return;
			}
		}
	}

	private void SetGridColumns() => _gridColumns = (GameGridSize / SEGMENT_SIZE);

	private void PlayMusic(string fileName)
	{
		_mediaPlayer.Open(new Uri($"Sounds/{fileName}.wav", UriKind.Relative));
		_mediaPlayer.Play();
	}

	private void WindowMouseDown(object obj) => (obj as Window).DragMove();

	private void MinimizeWindow(object obj) => (obj as Window).WindowState = WindowState.Minimized;

	private void CloseWindow(object obj) => (obj as Window).Close();
}