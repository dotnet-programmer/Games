using Tetris.WinForms.Enums;

namespace Tetris.WinForms.Models;

internal class Shape
{
	private const int SegmentSize = 20;

	private SegmentPosition _position;

	public Shape(ShapeKind shapeKind, int posX, int posY)
	{
		Segments = [];
		ShapeKind = shapeKind;
		switch (ShapeKind)
		{
			case ShapeKind.I:
				_position = SegmentPosition.Horizontal;
				Segments.Add(new Segment(SegmentSize, new Point(posX - 2 * SegmentSize, posY), Brushes.Cyan, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY), Brushes.Cyan, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY), Brushes.Cyan, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX + SegmentSize, posY), Brushes.Cyan, Pens.Black));
				break;
			case ShapeKind.J:
				_position = SegmentPosition.Start;
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY), Brushes.Blue, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY), Brushes.Blue, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX + SegmentSize, posY), Brushes.Blue, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX + SegmentSize, posY + SegmentSize), Brushes.Blue, Pens.Black));
				break;
			case ShapeKind.L:
				_position = SegmentPosition.Start;
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY), Brushes.Orange, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY), Brushes.Orange, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX + SegmentSize, posY), Brushes.Orange, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY + SegmentSize), Brushes.Orange, Pens.Black));
				break;
			case ShapeKind.O:
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY), Brushes.Yellow, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY), Brushes.Yellow, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY + SegmentSize), Brushes.Yellow, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY + SegmentSize), Brushes.Yellow, Pens.Black));
				break;
			case ShapeKind.S:
				_position = SegmentPosition.Horizontal;
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY), Brushes.Green, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX + SegmentSize, posY), Brushes.Green, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY + SegmentSize), Brushes.Green, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY + SegmentSize), Brushes.Green, Pens.Black));
				break;
			case ShapeKind.T:
				_position = SegmentPosition.Start;
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY), Brushes.Cyan, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY + SegmentSize), Brushes.Cyan, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY + SegmentSize), Brushes.Cyan, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX + SegmentSize, posY + SegmentSize), Brushes.Cyan, Pens.Black));
				break;
			case ShapeKind.Z:
				_position = SegmentPosition.Horizontal;
				Segments.Add(new Segment(SegmentSize, new Point(posX - SegmentSize, posY), Brushes.Red, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY), Brushes.Red, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX, posY + SegmentSize), Brushes.Red, Pens.Black));
				Segments.Add(new Segment(SegmentSize, new Point(posX + SegmentSize, posY + SegmentSize), Brushes.Red, Pens.Black));
				break;
			default:
				throw new ArgumentException("Nie ma takiego kształtu!");
		}
	}

	internal List<Segment> Segments { get; }
	internal ShapeKind ShapeKind { get; }
	internal int MinPosX => Segments.Min(s => s.Location.X);
	internal int MaxPosX => Segments.Max(s => s.Location.X);
	internal int MinPosY => Segments.Min(s => s.Location.Y);
	internal int MaxPosY => Segments.Max(s => s.Location.Y);

	internal void Rotate()
	{
		switch (ShapeKind)
		{
			case ShapeKind.I:
				if (_position == SegmentPosition.Horizontal)
				{
					Segments[0].Location = new(Segments[0].Location.X + SegmentSize, Segments[0].Location.Y - SegmentSize);
					Segments[2].Location = new(Segments[2].Location.X - SegmentSize, Segments[2].Location.Y + SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X - 2 * SegmentSize, Segments[3].Location.Y + 2 * SegmentSize);
					_position = SegmentPosition.Vertical;
				}
				else
				{
					Segments[0].Location = new(Segments[0].Location.X - SegmentSize, Segments[0].Location.Y + SegmentSize);
					Segments[2].Location = new(Segments[2].Location.X + SegmentSize, Segments[2].Location.Y - SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X + 2 * SegmentSize, Segments[3].Location.Y - 2 * SegmentSize);
					_position = SegmentPosition.Horizontal;
				}
				break;
			case ShapeKind.J:
				if (_position == SegmentPosition.Start)
				{
					Segments[0].Location = new(Segments[0].Location.X + 2 * SegmentSize, Segments[0].Location.Y - SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X + SegmentSize, Segments[1].Location.Y);
					Segments[2].Location = new(Segments[2].Location.X, Segments[2].Location.Y + SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X - SegmentSize, Segments[3].Location.Y);
					_position = SegmentPosition.FirstTurn;
				}
				else if (_position == SegmentPosition.FirstTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X + SegmentSize, Segments[0].Location.Y + 2 * SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y + SegmentSize);
					Segments[2].Location = new(Segments[2].Location.X - SegmentSize, Segments[2].Location.Y);
					Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y - SegmentSize);
					_position = SegmentPosition.SecondTurn;
				}
				else if (_position == SegmentPosition.SecondTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X - 2 * SegmentSize, Segments[0].Location.Y + SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X - SegmentSize, Segments[1].Location.Y);
					Segments[2].Location = new(Segments[2].Location.X, Segments[2].Location.Y - SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X + SegmentSize, Segments[3].Location.Y);
					_position = SegmentPosition.ThirdTurn;
				}
				else if (_position == SegmentPosition.ThirdTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X - SegmentSize, Segments[0].Location.Y - 2 * SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y - SegmentSize);
					Segments[2].Location = new(Segments[2].Location.X + SegmentSize, Segments[2].Location.Y);
					Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y + SegmentSize);
					_position = SegmentPosition.Start;
				}
				break;
			case ShapeKind.L:
				if (_position == SegmentPosition.Start)
				{
					Segments[0].Location = new(Segments[0].Location.X + SegmentSize, Segments[0].Location.Y);
					Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y + SegmentSize);
					Segments[2].Location = new(Segments[2].Location.X - SegmentSize, Segments[2].Location.Y + 2 * SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y - SegmentSize);
					_position = SegmentPosition.FirstTurn;
				}
				else if (_position == SegmentPosition.FirstTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y + SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X - SegmentSize, Segments[1].Location.Y);
					Segments[2].Location = new(Segments[2].Location.X - 2 * SegmentSize, Segments[2].Location.Y - SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X + SegmentSize, Segments[3].Location.Y);
					_position = SegmentPosition.SecondTurn;
				}
				else if (_position == SegmentPosition.SecondTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X - SegmentSize, Segments[0].Location.Y);
					Segments[1].Location = new(Segments[1].Location.X, Segments[1].Location.Y - SegmentSize);
					Segments[2].Location = new(Segments[2].Location.X + SegmentSize, Segments[2].Location.Y - 2 * SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X, Segments[3].Location.Y + SegmentSize);
					_position = SegmentPosition.ThirdTurn;
				}
				else if (_position == SegmentPosition.ThirdTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y - SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X + SegmentSize, Segments[1].Location.Y);
					Segments[2].Location = new(Segments[2].Location.X + 2 * SegmentSize, Segments[2].Location.Y + SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X - SegmentSize, Segments[3].Location.Y);
					_position = SegmentPosition.Start;
				}
				break;
			case ShapeKind.O:
				break;
			case ShapeKind.S:
				if (_position == SegmentPosition.Horizontal)
				{
					Segments[0].Location = new(Segments[0].Location.X - SegmentSize, Segments[0].Location.Y + SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X - 2 * SegmentSize, Segments[1].Location.Y);
					Segments[2].Location = new(Segments[2].Location.X + SegmentSize, Segments[2].Location.Y + SegmentSize);
					_position = SegmentPosition.Vertical;
				}
				else
				{
					Segments[0].Location = new(Segments[0].Location.X + SegmentSize, Segments[0].Location.Y - SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X + 2 * SegmentSize, Segments[1].Location.Y);
					Segments[2].Location = new(Segments[2].Location.X - SegmentSize, Segments[2].Location.Y - SegmentSize);
					_position = SegmentPosition.Horizontal;
				}
				break;
			case ShapeKind.T:
				if (_position == SegmentPosition.Start)
				{
					Segments[0].Location = new(Segments[0].Location.X + SegmentSize, Segments[0].Location.Y + SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X + SegmentSize, Segments[1].Location.Y - SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X - SegmentSize, Segments[3].Location.Y + SegmentSize);
					_position = SegmentPosition.FirstTurn;
				}
				else if (_position == SegmentPosition.FirstTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X - SegmentSize, Segments[0].Location.Y + SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X + SegmentSize, Segments[1].Location.Y + SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X - SegmentSize, Segments[3].Location.Y - SegmentSize);
					_position = SegmentPosition.SecondTurn;
				}
				else if (_position == SegmentPosition.SecondTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X - SegmentSize, Segments[0].Location.Y - SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X - SegmentSize, Segments[1].Location.Y + SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X + SegmentSize, Segments[3].Location.Y - SegmentSize);
					_position = SegmentPosition.ThirdTurn;
				}
				else if (_position == SegmentPosition.ThirdTurn)
				{
					Segments[0].Location = new(Segments[0].Location.X + SegmentSize, Segments[0].Location.Y - SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X - SegmentSize, Segments[1].Location.Y - SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X + SegmentSize, Segments[3].Location.Y + SegmentSize);
					_position = SegmentPosition.Start;
				}
				break;
			case ShapeKind.Z:
				if (_position == SegmentPosition.Horizontal)
				{
					Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y + 2 * SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X - SegmentSize, Segments[1].Location.Y + SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X - SegmentSize, Segments[3].Location.Y - SegmentSize);
					_position = SegmentPosition.Vertical;
				}
				else
				{
					Segments[0].Location = new(Segments[0].Location.X, Segments[0].Location.Y - 2 * SegmentSize);
					Segments[1].Location = new(Segments[1].Location.X + SegmentSize, Segments[1].Location.Y - SegmentSize);
					Segments[3].Location = new(Segments[3].Location.X + SegmentSize, Segments[3].Location.Y + SegmentSize);
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
			Segments.ForEach(s => s.Location = new(s.Location.X - SegmentSize, s.Location.Y));
		}
		else if (moveDirection == MoveDirection.Right)
		{
			Segments.ForEach(s => s.Location = new(s.Location.X + SegmentSize, s.Location.Y));
		}
		else if (moveDirection == MoveDirection.Down)
		{
			Segments.ForEach(s => s.Location = new(s.Location.X, s.Location.Y + SegmentSize));
		}
	}

	internal void MoveToLocation(int posX, int posY)
	{
		switch (ShapeKind)
		{
			case ShapeKind.I:
				Segments[0].Location = new(posX - SegmentSize * 2, posY);
				Segments[1].Location = new(posX - SegmentSize, posY);
				Segments[2].Location = new(posX, posY);
				Segments[3].Location = new(posX + SegmentSize, posY);
				break;
			case ShapeKind.J:
				Segments[0].Location = new(posX - SegmentSize, posY);
				Segments[1].Location = new(posX, posY);
				Segments[2].Location = new(posX + SegmentSize, posY);
				Segments[3].Location = new(posX + SegmentSize, posY + SegmentSize);
				break;
			case ShapeKind.L:
				Segments[0].Location = new(posX - SegmentSize, posY);
				Segments[1].Location = new(posX, posY);
				Segments[2].Location = new(posX + SegmentSize, posY);
				Segments[3].Location = new(posX - SegmentSize, posY + SegmentSize);
				break;
			case ShapeKind.O:
				Segments[0].Location = new(posX - SegmentSize, posY);
				Segments[1].Location = new(posX, posY);
				Segments[2].Location = new(posX - SegmentSize, posY + SegmentSize);
				Segments[3].Location = new(posX, posY + SegmentSize);
				break;
			case ShapeKind.S:
				Segments[0].Location = new(posX, posY);
				Segments[1].Location = new(posX + SegmentSize, posY);
				Segments[2].Location = new(posX - SegmentSize, posY + SegmentSize);
				Segments[3].Location = new(posX, posY + SegmentSize);
				break;
			case ShapeKind.T:
				Segments[0].Location = new(posX, posY);
				Segments[1].Location = new(posX - SegmentSize, posY + SegmentSize);
				Segments[2].Location = new(posX, posY + SegmentSize);
				Segments[3].Location = new(posX + SegmentSize, posY + SegmentSize);
				break;
			case ShapeKind.Z:
				Segments[0].Location = new(posX - SegmentSize, posY);
				Segments[1].Location = new(posX, posY);
				Segments[2].Location = new(posX, posY + SegmentSize);
				Segments[3].Location = new(posX + SegmentSize, posY + SegmentSize);
				break;
			default:
				throw new ArgumentException("Nie ma takiego kształtu!");
		}
	}

	internal void Draw(Graphics graphics)
		=> Segments.ForEach(segment => segment.Draw(graphics));
}