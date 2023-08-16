namespace Snake.WpfApp.ViewModels;

internal class OptionsViewModel : BaseViewModel
{
	public OptionsViewModel()
	{
		SetStartValues();
		SetGridColumns();
	}

	/// <summary>
	/// Property with the size of the segment in the grid
	/// </summary>
	public int SegmentSize { get; private set; }

	/// <summary>
	/// Property with the number of columns in the grid
	/// </summary>
	public int GridColumns { get; private set; }

	/// <summary>
	/// Property to set current snake speed
	/// </summary>
	private int _snakeSpeed;
	public int SnakeSpeed
	{
		get => _snakeSpeed;
		set
		{
			if (value is >= 1 and <= 9)
			{
				_snakeSpeed = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>
	/// Property to set current game grid size
	/// </summary>
	private int _gameGridSize;
	public int GameGridSize
	{
		get => _gameGridSize;
		set
		{
			_gameGridSize = value;
			SetGridColumns();
			OnPropertyChanged();
		}
	}

	/// <summary>
	/// Property to set current sound volume
	/// </summary>
	private int _soundVolume;
	public int SoundVolume
	{
		get => _soundVolume;
		set
		{
			if (value is >= 0 and <= 10)
			{
				_soundVolume = value;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>
	/// Property to set playfield border
	/// </summary>
	private bool _isFieldHaveBorder;
	public bool IsFieldHaveBorder
	{
		get => _isFieldHaveBorder;
		set { _isFieldHaveBorder = value; OnPropertyChanged(); }
	}

	private void SetStartValues()
	{
		SegmentSize = 20;
		_snakeSpeed = 1;
		_gameGridSize = 600;
		_soundVolume = 8;
		_isFieldHaveBorder = false;
	}

	private void SetGridColumns() => GridColumns = (_gameGridSize / SegmentSize);
}