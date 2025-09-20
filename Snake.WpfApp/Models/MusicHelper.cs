using System;
using System.Windows.Media;
using Snake.WpfApp.Models.Enums;

namespace Snake.WpfApp.Models;

internal class MusicHelper
{
	private readonly MediaPlayer _mediaPlayer = new();

	public MusicHelper()
		=> _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

	public void SetVolume(int soundVolume)
		=> _mediaPlayer.Volume = soundVolume / 10d;

	public void PlayMusic(MusicKind musicKind)
	{
		var fileName = musicKind switch
		{
			MusicKind.Menu => "menu",
			MusicKind.Game => "game",
			_ => throw new NotImplementedException(),
		};
		_mediaPlayer.Open(new Uri($"Resources/Sounds/{fileName}.wav", UriKind.Relative));
		_mediaPlayer.Play();
	}

	private void MediaPlayer_MediaEnded(object? sender, EventArgs e)
		=> _mediaPlayer.Position = TimeSpan.Zero;
}