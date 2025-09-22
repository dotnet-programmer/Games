using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris.WpfApp.Commands;
using Tetris.WpfApp.Models;

namespace Tetris.WpfApp.ViewModels;

public class GameViewModel : INotifyPropertyChanged
{
	private GameState _gameState = new();
	private readonly ImageSource[] _tileImages =
	[
		new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
	];
	private readonly ImageSource[] _blockImages =
	[
		new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
		new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
	];

	public event PropertyChangedEventHandler PropertyChanged;

	public GameState GameState => _gameState;
	public int Score => _gameState.Score;
	public bool GameOver => _gameState.GameOver;
	public ImageSource NextBlockImage => _blockImages[_gameState.BlockQueue.NextBlock.Id];
	public ImageSource HeldBlockImage => _gameState.HeldBlock == null ? _blockImages[0] : _blockImages[_gameState.HeldBlock.Id];

	public ICommand MoveLeftCommand { get; }
	public ICommand MoveRightCommand { get; }
	public ICommand MoveDownCommand { get; }
	public ICommand RotateClockwiseCommand { get; }
	public ICommand RotateCounterClockwiseCommand { get; }
	public ICommand HoldBlockCommand { get; }
	public ICommand DropBlockCommand { get; }
	public ICommand PlayAgainCommand { get; }

	public GameViewModel()
	{
		MoveLeftCommand = new RelayCommand(_ => MoveLeft());
		MoveRightCommand = new RelayCommand(_ => MoveRight());
		MoveDownCommand = new RelayCommand(_ => MoveDown());
		RotateClockwiseCommand = new RelayCommand(_ => RotateClockwise());
		RotateCounterClockwiseCommand = new RelayCommand(_ => RotateCounterClockwise());
		HoldBlockCommand = new RelayCommand(_ => HoldBlock());
		DropBlockCommand = new RelayCommand(_ => DropBlock());
		PlayAgainCommand = new RelayCommand(async _ => await PlayAgain());
	}

	public async Task StartGameLoop()
	{
		OnPropertyChanged(null);
		while (!_gameState.GameOver)
		{
			int delay = Math.Max(75, 1000 - (_gameState.Score * 25));
			await Task.Delay(delay);
			_gameState.MoveBlockDown();
			OnPropertyChanged(null);
		}
		OnPropertyChanged(nameof(GameOver));
	}

	private void MoveLeft() { _gameState.MoveBlockLeft(); OnPropertyChanged(null); }
	private void MoveRight() { _gameState.MoveBlockRight(); OnPropertyChanged(null); }
	private void MoveDown() { _gameState.MoveBlockDown(); OnPropertyChanged(null); }
	private void RotateClockwise() { _gameState.RotateBlockClockwise(); OnPropertyChanged(null); }
	private void RotateCounterClockwise() { _gameState.RotateBlockCounterClockwise(); OnPropertyChanged(null); }
	private void HoldBlock() { _gameState.HoldBlock(); OnPropertyChanged(null); }
	private void DropBlock() { _gameState.DropBlock(); OnPropertyChanged(null); }
	private async Task PlayAgain()
	{
		_gameState = new GameState();
		OnPropertyChanged(null);
		await StartGameLoop();
	}

	protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}