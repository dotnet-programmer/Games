namespace Tetris.WinForms
{
	internal class Segment
	{
		private readonly Brush _color;
		private readonly Pen _border;

		public int Size { get; private set; }
		public Point Location { get; set; }
		public Rectangle Area => new(Location.X, Location.Y, Size, Size);

		public Segment(int size, Point location, Brush color, Pen border)
		{
			Size = size;
			Location = location;
			_color = color;
			_border = border;
		}

		public void Draw(Graphics graphics)
		{
			graphics.FillRectangle(_color, Location.X, Location.Y, Size, Size);
			graphics.DrawRectangle(_border, Area);
		}
	}
}
