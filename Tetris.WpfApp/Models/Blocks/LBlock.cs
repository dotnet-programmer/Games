namespace Tetris.WpfApp.Models.Blocks;

public class LBlock : Block
{
	private readonly Position[][] _tiles =
	[
		[new(0,2), new(1,0), new(1,1), new(1,2)],
		[new(0,1), new(1,1), new(2,1), new(2,2)],
		[new(1,0), new(1,1), new(1,2), new(2,0)],
		[new(0,0), new(0,1), new(1,1), new(2,1)],
	];

	public override int Id => 3;

	protected override Position[][] Tiles => _tiles;

	protected override Position StartOffset => new(0, 3);
}