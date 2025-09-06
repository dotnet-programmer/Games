namespace TombsOfEternity.ConsoleApp;

internal class Player(int x, int y, char sign)
{
	public string Name { get; set; } = default!;
	public string Place { get; set; } = default!;

	public int X { get; set; } = x;
	public int Y { get; set; } = y;
	public char Sign { get; } = sign;
}