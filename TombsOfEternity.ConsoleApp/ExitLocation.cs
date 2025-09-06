namespace TombsOfEternity.ConsoleApp;

internal class ExitLocation
{
	public ExitLocation(int x, int y)
	{
		X = x;
		Y = y;
	}

	public int X { get; set; }
	public int Y { get; set; }

	public int EntryLevel { get; set; }
	public int EntryNumber { get; set; }
}