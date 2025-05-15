namespace RoguelikeGame.ConsoleApp;

internal class Game
{
	//private readonly string[] _level = Levels.Level1;
	private readonly int _scrollHalf = Scroll.MapScroll.Length / 2;

	private readonly Player _player = new(1, 1, "@");
	private readonly Map _map = new();

	private string? _name;
	private string? _place;

	internal async Task Start()
	{
		SetPlayerName();
		SetPlayerPlace();
		Console.Clear();
		await ShowWelcomeMessage();
		Console.Clear();
		ShowMap();
		await PlayGame();
	}

	private void SetPlayerName()
	{
		Console.WriteLine("Who are you, Stranger?");
		_name = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(_name))
		{
			Console.WriteLine("I will still call you Stranger, then.");
			_name = "Stranger";
		}
		else if (_name.ToLower() == "stranger")
		{
			Console.WriteLine("Ha! I knew it!");
		}
	}

	private void SetPlayerPlace()
	{
		Console.WriteLine($"{Environment.NewLine}Where are you from, {_name}?");
		_place = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(_place))
		{
			Console.WriteLine("You are not too talkative, are you?");
			_place = "Nowhere";
		}
	}

	private async Task ShowWelcomeMessage()
	{
		Console.WriteLine($"Welcome to Roguelike Game, {_name} from {_place}!");
		Console.WriteLine($"{Environment.NewLine}You are in a dark cave.");
		Console.WriteLine("You can see a scroll on the ground.");
		Console.WriteLine("Press any key to pick it up...");
		Console.ReadKey(true);
		await Task.Delay(500);
		Console.WriteLine(".");
		await Task.Delay(500);
		Console.WriteLine(".");
		await Task.Delay(500);
		Console.WriteLine(".");
		await Task.Delay(500);
		Console.WriteLine($"{Environment.NewLine}You picked up a scroll!");
		await Task.Delay(500);
		Console.WriteLine("It looks like this:");
		await Task.Delay(750);
		await ShowScrollAsync();
		await Task.Delay(500);
		Console.WriteLine($"{Environment.NewLine}Press any key to start the adventure...");
		Console.ReadKey(true);
	}

	private async Task ShowScrollAsync()
	{
		for (int i = 0; i < _scrollHalf; i++)
		{
			Console.WriteLine(Scroll.MapScroll[i]);
		}

		int nextMapRowPosition = Console.CursorTop;

		//foreach (string row in _level)
		foreach (string row in _map.GetMap)
		{
			Console.SetCursorPosition(0, nextMapRowPosition);
			Console.WriteLine($"       |   {row}   |");
			for (int i = _scrollHalf; i < Scroll.MapScroll.Length; i++)
			{
				Console.WriteLine(Scroll.MapScroll[i]);
			}
			nextMapRowPosition++;
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

	private async Task PlayGame()
	{
		while (true)
		{
			WritePlayerPosition();

			if (_map.IsExit(_player.Y, _player.X))
			{
				await Task.Delay(500);
				_map.ChangeLevel(_player);
				Console.Clear();
				ShowMap();
				WritePlayerPosition();
			}

			ConsoleKeyInfo keyInfo = Console.ReadKey(true);

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
			//	Console.Clear();

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
}
