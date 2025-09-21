using System.Resources;

namespace Invaders.WinFormsApp.Models.Enemies;

internal class Invader
{
	public const int InvaderWidth = 40;
	public const int InvaderHeight = 40;

	private const int HorizontalInterval = 10;
	private const int VerticalInterval = 40;
	private const int ImagesCount = 4;

	private readonly Bitmap[] _images = new Bitmap[ImagesCount];
	private readonly InvaderType _invaderType;
	private readonly ResourceSet? _resourceSet = Properties.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentUICulture, true, true);

	private int _animationCell;

	public Invader(InvaderType invaderType, Point location, int score)
	{
		_invaderType = invaderType;
		Location = location;
		Score = score;
		InitializeImages();
	}

	public int Score { get; private set; }
	public Point Location { get; private set; }
	public Point ShotLocation => new(Location.X + _images[_animationCell].Width / 2, Location.Y + _images[_animationCell].Height);
	public Rectangle Area => new(Location, _images[_animationCell].Size);

	private void InitializeImages()
	{
		var invaderImages = GetInvaderImagesFromResources(_invaderType.ToString());

		if (invaderImages.Count != 0)
		{
			for (int i = 0; i < ImagesCount; i++)
			{
				_images[i] = ImageUtil.ResizeImage(invaderImages[i], InvaderHeight, InvaderWidth);
			}
		}
	}

	private List<Bitmap> GetInvaderImagesFromResources(string invaderName)
		=> _resourceSet != null
			? _resourceSet
				.OfType<System.Collections.DictionaryEntry>()
				.Where(r => r.Key.ToString()!.StartsWith(invaderName))
				.OrderBy(r => r.Key.ToString())
				.Select(r => (Bitmap)r.Value!)
				.ToList()
			: [];

	// Draws a picture of a ship, using the correct frame of animation
	public void Draw(Graphics graphics, int animationCell)
	{
		_animationCell = animationCell;
		graphics.DrawImageUnscaled(_images[animationCell], Location);
	}

	// Moves the ship in a specific direction
	public void Move(MoveDirection direction)
		=> Location = direction switch
		{
			MoveDirection.Left => new Point(Location.X - HorizontalInterval, Location.Y),
			MoveDirection.Right => new Point(Location.X + HorizontalInterval, Location.Y),
			MoveDirection.Down => new Point(Location.X, Location.Y + VerticalInterval),
			_ => Location
		};
}