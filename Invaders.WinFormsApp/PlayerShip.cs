using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Invaders.WinFormsApp.Properties;

namespace Invaders.WinFormsApp;
internal class PlayerShip
{
	private const int _horizontalInterval = 20;
	private Rectangle _playfield;
	private int _deadShipHeight;
	private readonly Bitmap _image;

	public Point Location { get; private set; }
	public Point ShotLocation => new(Location.X + _image.Width / 2, Location.Y);
	public Rectangle Area => new(Location, _image.Size);
	public bool IsAlive { get; set; }

	public PlayerShip(Rectangle playfield)
	{
		IsAlive = true;
		Location = new Point(playfield.Width / 2, playfield.Height - 50);
		_playfield = playfield;
		//_image = Resources.player;
		_image = ImageUtil.ResizeImage(Resources.player_ship, 60, 50);
		_deadShipHeight = _image.Height;
	}

	// rysuje statek we właściwym miejscu, chyba że gracz został zestrzelony, wtedy zastosuj animacje niszczenia statku
	public void Draw(Graphics graphics)
	{
		if (IsAlive)
		{
			graphics.DrawImageUnscaled(_image, Location);
		}
		else
		{
			if (_deadShipHeight > 1)
			{
				_deadShipHeight--;
				Location = new Point(Location.X, Location.Y + 1);
			}
			graphics.DrawImageUnscaled(ImageUtil.ResizeImage(_image, _image.Width, _deadShipHeight), Location);
		}
	}

	// przesuwa gracza we wskazanym kierunku
	public void Move(MoveDirection direction)
	{
		if (direction == MoveDirection.Left && Location.X > 20)
		{
			Location = new Point(Location.X - _horizontalInterval, Location.Y);
		}
		else if (direction == MoveDirection.Right && Location.X < _playfield.Right - 70)
		{
			Location = new Point(Location.X + _horizontalInterval, Location.Y);
		}
	}
}