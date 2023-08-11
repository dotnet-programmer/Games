using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Invaders.WinFormsApp.Properties;

namespace Invaders.WinFormsApp;
internal class Shot
{
	private static readonly Image _playerShot = ImageUtil.ResizeImage(Resources.player_shot, 10, 30);
	private static readonly Image _enemyShot = ImageUtil.ResizeImage(Resources.enemy_shot, 10, 30);

	private const int _moveInterval = 20;

	private readonly MoveDirection _moveDirection;
	private readonly Rectangle _playfield;

	private readonly Image _image;

	public Point Location { get; private set; }

	public Shot(Point location, MoveDirection direction, Rectangle playfield)
	{
		_moveDirection = direction;
		_playfield = playfield;
		_image = direction == MoveDirection.Up ? _playerShot : _enemyShot;
		Location = new Point(location.X - _image.Width / 2, location.Y - _image.Height / 2);
	}

	// rysuje pocisk
	public void Draw(Graphics graphics, Brush brush)
	{
		//graphics.FillRectangle(brush, Location.X, Location.Y, _width, _height);
		graphics.DrawImageUnscaled(_image, Location);
	}

	// przemieszcza pocisk w górę/dół i zwraca true, jeśli jest nadal w obszarze gry
	public bool Move()
	{
		if (_moveDirection == MoveDirection.Up)
		{
			Location = new Point(Location.X, Location.Y - _moveInterval);
			return Location.Y > 0;
		}
		else
		{
			Location = new Point(Location.X, Location.Y + _moveInterval);
			return Location.Y < _playfield.Bottom;
		}
	}
}
