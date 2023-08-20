namespace Tetris.WinForms
{
	internal class Game
	{
		private const int _segmentSize = 20;
		private const int _shapeCount = 7;
		private const int _playfieldWidth = 200;
		private const int _playfieldHeight = 400;

		private readonly Random _random;
		private Rectangle _playfieldBackground;
		private Rectangle _playfield;
		private Rectangle _nextSegmentArea;
		private Rectangle _scoreArea;
		private int _score;
		private Shape? _actualShape;
		private Shape _nextShape;
		private List<Segment> _segments;

		private readonly System.Windows.Forms.Timer _gameTimer;
		private bool _isPaused;
		private bool _isGameOver;

		//public event Action? GameOver;
		public event Action? GameExit;

		public Game(Rectangle playfieldBackground)
		{
			_score = 0;
			_random = new();
			_segments = new();
			_isPaused = false;
			_playfieldBackground = playfieldBackground;

			_playfield = new(
				_playfieldBackground.Right / 2 - _playfieldWidth / 2,
				_playfieldBackground.Bottom / 2 - _playfieldHeight / 2,
				_playfieldWidth,
				_playfieldHeight);

			_nextSegmentArea = new(_playfield.Left - 120, _playfield.Top, 100, 100);
			_scoreArea = new(_playfield.Right + 20, _playfield.Top, 100, 100);

			_actualShape = GetNewShape(_playfield.Right - _playfield.Width / 2, _playfield.Top);
			_nextShape = GetNewShape();

			_gameTimer = new();
			_gameTimer.Interval = 500;
			_gameTimer.Tick += GameTimer_Tick;
		}

		internal void Start() =>
			_gameTimer.Start();

		internal void Stop() =>
			_gameTimer.Stop();

		private void GameTimer_Tick(object? sender, EventArgs e)
		{
			if (_actualShape == null)
			{
				_actualShape = _nextShape;
				_actualShape.MoveToLocation(_playfield.Right - _playfield.Width / 2, _playfield.Top);
				_nextShape = GetNewShape();
			}
			else
			{
				MoveDown();
			}
		}

		private Shape GetNewShape(int x, int y) =>
			new((ShapeKind)_random.Next(_shapeCount), x, y);

		private Shape GetNewShape()
		{
			//ShapeKind shapeKind = ShapeKind.J;
			ShapeKind shapeKind = (ShapeKind)_random.Next(_shapeCount);
			return shapeKind switch
			{
				ShapeKind.I => new(shapeKind, _nextSegmentArea.Left + _nextSegmentArea.Width / 2, _nextSegmentArea.Top + _nextSegmentArea.Height / 2 - _segmentSize / 2),
				ShapeKind.J => new(shapeKind, _nextSegmentArea.Left + _nextSegmentArea.Width / 2 - _segmentSize / 2, _nextSegmentArea.Top + _nextSegmentArea.Height / 2 - _segmentSize),
				ShapeKind.L => new(shapeKind, _nextSegmentArea.Left + _nextSegmentArea.Width / 2 - _segmentSize / 2, _nextSegmentArea.Top + _nextSegmentArea.Height / 2 - _segmentSize),
				ShapeKind.O => new(shapeKind, _nextSegmentArea.Left + _nextSegmentArea.Width / 2, _nextSegmentArea.Top + _nextSegmentArea.Height / 2 - _segmentSize),
				ShapeKind.S => new(shapeKind, _nextSegmentArea.Left + _nextSegmentArea.Width / 2 - _segmentSize / 2, _nextSegmentArea.Top + _nextSegmentArea.Height / 2 - _segmentSize),
				ShapeKind.T => new(shapeKind, _nextSegmentArea.Left + _nextSegmentArea.Width / 2 - _segmentSize / 2, _nextSegmentArea.Top + _nextSegmentArea.Height / 2 - _segmentSize),
				ShapeKind.Z => new(shapeKind, _nextSegmentArea.Left + _nextSegmentArea.Width / 2 - _segmentSize / 2, _nextSegmentArea.Top + _nextSegmentArea.Height / 2 - _segmentSize),
				_ => throw new ArgumentException("Nie ma takiego kształtu!"),
			};
		}



		#region region for pressed keys 

		internal void DoAction(Keys keyCode)
		{
			if (keyCode == Keys.Escape)
			{
				GameExit?.Invoke();
			}
			else if (keyCode == Keys.P)
			{
				SetPause();
			}

			if (!_isPaused && !_isGameOver && _actualShape != null)
			{
				if (keyCode == Keys.Space)
				{
					Rotate();
				}
				else if (keyCode == Keys.Left)
				{
					MoveLeft();
				}
				else if (keyCode == Keys.Right)
				{
					MoveRight();
				}
				else if (keyCode == Keys.Down)
				{
					MoveDown();
				}
			}
		}

		internal void SetPause()
		{
			_gameTimer.Enabled = !_gameTimer.Enabled;
			_isPaused = !_isPaused;
		}

		private void Rotate()
		{
			_actualShape.Rotate();
		}

		private void MoveLeft()
		{
			for (int i = 0; i < _actualShape!.Segments.Count; i++)
			{
				Point point = new(_actualShape.Segments[i].Location.X - _segmentSize, _actualShape.Segments[i].Location.Y);
				for (int j = 0; j < _segments.Count; j++)
				{
					if (point == _segments[j].Location)
					{
						return;
					}
				}
			}

			if (_actualShape.MinPosX > _playfield.Left)
			{
				_actualShape.Move(MoveDirection.Left);
			}
		}

		private void MoveRight()
		{
			foreach (var item in _actualShape!.Segments)
			{
				Point point = new(item.Location.X + _segmentSize, item.Location.Y);
				if (_segments.Any(s => s.Location == point))
				{
					return;
				}
			}

			if (_actualShape.MaxPosX < _playfield.Right - _segmentSize)
			{
				_actualShape.Move(MoveDirection.Right);
			}
		}

		private void MoveDown()
		{
			for (int i = 0; i < _actualShape!.Segments.Count; i++)
			{
				Point point = new(_actualShape.Segments[i].Location.X, _actualShape.Segments[i].Location.Y + _segmentSize);
				for (int j = 0; j < _segments.Count; j++)
				{
					if (point == _segments[j].Location)
					{
						if (_actualShape.MinPosY == _playfield.Top)
						{
							GameOver();
							return;
						}
						_segments.AddRange(_actualShape.Segments);
						_actualShape = null;
						CheckLines();
						return;
					}
				}
			}

			if (_actualShape.MaxPosY + _segmentSize == _playfield.Bottom)
			{
				_segments.AddRange(_actualShape.Segments);
				_actualShape = null;
				CheckLines();
				return;
			}

			_actualShape.Move(MoveDirection.Down);
		}

		private void GameOver()
		{
			_gameTimer.Stop();
			_isGameOver = true;
		}

		private void CheckLines()
		{
			int maxSegmentsInRow = _playfieldWidth / _segmentSize;

			for (int i = _segments.Min(s => s.Location.Y); i <= _segments.Max(s => s.Location.Y); i += _segmentSize)
			{
				if (maxSegmentsInRow == _segments.Count(s => s.Location.Y == i))
				{
					_score++;
					_segments.RemoveAll(s => s.Location.Y == i);
					var tmpList = _segments.FindAll(s => s.Location.Y < i);
					tmpList.ForEach(s => s.Location = new Point(s.Location.X, s.Location.Y + _segmentSize));
					_segments = _segments.FindAll(s => s.Location.Y > i);
					_segments.AddRange(tmpList);
				}
			}
		}

		#endregion



		#region drawing region

		internal void Draw(Graphics graphics)
		{
			DrawBackground(graphics);

			DrawNextShape(graphics);

			DrawActualShape(graphics);

			DrawSegments(graphics);

			DrawScore(graphics);

			if (_isPaused)
			{
				DrawPause(graphics);
			}

			if (_isGameOver)
			{
				DrawGameOver(graphics);
			}
		}

		private void DrawBackground(Graphics graphics)
		{
			// background
			graphics.FillRectangle(Brushes.DarkBlue, _playfieldBackground);

			// playfield border
			graphics.DrawRectangle(new Pen(Color.Yellow, 4f), _playfield.Left - 2, _playfield.Top - 2, _playfield.Width + 5, _playfield.Height + 5);

			// next segment border
			graphics.DrawRectangle(new Pen(Color.Yellow, 4f), _nextSegmentArea);

			// score border
			graphics.DrawRectangle(new Pen(Color.Yellow, 4f), _scoreArea);
		}

		private void DrawNextShape(Graphics graphics) =>
			_nextShape?.Draw(graphics);

		private void DrawActualShape(Graphics graphics) =>
			_actualShape?.Draw(graphics);

		private void DrawSegments(Graphics graphics) =>
			_segments.ForEach(segment => segment.Draw(graphics));

		private void DrawScore(Graphics graphics)
		{
			string message1 = "Wynik:";
			string message2 = _score.ToString();
			using (Font font = new("Arial", 16, FontStyle.Bold))
			{
				SizeF sizeMessage1 = graphics.MeasureString(message1, font);
				SizeF sizeMessage2 = graphics.MeasureString(message2, font);
				graphics.DrawString(message1, font, Brushes.Aqua, new Point(_scoreArea.Left + (_scoreArea.Width - (int)sizeMessage1.Width) / 2, _scoreArea.Top + 20));
				graphics.DrawString(message2, font, Brushes.Aqua, new Point(_scoreArea.Left + (_scoreArea.Width - (int)sizeMessage2.Width) / 2, _scoreArea.Top + 55));
			}
		}

		private void DrawPause(Graphics graphics)
		{
			string message = "PAUSE";
			using (Font font = new("Arial", 64, FontStyle.Bold))
			{
				SizeF sizePause = graphics.MeasureString(message, font);
				graphics.DrawString(message, font, Brushes.Red, new Point(_playfieldBackground.Right / 2 - (int)sizePause.Width / 2, _playfieldBackground.Bottom / 2 - (int)sizePause.Height / 2));
			}
		}

		private void DrawGameOver(Graphics graphics)
		{
			string message1 = "GAME OVER";
			string message2 = "ESC - wyjdź";
			using (Font font = new("Arial", 64, FontStyle.Bold))
			{
				SizeF size1 = graphics.MeasureString(message1, font);
				SizeF size2 = graphics.MeasureString(message2, font);
				graphics.DrawString(message1, font, Brushes.Red, new Point(_playfieldBackground.Right / 2 - (int)size1.Width / 2, _playfieldBackground.Bottom / 2 - (int)size1.Height / 2));
				graphics.DrawString(message2, font, Brushes.Red, new Point(_playfieldBackground.Right / 2 - (int)size2.Width / 2, _playfieldBackground.Bottom / 2 + (int)size2.Height / 2));
			}
		}

		#endregion
	}
}
