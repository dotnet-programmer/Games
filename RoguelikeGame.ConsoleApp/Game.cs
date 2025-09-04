namespace RoguelikeGame.ConsoleApp;

internal class Game
{
	//private readonly string[] _level = Levels.Level1;

	private readonly Player _player = new(1, 1, "@");
	private readonly Map _map = new();

	private bool _isGameOver;

	internal async Task StartAsync()
	{
		CreateNewPlayer();
		ShowWelcomeMessage();
		ReadKey();
		ClearConsole();
		await PlayIntroAsync();

		if (_isGameOver)
		{
			return;
		}

		ClearConsole();
		ShowMap();
		await PlayGameAsync();
	}

	private void CreateNewPlayer()
	{
		SetPlayerName();
		SetPlayerPlace();
	}

	private void SetPlayerName()
	{
		SetPrimaryColor();
		Console.WriteLine("Who are you, Stranger?");
		SetSecondaryColor();
		string? name = Console.ReadLine();
		SetPrimaryColor();
		if (string.IsNullOrWhiteSpace(name))
		{
			Console.WriteLine("I will still call you Stranger, then.");
			name = "Stranger";
		}
		else if (name.ToLower() == "stranger")
		{
			Console.WriteLine("Ha! I knew it!");
		}

		_player.Name = name;
	}

	private void SetPlayerPlace()
	{
		Console.Write($"{Environment.NewLine}Where are you from, ");
		SetSecondaryColor();
		Console.Write($"{_player.Name}");
		SetPrimaryColor();
		Console.WriteLine("?");
		SetSecondaryColor();
		string? place = Console.ReadLine();
		SetPrimaryColor();
		if (string.IsNullOrWhiteSpace(place))
		{
			Console.WriteLine("You are not too talkative, are you?");
			place = "Nowhere";
		}

		_player.Place = place;
	}

	private void ShowWelcomeMessage()
	{
		Console.Write($"{Environment.NewLine}{Environment.NewLine}Welcome to Roguelike Game, ");
		SetSecondaryColor();
		Console.Write($"{_player.Name}");
		SetPrimaryColor();
		Console.Write(" from ");
		SetSecondaryColor();
		Console.Write($"{_player.Place}");
		SetPrimaryColor();
		Console.WriteLine(".");
		Console.WriteLine($"{Environment.NewLine}Press any key to enter the dungeon and begin your adventure!");
	}

	private async Task PlayIntroAsync()
	{
		Console.WriteLine("You are in a dark cave, deep underground.");
		await Task.Delay(3000);
		Console.WriteLine("You walk slowly and carefully, but the echo of your steps carries far into the depths.");
		await Task.Delay(3000);
		Console.WriteLine("Every now and then, a strange, ominous growl reaches your ears.");
		await Task.Delay(3000);
		Console.WriteLine("You have reached the place where the cave split into 3 corridors.");
		await Task.Delay(3000);
		Console.WriteLine("In the faint light of the torch, on the side wall, you noticed a narrow passageway.");
		await Task.Delay(3000);
		Console.WriteLine("You walked closer to check it out and spotted an object!");
		await Task.Delay(3000);
		Console.WriteLine("Unfortunately, it lies too far away to reach it with your hand.");
		await Task.Delay(3000);
		Console.WriteLine("Do you want to take a chance and squeeze through the gap to pick it up? (Yes/No): ");

		string choice;
		do
		{
			choice = Console.ReadLine()!.ToUpper();
		} while (choice is not ("Y" or "YES" or "N" or "NO"));

		if (choice is "Y" or "YES")
		{
			Console.WriteLine("It was hard and you almost got stuck, but you made it!");
			await Task.Delay(3000);
			Console.WriteLine("To your eyes appeared an ancient scroll that looked like this:");
			await Task.Delay(3000);
			await ShowScrollAsync();
			await Task.Delay(1500);
			Console.WriteLine("Bravo! You have acquired a magical map of the underworld that will lead you to the hidden treasure!");
			_isGameOver = false;
		}
		else
		{
			Console.WriteLine("You trusted your intuition and moved on, choosing the first corridor.");
			await Task.Delay(3000);
			Console.WriteLine("The road was winding, leading constantly downward, and the disturbing murmur grew more and more intense.");
			await Task.Delay(3000);
			Console.WriteLine("After a long trek, you reached a huge underground lake!");
			await Task.Delay(3000);
			Console.WriteLine("You feel a penetrating chill, and a moment later a sudden gust of icy wind extinguish the torch.");
			await Task.Delay(3000);
			Console.WriteLine("Total darkness fell....");
			await Task.Delay(3000);
			Console.WriteLine("Something stirred the water....");
			await Task.Delay(3000);
			Console.WriteLine("You hear a quiet, steady clatter coming towards you!");
			await Task.Delay(3000);
			Console.WriteLine("You start to run away running blindly but hit a wall and fall over.");
			await Task.Delay(3000);
			Console.WriteLine("Before you could get up, the slimy tentacles of the monster managed to clamp down on your neck....");
			await Task.Delay(3000);
			SetGameOverColor();
			Console.WriteLine($"{Environment.NewLine}Crack!{Environment.NewLine}");
			await Task.Delay(3000);
			Console.WriteLine("GAME OVER!");
			_isGameOver = true;
		}

		ReadKey();
	}

	private async Task ShowScrollAsync()
	{
		Scroll scroll = new();
		int _scrollHalf = scroll.Length / 2;

		// draw the top part of the scroll
		for (int i = 0; i < _scrollHalf; i++)
		{
			Console.WriteLine(scroll[i]);
		}

		int nextMapRowPosition = Console.CursorTop;

		//foreach (string row in _level)
		foreach (string row in _map.GetMap)
		{
			Console.SetCursorPosition(0, nextMapRowPosition);
			Console.WriteLine($"       |   {row}   |");
			nextMapRowPosition++;

			// draw the bottom part of the scroll
			for (int i = _scrollHalf; i < scroll.Length; i++)
			{
				Console.WriteLine(scroll[i]);
			}

			await Task.Delay(300);
		}
	}

	private void ShowMap()
	{
		foreach (string row in _map.GetMap)
		{
			Console.WriteLine(row);
		}
	}

	private async Task PlayGameAsync()
	{
		while (true)
		{
			WritePlayerPosition();

			if (_map.IsExit(_player.Y, _player.X))
			{
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

	private void WritePlayerPosition()
		=> Display.WriteAt(_player.X, _player.Y, _player.Avatar);

	private void WriteFloorUnderPlayer()
		=> Display.WriteAt(_player.X, _player.Y, _map.GetMap[_player.Y][_player.X]);

	private void SetPrimaryColor()
	{
		Console.ForegroundColor = ConsoleColor.Green;
	}

	private void SetSecondaryColor()
	{
		Console.ForegroundColor = ConsoleColor.DarkYellow;
	}

	private void SetGameOverColor()
	{
		Console.ForegroundColor = ConsoleColor.Red;
	}

	private static ConsoleKeyInfo ReadKey(bool intercept = true)
		=> Console.ReadKey(intercept);

	private static void ClearConsole()
		=> Console.Clear();
}