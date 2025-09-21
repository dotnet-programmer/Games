namespace Invaders.WinFormsApp.Models.Star;

internal class Stars
{
	private const int NumberOfBlinkingStars = 5;
	private const int StarWidth = 1;
	private const int StarHeight = 1;
	private const float PenWidth = 1f;

	private static readonly List<Color> _colors = [Color.White, Color.Yellow, Color.Aquamarine, Color.Cyan, Color.Gainsboro];

	private readonly List<Pen> _pens;
	private readonly List<Star> _stars;
	private readonly Random _random;
	private readonly Rectangle _playfield;

	private int _actualFrame;

	public Stars(int numberOfStars, Rectangle playfield)
	{
		_random = new();
		_playfield = playfield;
		_actualFrame = 0;

		_pens = new(_colors.Count);
		for (int i = 0; i < _colors.Count; i++)
		{
			_pens.Add(new Pen(_colors[i], PenWidth));
		}

		_stars = new(numberOfStars);
		for (int i = 0; i < numberOfStars; i++)
		{
			_stars.Add(new Star(GetRandomPoint(), GetRandomPen()));
		}
	}

	public void Draw(Graphics graphics, int nextFrame)
	{
		if (_actualFrame != nextFrame)
		{
			_stars.ForEach(s => graphics.DrawRectangle(s.Pen, s.Point.X, s.Point.Y, StarWidth, StarHeight));
			Twinkle();
			_actualFrame = nextFrame;
		}
	}

	// Removes 5 stars and inserts 5 new ones - blinking star effect
	private void Twinkle()
	{
		for (int i = 0; i < NumberOfBlinkingStars; i++)
		{
			_stars.RemoveAt(_random.Next(_stars.Count));
		}

		for (int i = 0; i < NumberOfBlinkingStars; i++)
		{
			_stars.Add(new Star(GetRandomPoint(), GetRandomPen()));
		}
	}

	private Point GetRandomPoint()
		=> new(_random.Next(_playfield.Right), _random.Next(_playfield.Bottom));

	private Pen GetRandomPen()
		=> _pens[_random.Next(_pens.Count)];
}