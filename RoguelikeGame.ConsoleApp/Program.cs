using RoguelikeGame.ConsoleApp;

Console.WriteLine("Who are you, Stranger?");
string? name = Console.ReadLine();
if (string.IsNullOrWhiteSpace(name))
{
	Console.WriteLine("I will still call you Stranger, then.");
	name = "Stranger";
}
else if (name.ToLower() == "stranger")
{
	Console.WriteLine("Ha! I knew it!");
}

Console.WriteLine($"{Environment.NewLine}Where are you from, {name}?");
string? place = Console.ReadLine();
if (string.IsNullOrWhiteSpace(place))
{
	Console.WriteLine("You are not too talkative, are you?");
	place = "Nowhere";
}

Console.WriteLine($"{Environment.NewLine}Welcome to Roguelike Game, {name} from {place}!");
Console.Write("\nPress any key to see the map...");
Console.ReadKey(true);

string[] level =
{
	"#########",
	"#    #  #",
	"#   ##  #",
	"#    #  #",
	"#    #  #",
	"#       #",
	"#    #  #",
	"#########"
};

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

string[] scroll =
{
	"     _______________",
	"()==(              (@==()",
	"     '______________'|",
	"       |             |",
	"       |             |",
	"     __)_____________|    ",
	"()==(               (@==()",
	"     '--------------'    ",
};

Console.Clear();
Console.WriteLine("Wanna see the map? Press any key until it is revealed...");

int scrollHalf = scroll.Length / 2;
for (int i = 0; i < scrollHalf; i++)
{
	Console.WriteLine(scroll[i]);
}

int nextMapRowPosition = Console.CursorTop;
foreach (string row in level)
{

	Console.SetCursorPosition(0, nextMapRowPosition);
	Console.WriteLine($"       |  {row}  |");

	for (int i = scrollHalf; i < scroll.Length; i++)
	{
		Console.WriteLine(scroll[i]);
	}
	nextMapRowPosition++;
	await Task.Delay(250);
}

Console.ReadKey(true);
Console.WriteLine("Press any key to start.");
Console.Clear();

foreach (string row in level)
{
	Console.WriteLine(row);
}

Player player = new(2, 3, "@");

while (true)
{
	Display.WriteAt(player.X, player.Y, player.Avatar);
	ConsoleKeyInfo keyInfo = Console.ReadKey(true);
	Display.WriteAt(player.X, player.Y, level[player.Y][player.X]);

	int targetColumn = player.X;
	int targetRow = player.Y;

	if (keyInfo.Key == ConsoleKey.LeftArrow)
	{
		targetColumn = player.X - 1;
	}
	else if (keyInfo.Key == ConsoleKey.RightArrow)
	{
		targetColumn = player.X + 1;
	}
	else if (keyInfo.Key == ConsoleKey.UpArrow)
	{
		targetRow = player.Y - 1;
	}
	else if (keyInfo.Key == ConsoleKey.DownArrow)
	{
		targetRow = player.Y + 1;
	}
	else
	{
		break;
	}

	if (targetColumn >= 0 && targetColumn < level[player.Y].Length && level[player.Y][targetColumn] != '#')
	{
		player.X = targetColumn;
	}

	if (targetRow >= 0 && targetRow < level.Length && level[targetRow][player.X] != '#')
	{
		player.Y = targetRow;
	}
}

Console.SetCursorPosition(0, level.Length);