using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris.WpfApp.Models;
using Tetris.WpfApp.Models.Blocks;

namespace Tetris.WpfApp.Views;

public partial class MainWindow : Window
{
	// array containing images of tiles
	// important order, each color corresponds to the id of the block
	private readonly ImageSource[] _tileImages =
	[
		new BitmapImage(new Uri("/Resources/TileEmpty.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/TileCyan.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/TileBlue.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/TileOrange.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/TileYellow.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/TileGreen.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/TilePurple.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/TileRed.png", UriKind.Relative)),
	];

	// array containing images of blocks
	// the order here corresponds to the ID of each block
	private readonly ImageSource[] _blockImages =
	[
		new BitmapImage(new Uri("/Resources/Block-Empty.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/Block-I.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/Block-J.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/Block-L.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/Block-O.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/Block-S.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/Block-T.png", UriKind.Relative)),
		new BitmapImage(new Uri("/Resources/Block-Z.png", UriKind.Relative)),
	];

	// an array of image controls for each cell in the game grid
	private readonly Image[,] _imageControls;
	private GameState _gameState = new();

	// values associated with increasing game speed as player score increases
	private readonly int _maxDelay = 1000;
	private readonly int _minDelay = 75;
	private readonly int _delayDecrease = 25;

	public MainWindow()
	{
		InitializeComponent();
		_imageControls = SetupGameCanvas(_gameState.GameGrid);
	}

	// keystroke detection
	private void Window_KeyDown(object sender, KeyEventArgs e)
	{
		// if the end of the game, key presses do nothing
		if (_gameState.GameOver)
		{
			return;
		}

		switch (e.Key)
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

		Draw(_gameState);
	}

	// game start when Canvas is loaded
	private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
		=> await GameLoop();

	// game restart
	private async void PlayAgain_Click(object sender, RoutedEventArgs e)
	{
		// new game state
		_gameState = new GameState();

		// hide the end game menu
		GameOverMenu.Visibility = Visibility.Hidden;

		// launch a new game loop
		await GameLoop();
	}

	// game loop, causing automatic downward movement of blocks
	private async Task GameLoop()
	{
		// draw the state of the game
		Draw(_gameState);

		// loop runs until the game ends
		while (!_gameState.GameOver)
		{
			// delay reduced with the result, but will never fall below the minimum delay
			int delay = Math.Max(_minDelay, _maxDelay - (_gameState.Score * _delayDecrease));
			await Task.Delay(delay);

			// move the block down
			_gameState.MoveBlockDown();

			// draw the state of the game
			Draw(_gameState);
		}

		// if end of game, display end of game screen
		GameOverMenu.Visibility = Visibility.Visible;
		FinalScoreText.Text = $"Score: {_gameState.Score}";
	}

	// image control configuration on Canvas
	private Image[,] SetupGameCanvas(GameGrid gameGrid)
	{
		// size corresponds to the game grid
		Image[,] imageControls = new Image[gameGrid.Rows, gameGrid.Columns];

		// width and height of each cell
		int cellSize = 25;

		for (int row = 0; row < gameGrid.Rows; row++)
		{
			for (int col = 0; col < gameGrid.Columns; col++)
			{
				// new image control created for each cell
				Image imageControl = new()
				{
					Width = cellSize,
					Height = cellSize,
				};

				// positioning on Canva
				// rows counted from top down, columns from left to right
				// 2 means moving 2 hidden top rows, so these will not be on Canva
				// 10 - showing part of the hidden line to show which block caused the end of the game (think about it if I want to have it)
				Canvas.SetTop(imageControl, (row - 2) * cellSize + 10);
				Canvas.SetLeft(imageControl, col * cellSize);
				GameCanvas.Children.Add(imageControl);
				imageControls[row, col] = imageControl;
			}
		}
		// returns a two-dimensional array with one image for each cell in the game grid
		return imageControls;
	}

	// draws all the elements of the game
	private void Draw(GameState gameState)
	{
		DrawGrid(gameState.GameGrid);
		DrawGhostBlock(gameState.CurrentBlock);
		DrawBlock(gameState.CurrentBlock);
		DrawNextBlock(gameState.BlockQueue);
		DrawHeldBlock(gameState.HeldBlock);
		ScoreText.Text = $"Score: {gameState.Score}";
	}

	// drawing the game grid
	private void DrawGrid(GameGrid gameGrid)
	{
		for (int row = 0; row < gameGrid.Rows; row++)
		{
			for (int col = 0; col < gameGrid.Columns; col++)
			{
				// retrieve the initial ID for each item 
				int id = gameGrid[row, col];

				// setting the image source in this position with ID
				// transparency reset due to changes to this value in the method that draws the "ghost" of the current block
				_imageControls[row, col].Opacity = 1;
				_imageControls[row, col].Source = _tileImages[id];
			}
		}
	}

	// drawing the current block
	private void DrawBlock(Block block)
	{
		// go through all the tile items and update the image source with the ID
		foreach (Position p in block.TilePositions())
		{
			// transparency reset due to changes to this value in the method that draws the "ghost" of the current block
			_imageControls[p.Row, p.Column].Opacity = 1;
			_imageControls[p.Row, p.Column].Source = _tileImages[block.Id];
		}
	}

	// drawing a preview of the next block
	private void DrawNextBlock(BlockQueue blockQueue)
	{
		Block nextBlock = blockQueue.NextBlock;
		NextImage.Source = _blockImages[nextBlock.Id];
	}

	// drawing the stored block
	private void DrawHeldBlock(Block heldBlock)
	{
		if (heldBlock == null)
		{
			HoldImage.Source = _blockImages[0];
		}
		else
		{
			HoldImage.Source = _blockImages[heldBlock.Id];
		}
	}

	// drawing the "ghost" of the current block in the last place it can reach
	// the cells in which it lands are found by adding the drop distance to the tile position of the current block
	private void DrawGhostBlock(Block block)
	{
		int dropDistance = _gameState.BlockDropDistance();
		foreach (Position p in block.TilePositions())
		{
			_imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
			_imageControls[p.Row + dropDistance, p.Column].Source = _tileImages[block.Id];
		}
	}
}