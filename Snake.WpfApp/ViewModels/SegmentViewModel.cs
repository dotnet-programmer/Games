using System.Windows.Media;
using Snake.WpfApp.Models;
using Snake.WpfApp.Models.Enums;

namespace Snake.WpfApp.ViewModels;

internal class SegmentViewModel : BaseViewModel
{
	private ImageSource? _imageSource;
	private double _rotation;

	public SegmentViewModel(int xPos, int yPos, int segmentSize, SegmentKind snakeSegmentKind)
	{
		XPos = xPos;
		YPos = yPos;
		SegmentSize = segmentSize;
		ImageSource = snakeSegmentKind switch
		{
			SegmentKind.Head => Images.Head,
			SegmentKind.Body => Images.Body,
			SegmentKind.Tail => Images.Tail,
			SegmentKind.Food => Images.Food,
			_ => throw new System.NotImplementedException(),
		};
	}

	public int XPos { get; set; }
	public int YPos { get; set; }
	public int SegmentSize { get; set; }

	public double Rotation
	{
		get => _rotation;
		set { _rotation = value; OnPropertyChanged(); }
	}

	public ImageSource? ImageSource
	{
		get => _imageSource;
		set { _imageSource = value; OnPropertyChanged(); }
	}
}