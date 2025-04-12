namespace RoguelikeGame.ConsoleApp;

internal class Player(int x, int y, string avatar)
{
	public int X { get; set; } = x;
	public int Y { get; set; } = y;
	public string Avatar { get; set; } = avatar;
}