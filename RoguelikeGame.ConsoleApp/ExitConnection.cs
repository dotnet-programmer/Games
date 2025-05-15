namespace RoguelikeGame.ConsoleApp;

internal class ExitConnection(int exitLevel, int exitNumber, int entryLevel, int entryNumber)
{
	public int ExitLevel { get; } = exitLevel;
	public int ExitNumber { get; } = exitNumber;
	public int EntryLevel { get; } = entryLevel;
	public int EntryNumber { get; } = entryNumber;
}