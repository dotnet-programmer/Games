using Invaders.WinFormsApp.Properties;

namespace Invaders.WinFormsApp.Models;

internal class Shot
{
	private const int MoveInterval = 20;
	private const int ShotWidth = 10;
	private const int ShotHeight = 30;

	private static readonly Image _playerShot = ImageUtil.ResizeImage(Resources.player_shot, ShotWidth, ShotHeight);
	private static readonly Image _enemyShot = ImageUtil.ResizeImage(Resources.enemy_shot, ShotWidth, ShotHeight);

	private readonly MoveDirection _moveDirection;
	private readonly Rectangle _playfield;
	private readonly Image _image;

	public Shot(Point location, MoveDirection direction, Rectangle playfield)
	{
		_moveDirection = direction;
		_playfield = playfield;
		_image = direction == MoveDirection.Up ? _playerShot : _enemyShot;
		Location = new Point(location.X - _image.Width / 2, location.Y - _image.Height / 2);
	}

	public Point Location { get; private set; }

	public void Draw(Graphics graphics)
		=> graphics.DrawImageUnscaled(_image, Location);

	// Moves the projectile up/down and returns true if it is still in the game area
	public bool Move()
	{
		if (_moveDirection == MoveDirection.Up)
		{
			Location = new Point(Location.X, Location.Y - MoveInterval);
			return Location.Y > _playfield.Top;
		}
		else
		{
			Location = new Point(Location.X, Location.Y + MoveInterval);
			return Location.Y < _playfield.Bottom;
		}
	}
}