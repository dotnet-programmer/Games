using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invaders.WinFormsApp.Star;
internal struct Star
{
	public Point point;
	public Pen pen;

	public Star(Point point, Pen pen)
	{
		this.point = point;
		this.pen = pen;
	}
}
