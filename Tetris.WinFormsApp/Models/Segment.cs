namespace Tetris.WinForms.Models;

internal class Segment(int size, Point location, Brush color, Pen border)
{
	private readonly Brush _color = color;
	private readonly Pen _border = border;

	public int Size { get; private set; } = size;
	public Point Location { get; set; } = location;
	public Rectangle Area => new(Location.X, Location.Y, Size, Size);

	public void Draw(Graphics graphics)
	{
		graphics.FillRectangle(_color, Location.X, Location.Y, Size, Size);
		graphics.DrawRectangle(_border, Area);
	}
}