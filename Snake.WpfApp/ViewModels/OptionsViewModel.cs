namespace Snake.WpfApp.ViewModels;

internal class OptionsViewModel : BaseViewModel
{
	private int _snakeSpeed;
	private int _gameGridSize;
	private int _soundVolume;
	private bool _isFieldHaveBorder;

	public OptionsViewModel()
	{
		SetStartValues();
		SetGridColumns();
	}

	public int SegmentSize { get; private set; }
	public int GridColumns { get; private set; }

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

	private void SetGridColumns()
		=> GridColumns = (_gameGridSize / SegmentSize);
}