using TombsOfEternity.ConsoleApp.Enums;

namespace TombsOfEternity.ConsoleApp;

internal class Game
{
	//private readonly string[] _level = Levels.Level1;

	private readonly Map _map = new();

	private Player _player = default!;
	private bool _isGameOver;

	internal async Task StartAsync()
	{
		await CreateNewPlayerAsync();
		await ShowWelcomeMessageAsync();
		await PlayIntroAsync();

		if (_isGameOver)
		{
			return;
		}

		await PlayGameAsync();
	}

	private async Task CreateNewPlayerAsync()
	{
		_player = new(1, 1, '@');
		await SetPlayerNameAsync();
		await SetPlayerPlaceAsync();
	}

	private async Task SetPlayerNameAsync()
	{
		await ExpandTextAsync("Who are you, Stranger?");
		string? name = GetInput();

		if (string.IsNullOrWhiteSpace(name))
		{
			await ExpandTextAsync("I will still call you Stranger, then.");
			name = "Stranger";
		}
		else if (name.ToLower() == "stranger")
		{
			await ExpandTextAsync("Ha! I knew it!");
		}

		_player.Name = name;
	}

	private async Task SetPlayerPlaceAsync()
	{
		Console.WriteLine();
		await ExpandTextAsync("Where are you from, ", addNewLine: false);
		await ExpandTextAsync(_player.Name, ColorType.Secondary, false);
		await ExpandTextAsync("?");
		string? place = GetInput();

		if (string.IsNullOrWhiteSpace(place))
		{
			await ExpandTextAsync("You are not too talkative, are you?");
			place = "Nowhere";
		}

		_player.Place = place;
	}

	private async Task ShowWelcomeMessageAsync()
	{
		Console.WriteLine();
		Console.WriteLine();
		await ExpandTextAsync("Welcome to Tombs Of Eternity, ", addNewLine: false);
		await ExpandTextAsync(_player.Name, ColorType.Secondary, false);
		await ExpandTextAsync(" from ", addNewLine: false);
		await ExpandTextAsync(_player.Place, ColorType.Secondary, false);
		await ExpandTextAsync(".");
		Console.WriteLine();
		await ExpandTextAsync("Press any key to enter the dungeon and begin your adventure!");
		ReadKey();
	}

	private async Task PlayIntroAsync()
	{
		ClearConsole();
		await ExpandTextAsync("You are in a dark cave, deep underground.", delay: 1000);
		await ExpandTextAsync("You walk slowly and carefully, but the echo of your steps carries far into the depths.", delay: 1000);
		await ExpandTextAsync("Every now and then, a strange, ominous growl reaches your ears.", delay: 1000);
		await ExpandTextAsync("You have reached the place where the cave split into 3 corridors.", delay: 1000);
		await ExpandTextAsync("In the faint light of the torch, on the side wall, you noticed a narrow passageway.", delay: 1000);
		await ExpandTextAsync("You walked closer to check it out and spotted an object!", delay: 1000);
		await ExpandTextAsync("Unfortunately, it lies too far away to reach it with your hand.", delay: 1000);
		await ExpandTextAsync("Do you want to take a chance and squeeze through the gap to pick it up? (Yes/No): ");

		string choice;
		do
		{
			choice = GetInput().ToUpper();
		} while (choice is not ("Y" or "YES" or "N" or "NO"));

		if (choice is "Y" or "YES")
		{
			await ExpandTextAsync("It was hard and you almost got stuck, but you made it!", delay: 1000);
			await ExpandTextAsync("To your eyes appeared an ancient scroll that looked like this:", delay: 1000);
			await ShowScrollAsync();
			await ExpandTextAsync("Bravo! You have acquired a magical map of the underworld that will lead you to the hidden treasure!", delay: 1000);
			_isGameOver = false;
		}
		else
		{
			await ExpandTextAsync("You trusted your intuition and moved on, choosing the first corridor.", delay: 1000);
			await ExpandTextAsync("The road was winding, leading constantly downward, and the disturbing murmur grew more and more intense.", delay: 1000);
			await ExpandTextAsync("After a long trek, you reached a huge underground lake!", delay: 1000);
			await ExpandTextAsync("You feel a penetrating chill, and a moment later a sudden gust of icy wind extinguish the torch.", delay: 1000);
			await ExpandTextAsync("Total darkness fell....", delay: 1000);
			await ExpandTextAsync("Something stirred the water....", delay: 1000);
			await ExpandTextAsync("You hear a quiet, steady clatter coming towards you!", delay: 1000);
			await ExpandTextAsync("You start to run away running blindly but hit a wall and fall over.", delay: 1000);
			await ExpandTextAsync("Before you could get up, the slimy tentacles of the monster managed to clamp down on your neck....", delay: 3000);
			Console.WriteLine();
			await ExpandTextAsync("Crack!", ColorType.GameOver, delay: 3000);
			Console.WriteLine();
			await ExpandTextAsync("GAME OVER!", ColorType.GameOver);
			_isGameOver = true;
		}

		ReadKey();
	}

