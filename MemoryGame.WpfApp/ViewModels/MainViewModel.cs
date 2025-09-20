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
	private const int MaxMatchesCount = 8;

	private readonly List<string> _randomAnimals = [];
	private readonly DispatcherTimer _timer = new();
	private readonly Random _random = new();
	private readonly IEnumerable<Button> _buttons;

	private string? _timeElapsed;

	private Button? _lastButtonClicked;
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

	public string? TimeElapsed
	{
		get => _timeElapsed;
		set { _timeElapsed = value; OnPropertyChanged(); }
	}

	public ICommand GridTileCommandAsync { get; private set; } = null!;
	public ICommand RestartGameCommand { get; private set; } = null!;

	private void SetCommands()
	{
		GridTileCommandAsync = new RelayCommandAsync(CheckIsMatchAsync, CanCheckIsMatch);
		RestartGameCommand = new RelayCommand(RestartGame, CanRestartGame);
	}

	private async Task CheckIsMatchAsync(object? commandParameter)
	{
		if (commandParameter is Button button)
		{
			if (button.Content.ToString() != "?")
			{
				return;
			}

			if (!int.TryParse(button.Name[3..], out int index))
			{
				return;
			}

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
				_findingMatch = false;

				if (_lastButtonClicked!.Content == button.Content)
				{
					_matchesFound++;
				}
				else
				{
					await Task.Delay(1000);
					button.Content = _lastButtonClicked.Content = "?";
					_lastButtonClicked = null;
				}
			}
		}
	}

	private bool CanCheckIsMatch()
		=> _matchesFound != MaxMatchesCount;

	private void RestartGame()
		=> StartGame();

	private bool CanRestartGame()
		=> _matchesFound == MaxMatchesCount;

	private void SetTimer()
	{
		_timer.Interval = TimeSpan.FromMilliseconds(100);
		_timer.Tick += Timer_Tick;
	}

	private void Timer_Tick(object? sender, EventArgs e)
	{
		_tenthsOfSecondsElapsed++;
		TimeElapsed = (_tenthsOfSecondsElapsed / 10d).ToString("0.0s");

		if (_matchesFound == MaxMatchesCount)
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