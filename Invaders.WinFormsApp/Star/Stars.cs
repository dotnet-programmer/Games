using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders.WinFormsApp.Star;
internal class Stars
{
	private readonly Random _random;
	private readonly Rectangle _playfield;

	private int _actualFrame;

	private static readonly List<Color> _colors = new() { Color.White, Color.Yellow, Color.Aquamarine, Color.Cyan, Color.Gainsboro };
	private readonly List<Pen> _pens;

	private readonly List<Star> _stars;

	public Stars(int numberOfStars, Rectangle playfield)
	{
		_random = new Random();
		_playfield = playfield;
		_actualFrame = 0;

		_pens = new List<Pen>(_colors.Count);
		for (int i = 0; i < _colors.Count; i++)
		{
			_pens.Add(new Pen(_colors[i], 1f));
		}

		_stars = new List<Star>(numberOfStars);
		for (int i = 0; i < numberOfStars; i++)
		{
			_stars.Add(new Star(GetRandomPoint(), GetRandomPen()));
		}
	}

	// zwraca losowy punkt dla gwiazdy
	private Point GetRandomPoint() => new(_random.Next(_playfield.Right), _random.Next(_playfield.Bottom));

	// zwraca losowy kolor za każdym razem gdy tworze nową gwiazdę
	private Pen GetRandomPen() => _pens[_random.Next(_pens.Count)];

	// rysuje gwiazdy
	public void Draw(Graphics graphics, int nextFrame)
	{
		if (_actualFrame != nextFrame)
		{
			_stars.ForEach(s => graphics.DrawRectangle(s.pen, s.point.X, s.point.Y, 1, 1));
			Twinkle();
			_actualFrame = nextFrame;
		}
	}

	// usuwa 5 gwiazd i wstawia 5 nowych - efekt mrugania gwiazd
	public void Twinkle()
	{
		for (int i = 0; i < 5; i++)
		{
			_stars.RemoveAt(_random.Next(_stars.Count));
		}

		for (int i = 0; i < 5; i++)
		{
			_stars.Add(new Star(GetRandomPoint(), GetRandomPen()));
		}
	}
}
