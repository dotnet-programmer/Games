namespace RoguelikeGame.ConsoleApp;

public class Levels
{
	public static string[] Level0 =>
	[
		"#######",
		"#      ",
		"#  # ##",
		"# ##   ",
		"#  #  #",
		"## # ##"
	];

	public static string[] Level1 =>
	[
		"#### #",
		"#    #",
		"#    #",
		"#    #",
		"######"
	];

	public static string[] Level2 =>
	[
		"# ####",
		"#    #",
		"#    #",
		"#    #",
		"######"
	];

	public static string[][] GetLevels =>
	[
		[
			"#######",
			"#      ",
			"#  # ##",
			"# ##   ",
			"#  #  #",
			"## # ##"
		],
		[
			"#### #",
			"#    #",
			"#    #",
			"#    #",
			"######"
		],
		[
			"# ####",
			"#    #",
			"#    #",
			"#    #",
			"######"
		]
	];
}