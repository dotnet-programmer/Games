using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake.WpfApp.Models;

internal class Images
{
	public static readonly ImageSource Empty = LoadImage("Empty.png");
	public static readonly ImageSource Head = LoadImage("Head.png");
	public static readonly ImageSource Body = LoadImage("Body.png");
	public static readonly ImageSource Tail = LoadImage("Tail.png");
	public static readonly ImageSource Food = LoadImage("Food.png");
	public static readonly ImageSource DeadHead = LoadImage("DeadHead.png");
	public static readonly ImageSource DeadBody = LoadImage("DeadBody.png");
	public static readonly ImageSource DeadTail = LoadImage("DeadTail.png");

	private static BitmapImage LoadImage(string fileName)
		=> new(new Uri($"/Resources/Images/{fileName}", UriKind.Relative));
}