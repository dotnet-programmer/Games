namespace Tetris.WpfApp.Models.Blocks;

public class IBlock : Block
{
	// tile positions for 4 states of rotation
	private readonly Position[][] _tiles =
	[
		[new(1,0), new(1,1), new(1,2), new(1,3)],
		[new(0,2), new(1,2), new(2,2), new(3,2)],
		[new(2,0), new(2,1), new(2,2), new(2,3)],
		[new(0,1), new(1,1), new(2,1), new(3,1)],
	];

	// TODO: zmienić na enum (BlockType) żeby nie było takich wartości liczbowych wziętych z nikąd
	public override int Id => 1;

	// get the tile array
	protected override Position[][] Tiles => _tiles;

	// initial offset (-1, 3), so the block will appear in the middle of the top row
	protected override Position StartOffset => new(-1, 3);
}