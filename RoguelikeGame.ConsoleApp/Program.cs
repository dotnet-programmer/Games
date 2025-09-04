using RoguelikeGame.ConsoleApp;

Game game = new();
await game.StartAsync();

Console.SetCursorPosition(0, Levels.Level1.Length);