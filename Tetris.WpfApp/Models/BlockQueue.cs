using Tetris.WpfApp.Models.Blocks;

namespace Tetris.WpfApp.Models;

// class responsible for selecting the next block in the game
public class BlockQueue
{
	private readonly Random _random = new();

	// an array with an instance of all 7 types of blocks
	private readonly Block[] _blocks =
	[
		new IBlock(),
		new JBlock(),
		new LBlock(),
		new OBlock(),
		new SBlock(),
		new TBlock(),
		new ZBlock(),
	];

	// initialization of the next block with a random block
	public BlockQueue()
		=> NextBlock = RandomBlock();

	// the next block in the queue, will be displayed as Next during the game
	// it can also be a collection of blocks to display the queue of next blocks in the UI
	public Block NextBlock { get; private set; }

	// fetches the next block, updates the property and returns that block
	public Block GetNextBlockAndUpdate()
	{
		Block block = NextBlock;

		// loop to prevent the same block from being selected in a row
		do
		{
			NextBlock = RandomBlock();
		}
		while (block.Id == NextBlock.Id);

		return block;
	}

	// returns a random block
	private Block RandomBlock()
		=> _blocks[_random.Next(_blocks.Length)];
}