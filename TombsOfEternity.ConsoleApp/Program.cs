using TombsOfEternity.ConsoleApp;

Console.CursorVisible = false;
Console.Title = "Tombs Of Eternity";

Game game = new();
await game.StartAsync();

//Console.SetCursorPosition(0, Levels.Level1.Length);