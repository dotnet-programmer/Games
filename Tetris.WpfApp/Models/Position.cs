namespace Tetris.WpfApp.Models;

// represents the position of a cell in the grid
public class Position(int row, int column)
{
	public int Row { get; set; } = row;
	public int Column { get; set; } = column;
}