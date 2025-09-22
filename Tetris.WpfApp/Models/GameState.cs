using Tetris.WpfApp.Models.Blocks;

namespace Tetris.WpfApp.Models;

// class that handles interactions between Grid and blocks
public class GameState
{
	private Block _currentBlock = default!;

	public GameState()
	{
		// initialization of the game field with 22 rows and 10 columns
		// the first 2 rows are hidden, so the game grid is 20x10
		GameGrid = new GameGrid(22, 10);

		BlockQueue = new BlockQueue();
		CurrentBlock = BlockQueue.GetNextBlockAndUpdate();
		CanHold = true;
	}

	public Block CurrentBlock
	{
		get => _currentBlock;
		private set
		{
			// after updating the current block, a reset is called, which sets the correct starting position and rotation
			_currentBlock = value;
			_currentBlock.Reset();

			// newly created blocks are moved down 2 rows to appear immediately in the game area
			// if there is no space, reversed back to the starting position
			for (int i = 0; i < 2; i++)
			{
				_currentBlock.Move(1, 0);
				if (!BlockFits())
				{
					_currentBlock.Move(-1, 0);
				}
			}
		}
	}

	public GameGrid GameGrid { get; }
	public BlockQueue BlockQueue { get; }
	public bool GameOver { get; private set; }

	// result is the total number of cleared rows
	public int Score { get; private set; }

	// storing the block for later
	public Block? HeldBlock { get; private set; }
	public bool CanHold { get; private set; }

	// storing the block for later
	public void HoldBlock()
	{
		if (!CanHold)
		{
			return;
		}

		// if there is no stored block, then set it to the current block, and insert the next block into the current one
		if (HeldBlock == null)
		{
			HeldBlock = CurrentBlock;
			CurrentBlock = BlockQueue.GetNextBlockAndUpdate();
		}
		// if there is a block stored, then swap it with the current block
		else
		{
			(CurrentBlock, HeldBlock) = (HeldBlock, CurrentBlock);
		}

		// setting to false to avoid switching back and forth between blocks
		CanHold = false;
	}

	// turns the block clockwise but only if it is possible to do so in the place where it is located
	public void RotateBlockClockwise()
	{
		// rotate the block, if it is in the forbidden position then rotate it back
		CurrentBlock.RotateClockwise();
		if (!BlockFits())
		{
			CurrentBlock.RotateCounterClockwise();
		}
	}

	// turns the block counterclockwise but only if it is possible in the place where it is located
	public void RotateBlockCounterClockwise()
	{
		// rotate the block, if it is in the forbidden position then rotate it back
		CurrentBlock.RotateCounterClockwise();
		if (!BlockFits())
		{
			CurrentBlock.RotateClockwise();
		}
	}

	// move the block to the left
	public void MoveBlockLeft()
	{
		// move the block, if it is in the forbidden position then move it back
		CurrentBlock.Move(0, -1);
		if (!BlockFits())
		{
			CurrentBlock.Move(0, 1);
		}
	}

	// move the block to the left
	public void MoveBlockRight()
	{
		// move the block, if it is in the forbidden position then move it back
		CurrentBlock.Move(0, 1);
		if (!BlockFits())
		{
			CurrentBlock.Move(0, -1);
		}
	}

	// move the block down
	public void MoveBlockDown()
	{
		// move the block, if it is in an forbidden position then move it back and place it on the game grid
		CurrentBlock.Move(1, 0);
		if (!BlockFits())
		{
			CurrentBlock.Move(-1, 0);
			PlaceBlock();
		}
	}

	// moves the current block down by the amount of free space below it, and then places it in the game grid
	public void DropBlock()
	{
		CurrentBlock.Move(BlockDropDistance(), 0);
		PlaceBlock();
	}

	// calculation of the minimum distance by which the current block can be moved
	public int BlockDropDistance()
	{
		int drop = GameGrid.Rows;
		foreach (Position p in CurrentBlock.TilePositions())
		{
			drop = Math.Min(drop, TileDropDistance(p));
		}
		return drop;
	}

	// checks that the current block is in the correct position
	private bool BlockFits()
	{
		// goes through the tile positions of the current block
		// false if any is outside the grid or overlaps another tile
		foreach (Position p in CurrentBlock.TilePositions())
		{
			if (!GameGrid.IsEmpty(p.Row, p.Column))
			{
				return false;
			}
		}
		return true;
	}

	// method called when the current block cannot be moved down
	private void PlaceBlock()
	{
		// goes through the tile positions of the current block and sets those positions in the game grid to the corresponding ID
		foreach (Position p in CurrentBlock.TilePositions())
		{
			GameGrid[p.Row, p.Column] = CurrentBlock.Id;
		}

		// cleaning all full lines + increasing the result
		Score += GameGrid.ClearFullRows();

		// check if the end of the game
		if (IsGameOver())
		{
			GameOver = true;
		}
		else
		{
			// if not the end of the game, update the current block
			CurrentBlock = BlockQueue.GetNextBlockAndUpdate();

			// unblocking the possibility of block swapping
			CanHold = true;
		}
	}

	// returns information about how many rows to move down the current block
	// takes the position, returns the number of empty cells directly below it
	// called for each tile in the current block
	private int TileDropDistance(Position p)
	{
		int drop = 0;
		while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
		{
			drop++;
		}
		return drop;
	}

	// check if the end of the game
	// true if one of the two hidden lines at the top is not empty
	private bool IsGameOver()
		=> !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
}