	private async Task ExpandTextAsync(string text, ColorType colorType = ColorType.Primary, bool addNewLine = true, int delay = 0)
	{
		switch (colorType)
		{
			case ColorType.Primary:
				SetPrimaryColor();
				break;
			case ColorType.Secondary:
				SetSecondaryColor();
				break;
			case ColorType.GameOver:
				SetGameOverColor();
				break;
			default:
				SetPrimaryColor();
				break;
		}

		foreach (var letter in text)
		{
			Console.Write(letter);
			await Task.Delay(40);
		}

		if (addNewLine)
		{
			Console.WriteLine();
		}

		if (delay > 0)
		{
			await Task.Delay(delay);
		}
	}

	private string GetInput()
	{
		SetSecondaryColor();
		Console.Write("> ");
		return Console.ReadLine()!;
	}

	private async Task ShowScrollAsync()
	{
		Scroll scroll = new();
		int _scrollHalf = scroll.Length / 2;

		int mapRowPosition = 0;
		for (int i = 0; i < scroll.Length; i++)
		{
			Console.WriteLine(scroll[i]);
			if (i == _scrollHalf - 1)
			{
				mapRowPosition = Console.CursorTop;
			}
		}

		await Task.Delay(700);

		//foreach (string row in _level)
		foreach (string row in _map.GetMap)
		{
			Console.SetCursorPosition(0, mapRowPosition);
			Console.WriteLine($"       |   {row}   |");
			mapRowPosition++;

			// draw the bottom part of the scroll
			for (int i = _scrollHalf; i < scroll.Length; i++)
			{
				Console.WriteLine(scroll[i]);
			}

			await Task.Delay(200);
		}

		await Task.Delay(1000);
	}

	private async Task PlayGameAsync()
	{
		ClearConsole();
		ShowMap();

		while (true)
		{
			WritePlayerPosition();

			if (_map.IsExit(_player.Y, _player.X))
			{
				// TODO: usunąć czas, dodać normalne przechodzenie po wyjściu przez konkretne wyjście
				await Task.Delay(500);
				_map.ChangeLevel(_player);
				ClearConsole();
				ShowMap();
				WritePlayerPosition();
			}

			ConsoleKeyInfo keyInfo = ReadKey();

			WriteFloorUnderPlayer();

			int targetColumn = _player.X;
			int targetRow = _player.Y;

			switch (keyInfo.Key)
			{
				case ConsoleKey.LeftArrow:
					targetColumn = _player.X - 1;
					break;
				case ConsoleKey.RightArrow:
					targetColumn = _player.X + 1;
					break;
				case ConsoleKey.UpArrow:
					targetRow = _player.Y - 1;
					break;
				case ConsoleKey.DownArrow:
					targetRow = _player.Y + 1;
					break;
				default:
					return;
			}

			// sprawdzenie czy gracz w wejściu, jeśli tak to sprawdź czy wyszedł i zmień mapę
			//if (targetRow < 0 || targetRow >= Levels.Level1.Length)
			//{
			//	ClearConsole();

			//	//Levels.Level1 = Levels.Level2;
			//	player.X = 1;
			//	player.Y = 0;

			//	foreach (string row in level)
			//	{
			//		Console.WriteLine(row);
			//	}
			//}

			//if (targetColumn >= 0 && targetColumn < Levels.Level1[_player.Y].Length && Levels.Level1[_player.Y][targetColumn] != '#')
			//{
			//	_player.X = targetColumn;
			//}

			//if (targetRow >= 0 && targetRow < Levels.Level1.Length && Levels.Level1[targetRow][_player.X] != '#')
			//{
			//	_player.Y = targetRow;
			//}

			if (targetColumn >= 0 && targetColumn < _map.GetMap[_player.Y].Length && _map.GetMap[_player.Y][targetColumn] != '#')
			{
				_player.X = targetColumn;
			}

			if (targetRow >= 0 && targetRow < _map.GetMap.Length && _map.GetMap[targetRow][_player.X] != '#')
			{
				_player.Y = targetRow;
			}
		}
	}

	private void ShowMap()
	{
		foreach (string row in _map.GetMap)
		{
			Console.WriteLine(row);
		}
	}

	private void SetPrimaryColor()
		=> Console.ForegroundColor = ConsoleColor.DarkYellow;

	private void SetSecondaryColor()
		=> Console.ForegroundColor = ConsoleColor.Green;

	private void SetGameOverColor()
		=> Console.ForegroundColor = ConsoleColor.Red;

	private static ConsoleKeyInfo ReadKey(bool intercept = true)
		=> Console.ReadKey(intercept);

	private static void ClearConsole()
		=> Console.Clear();

	private void WritePlayerPosition()
		=> WriteAt(_player.X, _player.Y, _player.Sign);

	private void WriteFloorUnderPlayer()
		=> WriteAt(_player.X, _player.Y, _map.GetMap[_player.Y][_player.X]);

	private void WriteAt(int columnNumber, int rowNumber, char sign)
	{
		Console.SetCursorPosition(columnNumber, rowNumber);
		Console.Write(sign);
	}
}