using RoguelikeGame.ConsoleApp;

Game game = new();
await game.Start();

Console.SetCursorPosition(0, Levels.Level1.Length);