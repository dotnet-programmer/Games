namespace Invaders.WinFormsApp.Models.Star;

internal struct Star(Point point, Pen pen)
{
	public Point Point = point;
	public Pen Pen = pen;
}