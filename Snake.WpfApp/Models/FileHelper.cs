using System.IO;
using System.Text.Json;

namespace Snake.WpfApp.Models;

internal class FileHelper<T> where T : new()
{
	private readonly string _filePath;

	public FileHelper(string filePath) => _filePath = filePath;

	public void SerializeToJSON(T item)
	{
		var directoryName = Path.GetDirectoryName(_filePath);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}

		var json = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(_filePath, json);
	}

	public T DeserializeFromJSON()
	{
		if (!File.Exists(_filePath))
		{
			return new T();
		}

		var json = File.ReadAllText(_filePath);
		return JsonSerializer.Deserialize<T>(json);
	}
}