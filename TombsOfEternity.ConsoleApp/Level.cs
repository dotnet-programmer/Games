namespace TombsOfEternity.ConsoleApp;

internal class Level(string[] map)
{
	public string[] Map { get; } = map;

	public List<ExitLocation> ExitLocations { get; } = [];
}