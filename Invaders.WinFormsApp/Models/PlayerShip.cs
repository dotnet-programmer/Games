using Invaders.WinFormsApp.Properties;

namespace Invaders.WinFormsApp.Models;

internal class PlayerShip
{
	private const int HorizontalInterval = 20;
	private const int ShipWidth = 60;
	private const int ShipHeight = 50;
	private const int SpeedOfDestruction = 1;

	private Rectangle _playfield;
	private int _deadShipHeight;

	public PlayerShip(Rectangle playfield)
	{
		IsAlive = true;
		_playfield = playfield;
		Location = new Point(playfield.Width / 2, playfield.Height - ShipHeight);
		Image = ImageUtil.ResizeImage(Resources.player_ship, ShipWidth, ShipHeight);
		_deadShipHeight = Image.Height;
	}

	public Point Location { get; private set; }
	public Point ShotLocation => new(Location.X + Image.Width / 2, Location.Y);
	public Rectangle Area => new(Location, Image.Size);
	public bool IsAlive { get; set; }
	public Bitmap Image { get; private set; }

	// Draws the ship in the right place, unless the player has been shot down, in which case apply ship destruction animations
	public void Draw(Graphics graphics)
	{
		if (IsAlive)
		{
			graphics.DrawImageUnscaled(Image, Location);
		}
		else
		{
			if (_deadShipHeight > SpeedOfDestruction)
			{
				_deadShipHeight -= SpeedOfDestruction;
				Location = new Point(Location.X, Location.Y + SpeedOfDestruction);
			}
			graphics.DrawImageUnscaled(ImageUtil.ResizeImage(Image, Image.Width, _deadShipHeight), Location);
		}
	}

	// Moves the player in a specific direction
	public void Move(MoveDirection direction)
	{
		if (direction == MoveDirection.Left && Location.X > HorizontalInterval)
		{
			Location = new Point(Location.X - HorizontalInterval, Location.Y);
		}
		else if (direction == MoveDirection.Right && Location.X < _playfield.Right - (ShipWidth + HorizontalInterval))
		{
			Location = new Point(Location.X + HorizontalInterval, Location.Y);
		}
	}
}