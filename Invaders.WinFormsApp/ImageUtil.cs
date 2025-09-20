namespace Invaders.WinFormsApp;

internal class ImageUtil
{
	public static Bitmap ResizeImage(Bitmap picture, int width, int height)
	{
		Bitmap resizedPicture = new(width, height);
		using (Graphics graphics = Graphics.FromImage(resizedPicture))
		{
			graphics.DrawImage(picture, 0, 0, width, height);
		}
		return resizedPicture;
	}
}