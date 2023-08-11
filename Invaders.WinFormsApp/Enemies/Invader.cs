using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders.WinFormsApp.Enemies;
internal class Invader
{
	private const int _horizontalInterval = 10;
	private const int _verticalInterval = 40;

	private int _animationCell;
	private Bitmap[] _images;
	private readonly InvaderType _invaderType;

	public Point Location { get; private set; }
	public Point ShotLocation => new(Location.X + _images[_animationCell].Width / 2, Location.Y + _images[_animationCell].Height);
	public int Score { get; private set; }

	public Rectangle Area => new(Location, _images[_animationCell].Size);

	public Invader(InvaderType invaderType, Point location, int score)
	{
		_invaderType = invaderType;
		Location = location;
		Score = score;
		InitializeImages();
	}

	private void InitializeImages()
	{
		_images = new Bitmap[4];

		switch (_invaderType)
		{
			case InvaderType.Bug:
				_images[0] = ImageUtil.ResizeImage(Properties.Resources.Bug1, 40, 40);
				_images[1] = ImageUtil.ResizeImage(Properties.Resources.Bug2, 40, 40);
				_images[2] = ImageUtil.ResizeImage(Properties.Resources.Bug3, 40, 40);
				_images[3] = ImageUtil.ResizeImage(Properties.Resources.Bug4, 40, 40);
				break;
			case InvaderType.Saucer:
				_images[0] = ImageUtil.ResizeImage(Properties.Resources.Saucer1, 40, 40);
				_images[1] = ImageUtil.ResizeImage(Properties.Resources.Saucer2, 40, 40);
				_images[2] = ImageUtil.ResizeImage(Properties.Resources.Saucer3, 40, 40);
				_images[3] = ImageUtil.ResizeImage(Properties.Resources.Saucer4, 40, 40);
				break;
			case InvaderType.Satellite:
				_images[0] = ImageUtil.ResizeImage(Properties.Resources.Satellite1, 40, 40);
				_images[1] = ImageUtil.ResizeImage(Properties.Resources.Satellite2, 40, 40);
				_images[2] = ImageUtil.ResizeImage(Properties.Resources.Satellite3, 40, 40);
				_images[3] = ImageUtil.ResizeImage(Properties.Resources.Satellite4, 40, 40);
				break;
			case InvaderType.Spaceship:
				_images[0] = ImageUtil.ResizeImage(Properties.Resources.Spaceship1, 40, 40);
				_images[1] = ImageUtil.ResizeImage(Properties.Resources.Spaceship2, 40, 40);
				_images[2] = ImageUtil.ResizeImage(Properties.Resources.Spaceship3, 40, 40);
				_images[3] = ImageUtil.ResizeImage(Properties.Resources.Spaceship4, 40, 40);
				break;
			case InvaderType.Star:
				_images[0] = ImageUtil.ResizeImage(Properties.Resources.Star1, 40, 40);
				_images[1] = ImageUtil.ResizeImage(Properties.Resources.Star2, 40, 40);
				_images[2] = ImageUtil.ResizeImage(Properties.Resources.Star3, 40, 40);
				_images[3] = ImageUtil.ResizeImage(Properties.Resources.Star4, 40, 40);
				break;
			default:
				throw new ArgumentException("Nieprawidłowy typ statku!");
		}
	}

	// rysuje obrazek statku, wykorzystując właściwą klatkę animacji
	public void Draw(Graphics graphics, int animationCell)
	{
		_animationCell = animationCell;
		graphics.DrawImageUnscaled(_images[animationCell], Location);
	}

	// przemieszcza statek w określonym kierunku
	public void Move(MoveDirection direction)
	{
		Location = direction switch
		{
			MoveDirection.Left => new Point(Location.X - _horizontalInterval, Location.Y),
			MoveDirection.Right => new Point(Location.X + _horizontalInterval, Location.Y),
			MoveDirection.Down => new Point(Location.X, Location.Y + _verticalInterval),
			_ => Location
		};
	}
}
