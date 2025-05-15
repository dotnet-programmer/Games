namespace RoguelikeGame.ConsoleApp;

internal class Map
{
	private readonly Level[] _levels = new Level[Levels.GetLevels.Length];
	private readonly List<ExitConnection> _connections = [];

	private int _actualLevel = 0;

	public Map()
	{
		for (int i = 0; i < Levels.GetLevels.Length; i++)
		{
			_levels[i] = new(Levels.GetLevels[i]);
			AddExitLocations(_levels[i]);
		}
		AddExitConnections();
	}

	public Level GetLevel 
		=> _levels[_actualLevel];

	public string[] GetMap 
		=> _levels[_actualLevel].Map;

	public bool IsExit(int x, int y)
	{
		foreach (var item in GetLevel.ExitLocations)
		{
			if (item.X == x && item.Y == y)
			{
				return true;
			}
		}
		return false;
	}

	public void ChangeLevel(Player player)
	{
		//for (int i = 0; i < GetLevel.ExitLocations.Count; i++)
		//{
		//	if (GetLevel.ExitLocations[i].X == player.Y && GetLevel.ExitLocations[i].Y == player.X)
		//	{
		//		ExitConnection exitConnection = _connections.Single(c => c.ExitLevel == _actualLevel && c.ExitNumber == i);
		//		_actualLevel = exitConnection.EntryLevel;
		//		ExitLocation exitLocation = GetLevel.ExitLocations[exitConnection.EntryNumber];
		//		player.X = exitLocation.Y;
		//		player.Y = exitLocation.X;
		//		return;
		//	}
		//}

		for (int i = 0; i < GetLevel.ExitLocations.Count; i++)
		{
			if (GetLevel.ExitLocations[i].X == player.Y && GetLevel.ExitLocations[i].Y == player.X)
			{
				ExitLocation exitLocation = GetLevel.ExitLocations[i];
				_actualLevel = exitLocation.EntryLevel;
				ExitLocation entryLocation = GetLevel.ExitLocations[exitLocation.EntryNumber];
				player.X = entryLocation.Y;
				player.Y = entryLocation.X;
				return;
			}
		}
	}

	private void AddExitLocations(Level level)
	{
		for (int i = 0; i < level.Map.Length; i++)
		{
			for (int j = 0; j < level.Map[i].Length; j++)
			{
				if (((i == 0 || i == level.Map.Length - 1) || (j == 0 || j == level.Map[i].Length - 1)) && level.Map[i][j] == ' ')
				{
					level.ExitLocations.Add(new(i, j));
				}
			}
		}
	}

	private void AddExitConnections()
	{
		//_connections.Add(new(0, 2, 1, 0));
		//_connections.Add(new(1, 0, 0, 2));

		_levels[0].ExitLocations[2].EntryLevel = 1;
		_levels[0].ExitLocations[2].EntryNumber = 0;

		_levels[1].ExitLocations[0].EntryLevel = 0;
		_levels[1].ExitLocations[0].EntryNumber = 2;
	}
}