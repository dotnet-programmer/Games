using System.IO;
using System.Text.Json;

namespace Snake.WpfApp.Models;

internal class FileHelper<T>(string filePath) where T : new()
{
	private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
	private readonly string _filePath = filePath;

	public void SerializeToJSON(T item)
	{
		var directoryName = Path.GetDirectoryName(_filePath);
		if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}

		var json = JsonSerializer.Serialize(item, _jsonOptions);
		File.WriteAllText(_filePath, json);
	}

	public T DeserializeFromJSON()
	{
		if (!File.Exists(_filePath))
		{
			return new T();
		}

		var json = File.ReadAllText(_filePath);
		return JsonSerializer.Deserialize<T>(json) ?? new T();
	}
}