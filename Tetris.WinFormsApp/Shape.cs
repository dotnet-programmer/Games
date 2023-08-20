namespace Tetris.WinForms
{
	internal class Shape
	{
		private const int _segmentSize = 20;
		private SegmentPosition _position;

		internal List<Segment> Segments { get; }
		internal ShapeKind ShapeKind { get; }
		internal int MinPosX => Segments.Min(s => s.Location.X);
		internal int MaxPosX => Segments.Max(s => s.Location.X);
		internal int MinPosY => Segments.Min(s => s.Location.Y);
		internal int MaxPosY => Segments.Max(s => s.Location.Y);

		public Shape(ShapeKind shapeKind, int posX, int posY)
		{
			Segments = new();
			ShapeKind = shapeKind;
			switch (ShapeKind)
			{
				case ShapeKind.I:
					_position = SegmentPosition.Horizontal;
					Segments.Add(new Segment(_segmentSize, new Point(posX - 2 * _segmentSize, posY), Brushes.Cyan, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY), Brushes.Cyan, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY), Brushes.Cyan, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX + _segmentSize, posY), Brushes.Cyan, Pens.Black));
					break;
				case ShapeKind.J:
					_position = SegmentPosition.Start;
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY), Brushes.Blue, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY), Brushes.Blue, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX + _segmentSize, posY), Brushes.Blue, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX + _segmentSize, posY + _segmentSize), Brushes.Blue, Pens.Black));
					break;
				case ShapeKind.L:
					_position = SegmentPosition.Start;
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY), Brushes.Orange, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY), Brushes.Orange, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX + _segmentSize, posY), Brushes.Orange, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY + _segmentSize), Brushes.Orange, Pens.Black));
					break;
				case ShapeKind.O:
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY), Brushes.Yellow, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY), Brushes.Yellow, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY + _segmentSize), Brushes.Yellow, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY + _segmentSize), Brushes.Yellow, Pens.Black));
					break;
				case ShapeKind.S:
					_position = SegmentPosition.Horizontal;
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY), Brushes.Green, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX + _segmentSize, posY), Brushes.Green, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY + _segmentSize), Brushes.Green, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY + _segmentSize), Brushes.Green, Pens.Black));
					break;
				case ShapeKind.T:
					_position = SegmentPosition.Start;
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY), Brushes.Cyan, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY + _segmentSize), Brushes.Cyan, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY + _segmentSize), Brushes.Cyan, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX + _segmentSize, posY + _segmentSize), Brushes.Cyan, Pens.Black));
					break;
				case ShapeKind.Z:
					_position = SegmentPosition.Horizontal;
					Segments.Add(new Segment(_segmentSize, new Point(posX - _segmentSize, posY), Brushes.Red, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY), Brushes.Red, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX, posY + _segmentSize), Brushes.Red, Pens.Black));
					Segments.Add(new Segment(_segmentSize, new Point(posX + _segmentSize, posY + _segmentSize), Brushes.Red, Pens.Black));
					break;
				default:
					throw new ArgumentException("Nie ma takiego kształtu!");
			}
		}

		internal void Rotate()
		{
			switch (ShapeKind)
			{
				case ShapeKind.I:
					if (_position == SegmentPosition.Horizontal)
					{
						Segments[0].Location = new(Segments[0].Location.X + _segmentSize, Segments[0].Location.Y - _segmentSize);
						Segments[2].Location = new(Segments[2].Location.X - _segmentSize, Segments[2].Location.Y + _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X - 2 * _segmentSize, Segments[3].Location.Y + 2 * _segmentSize);
						_position = SegmentPosition.Vertical;
					}
					else
					{
						Segments[0].Location = new(Segments[0].Location.X - _segmentSize, Segments[0].Location.Y + _segmentSize);
						Segments[2].Location = new(Segments[2].Location.X + _segmentSize, Segments[2].Location.Y - _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X + 2 * _segmentSize, Segments[3].Location.Y - 2 * _segmentSize);
						_position = SegmentPosition.Horizontal;
					}
					break;
				case ShapeKind.J:
					if (_position == SegmentPosition.Start)
					{
						Segments[0].Location = new(Segments[0].Location.X + 2 * _segmentSize, Segments[0].Location.Y - _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X + _segmentSize, Segments[1].Location.Y);
						Segments[2].Location = new(Segments[2].Location.X, Segments[2].Location.Y + _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X - _segmentSize, Segments[3].Location.Y);
						_position = SegmentPosition.FirstTurn;
					}
					else if (_position == SegmentPosition.FirstTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X + _segmentSize, Segments[0].Location.Y + 2 * _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y + _segmentSize);
						Segments[2].Location = new(Segments[2].Location.X - _segmentSize, Segments[2].Location.Y);
						Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y - _segmentSize);
						_position = SegmentPosition.SecondTurn;
					}
					else if (_position == SegmentPosition.SecondTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X - 2 * _segmentSize, Segments[0].Location.Y + _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X - _segmentSize, Segments[1].Location.Y);
						Segments[2].Location = new(Segments[2].Location.X, Segments[2].Location.Y - _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X + _segmentSize, Segments[3].Location.Y);
						_position = SegmentPosition.ThirdTurn;
					}
					else if (_position == SegmentPosition.ThirdTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X - _segmentSize, Segments[0].Location.Y - 2 * _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y - _segmentSize);
						Segments[2].Location = new(Segments[2].Location.X + _segmentSize, Segments[2].Location.Y);
						Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y + _segmentSize);
						_position = SegmentPosition.Start;
					}
					break;
				case ShapeKind.L:
					if (_position == SegmentPosition.Start)
					{
						Segments[0].Location = new(Segments[0].Location.X + _segmentSize, Segments[0].Location.Y);
						Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y + _segmentSize);
						Segments[2].Location = new(Segments[2].Location.X - _segmentSize, Segments[2].Location.Y + 2 * _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y - _segmentSize);
						_position = SegmentPosition.FirstTurn;
					}
					else if (_position == SegmentPosition.FirstTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y + _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X - _segmentSize, Segments[1].Location.Y);
						Segments[2].Location = new(Segments[2].Location.X - 2 * _segmentSize, Segments[2].Location.Y - _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X + _segmentSize, Segments[3].Location.Y);
						_position = SegmentPosition.SecondTurn;
					}
					else if (_position == SegmentPosition.SecondTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X - _segmentSize, Segments[0].Location.Y);
						Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y - _segmentSize);
						Segments[2].Location = new(Segments[2].Location.X + _segmentSize, Segments[2].Location.Y - 2 * _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y + _segmentSize);
						_position = SegmentPosition.ThirdTurn;
					}
					else if (_position == SegmentPosition.ThirdTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y - _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X + _segmentSize, Segments[1].Location.Y);
						Segments[2].Location = new(Segments[2].Location.X + 2 * _segmentSize, Segments[2].Location.Y + _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X - _segmentSize, Segments[3].Location.Y);
						_position = SegmentPosition.Start;
					}
					break;
				case ShapeKind.O:
					break;
				case ShapeKind.S:
					if (_position == SegmentPosition.Horizontal)
					{
						Segments[0].Location = new(Segments[0].Location.X - _segmentSize, Segments[0].Location.Y + _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X - 2 * _segmentSize, Segments[1].Location.Y);
						Segments[2].Location = new(Segments[2].Location.X + _segmentSize, Segments[2].Location.Y + _segmentSize);
						_position = SegmentPosition.Vertical;
					}
					else
					{
						Segments[0].Location = new(Segments[0].Location.X + _segmentSize, Segments[0].Location.Y - _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X + 2 * _segmentSize, Segments[1].Location.Y);
						Segments[2].Location = new(Segments[2].Location.X - _segmentSize, Segments[2].Location.Y - _segmentSize);
						_position = SegmentPosition.Horizontal;
					}
					break;
				case ShapeKind.T:
					if (_position == SegmentPosition.Start)
					{
						Segments[0].Location = new(Segments[0].Location.X + _segmentSize, Segments[0].Location.Y + _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X + _segmentSize, Segments[1].Location.Y - _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X - _segmentSize, Segments[3].Location.Y + _segmentSize);
						_position = SegmentPosition.FirstTurn;
					}
					else if (_position == SegmentPosition.FirstTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X - _segmentSize, Segments[0].Location.Y + _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X + _segmentSize, Segments[1].Location.Y + _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X - _segmentSize, Segments[3].Location.Y - _segmentSize);
						_position = SegmentPosition.SecondTurn;
					}
					else if (_position == SegmentPosition.SecondTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X - _segmentSize, Segments[0].Location.Y - _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X - _segmentSize, Segments[1].Location.Y + _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X + _segmentSize, Segments[3].Location.Y - _segmentSize);
						_position = SegmentPosition.ThirdTurn;
					}
					else if (_position == SegmentPosition.ThirdTurn)
					{
						Segments[0].Location = new(Segments[0].Location.X + _segmentSize, Segments[0].Location.Y - _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X - _segmentSize, Segments[1].Location.Y - _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X + _segmentSize, Segments[3].Location.Y + _segmentSize);
						_position = SegmentPosition.Start;
					}
					break;
				case ShapeKind.Z:
					if (_position == SegmentPosition.Horizontal)
					{
						Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y + 2 * _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X - _segmentSize, Segments[1].Location.Y + _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X - _segmentSize, Segments[3].Location.Y - _segmentSize);
						_position = SegmentPosition.Vertical;
					}
					else
					{
						Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y - 2 * _segmentSize);
						Segments[1].Location = new(Segments[1].Location.X + _segmentSize, Segments[1].Location.Y - _segmentSize);
						Segments[3].Location = new(Segments[3].Location.X + _segmentSize, Segments[3].Location.Y + _segmentSize);
						_position = SegmentPosition.Horizontal;
					}
					break;
				default:
					throw new ArgumentException("Nie ma takiego kształtu!");
			}
		}

		public void Move(MoveDirection moveDirection)
		{
			if (moveDirection == MoveDirection.Left)
			{
				Segments.ForEach(s => s.Location = new(s.Location.X - _segmentSize, s.Location.Y));
			}
			else if (moveDirection == MoveDirection.Right)
			{
				Segments.ForEach(s => s.Location = new(s.Location.X + _segmentSize, s.Location.Y));
			}
			else if (moveDirection == MoveDirection.Down)
			{
				Segments.ForEach(s => s.Location = new(s.Location.X, s.Location.Y + _segmentSize));
			}
		}

		internal void MoveToLocation(int posX, int posY)
		{
			switch (ShapeKind)
			{
				case ShapeKind.I:
					Segments[0].Location = new(posX - _segmentSize * 2, posY);
					Segments[1].Location = new(posX - _segmentSize, posY);
					Segments[2].Location = new(posX, posY);
					Segments[3].Location = new(posX + _segmentSize, posY);
					break;
				case ShapeKind.J:
					Segments[0].Location = new(posX - _segmentSize, posY);
					Segments[1].Location = new(posX, posY);
					Segments[2].Location = new(posX + _segmentSize, posY);
					Segments[3].Location = new(posX + _segmentSize, posY + _segmentSize);
					break;
				case ShapeKind.L:
					Segments[0].Location = new(posX - _segmentSize, posY);
					Segments[1].Location = new(posX, posY);
					Segments[2].Location = new(posX + _segmentSize, posY);
					Segments[3].Location = new(posX - _segmentSize, posY + _segmentSize);
					break;
				case ShapeKind.O:
					Segments[0].Location = new(posX - _segmentSize, posY);
					Segments[1].Location = new(posX, posY);
					Segments[2].Location = new(posX - _segmentSize, posY + _segmentSize);
					Segments[3].Location = new(posX, posY + _segmentSize);
					break;
				case ShapeKind.S:
					Segments[0].Location = new(posX, posY);
					Segments[1].Location = new(posX + _segmentSize, posY);
					Segments[2].Location = new(posX - _segmentSize, posY + _segmentSize);
					Segments[3].Location = new(posX, posY + _segmentSize);
					break;
				case ShapeKind.T:
					Segments[0].Location = new(posX, posY);
					Segments[1].Location = new(posX - _segmentSize, posY + _segmentSize);
					Segments[2].Location = new(posX, posY + _segmentSize);
					Segments[3].Location = new(posX + _segmentSize, posY + _segmentSize);
					break;
				case ShapeKind.Z:
					Segments[0].Location = new(posX - _segmentSize, posY);
					Segments[1].Location = new(posX, posY);
					Segments[2].Location = new(posX, posY + _segmentSize);
					Segments[3].Location = new(posX + _segmentSize, posY + _segmentSize);
					break;
				default:
					throw new ArgumentException("Nie ma takiego kształtu!");
			}
		}

		internal void Draw(Graphics graphics) =>
			Segments.ForEach(segment => segment.Draw(graphics));
	}
}
