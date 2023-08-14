using System.Windows.Media;
using Snake.WpfApp.Models.Enums;

namespace Snake.WpfApp.Models;

internal class Segment
{
	public Segment(int xPos, int yPos, int segmentSize, SegmentKind snakeSegmentKind)
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
		};
	}

	public int XPos { get; set; }
	public int YPos { get; set; }
	public int SegmentSize { get; set; }
	public ImageSource? ImageSource { get; set; }
	public double Rotation { get; set; }
}