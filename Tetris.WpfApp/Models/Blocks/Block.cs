namespace Tetris.WpfApp.Models.Blocks;

// an abstract base class for each of the 7 types of blocks
public abstract class Block
{
	// current rotation
	private int _currentRotation;

	// current offset
	private readonly Position _currentOffset;

	// constructor sets the offset equal to the initial offset
	public Block()
		=> _currentOffset = new Position(StartOffset.Row, StartOffset.Column);

	// identifier, to distinguish blocks
	public abstract int Id { get; }

	// get tile positions in four states of rotation
	protected abstract Position[][] Tiles { get; }

	// the initial offset, which determines where the block appears in the grid
	protected abstract Position StartOffset { get; }

	// returns the grid positions occupied by the block, taking into account the current rotation and offset
	public IEnumerable<Position> TilePositions()
		=> Tiles[_currentRotation].Select(p => new Position(p.Row + _currentOffset.Row, p.Column + _currentOffset.Column));

	// increasing the state of rotation, wrapping around zero if in the final state
	public void RotateClockwise()
		=> _currentRotation = (_currentRotation + 1) % Tiles.Length;

	// decreasing the state of rotation, wrapping around zero if in the final state
	public void RotateCounterClockwise()
	{
		if (_currentRotation == 0)
		{
			_currentRotation = Tiles.Length - 1;
		}
		else
		{
			_currentRotation--;
		}
	}

	// move the block by a given number of rows and columns
	public void Move(int rows, int columns)
	{
		_currentOffset.Row += rows;
		_currentOffset.Column += columns;
	}

	// resets rotation and position
	public void Reset()
	{
		_currentRotation = 0;
		_currentOffset.Row = StartOffset.Row;
		_currentOffset.Column = StartOffset.Column;
	}
}