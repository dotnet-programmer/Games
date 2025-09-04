namespace RoguelikeGame.ConsoleApp;

internal class Scroll
{
	private readonly string[] _scroll =
	[
		"     _______________",
		"()==(              (@==()",
		"     '______________'|",
		"       |             |",
		"       |             |",
		"     __)_____________|    ",
		"()==(               (@==()",
		"     '--------------'    ",
	];

	public int Length
		=> _scroll.Length;

	public string this[int index]
		=> _scroll[index];
}

/*
	https://ascii.co.uk/art/scroll
		 _______________
	()==(              (@==()
		 '______________'|
		   |             |
		   |             |
		 __)_____________|
	()==(               (@==()
		 '--------------'
					   PjP
*/