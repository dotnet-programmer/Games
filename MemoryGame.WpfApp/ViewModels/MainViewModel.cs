using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MemoryGame.WpfApp.Commands;

namespace MemoryGame.WpfApp.ViewModels;

internal class MainViewModel : BaseViewModel
{
	private readonly List<string> _randomAnimals = [];
	private readonly DispatcherTimer _timer = new();
	private readonly Random _random = new();
	private readonly IEnumerable<Button> _buttons;

	private Button _lastButtonClicked;
	private int _tenthsOfSecondsElapsed;
	private int _matchesFound;
	private bool _findingMatch;

	public MainViewModel(IEnumerable<Button> buttons)
	{
		_buttons = buttons;
		SetCommands();
		SetTimer();
		StartGame();
	}

	private string _timeElapsed;
	public string TimeElapsed
	{
		get => _timeElapsed;
		set { _timeElapsed = value; OnPropertyChanged(); }
	}

	public ICommand GridTileCommandAsync { get; private set; }
	public ICommand RestartGameCommand { get; private set; }

	private void SetCommands()
	{
		GridTileCommandAsync = new RelayCommandAsync(CheckIsMatchAsync, CanCheckIsMatch);
		RestartGameCommand = new RelayCommand(RestartGame, CanRestartGame);
	}

	private async Task CheckIsMatchAsync(object commandParameter)
	{
		if (commandParameter is Button button)
		{
			if (button.Content.ToString() != "?")
			{
				return;
			}

			int index = int.Parse(button.Name[3..]);

			if (!_findingMatch)
			{
				_lastButtonClicked = button;
				_lastButtonClicked.Content = _randomAnimals[index];
				_findingMatch = true;
			}
			else if (button != _lastButtonClicked)
			{
				button.Content = _randomAnimals[index];
				Application.Current.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle);

				if (_lastButtonClicked.Content == button.Content)
				{
					_matchesFound++;
					_findingMatch = false;
				}
				else
				{
					await Task.Delay(1000);

					button.Content = _lastButtonClicked.Content = "?";
					_findingMatch = false;
					_lastButtonClicked = null;
				}
			}
		}
	}

	private bool CanCheckIsMatch(object commandParameter)
		=> _matchesFound != 8;

	private void RestartGame(object commandParameter) 
		=> StartGame();

	private bool CanRestartGame(object commandParameter)
		=> _matchesFound == 8;

	private void SetTimer()
	{
		_timer.Interval = TimeSpan.FromMilliseconds(100);
		_timer.Tick += Timer_Tick;
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		_tenthsOfSecondsElapsed++;
		TimeElapsed = (_tenthsOfSecondsElapsed / 10d).ToString("0.0s");

		if (_matchesFound == 8)
		{
			_timer.Stop();
			TimeElapsed += " - Jeszcze raz?";
		}
	}

	private void StartGame()
	{
		SetUpGame();
		_timer.Start();
	}

	private void SetUpGame()
	{
		List<string> animals =
		[
			"🦇", "🦇",
			"🐅", "🐅",
			"🦥", "🦥",
			"🦔", "🦔",
			"🐢", "🐢",
			"🐘", "🐘",
			"🦭", "🦭",
			"🦀", "🦀",
		];

		_randomAnimals.Clear();

		foreach (var button in _buttons)
		{
			button.Content = "?";
			int index = _random.Next(animals.Count);
			_randomAnimals.Add(animals[index]);
			animals.RemoveAt(index);
		}

		_tenthsOfSecondsElapsed = _matchesFound = 0;
	}
}