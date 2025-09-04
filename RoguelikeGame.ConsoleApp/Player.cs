namespace RoguelikeGame.ConsoleApp;

internal class Player(int x, int y, string avatar)
{
	public string? Name { get; set; }
	public string? Place { get; set; }

	public int X { get; set; } = x;
	public int Y { get; set; } = y;
	public string Avatar { get; } = avatar;
}