using RoguelikeGame.ConsoleApp;

Console.CursorVisible = false;
Console.Title = "Rougelike game";

Game game = new();
await game.StartAsync();

//Console.SetCursorPosition(0, Levels.Level1.Length);