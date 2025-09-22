namespace Tetris.WpfApp.Models;

public class GameGrid
{
	// główny grid gry
	private readonly int[,] _grid;

	public GameGrid(int rows, int columns)
	{
		Rows = rows;
		Columns = columns;
		_grid = new int[rows, columns];
	}

	// indekser do używania klasy jak tablicy
	public int this[int r, int c]
	{
		get => _grid[r, c];
		set => _grid[r, c] = value;
	}

	public int Rows { get; }
	public int Columns { get; }

	// sprawdzenie czy dana komórka jest pusta, czy nie
	// musi być w gridzie i wartość w tym polu tablicy musi być 0
	public bool IsEmpty(int row, int col)
		=> IsInside(row, col) && _grid[row, col] == 0;

	// sprawdzenie czy dany wiersz i kolumna znajdują się w siatce, czy nie
	public bool IsInside(int row, int col)
		=> row >= 0 && row < Rows && col >= 0 && col < Columns;

	// sprawdzenie czy cały wiersz jest pełny
	public bool IsRowFull(int row)
	{
		for (int col = 0; col < Columns; col++)
		{
			if (_grid[row, col] == 0)
			{
				return false;
			}
		}
		return true;
	}

	// sprawdzenie czy cały wiersz jest pusty
	public bool IsRowEmpty(int row)
	{
		for (int col = 0; col < Columns; col++)
		{
			if (_grid[row, col] != 0)
			{
				return false;
			}
		}
		return true;
	}

	// czyści cały wiersz
	private void ClearRow(int row)
	{
		for (int col = 0; col < Columns; col++)
		{
			_grid[row, col] = 0;
		}
	}

	// przesuwa wiersz na dół o podaną ilość rzędów
	private void MoveRowDown(int row, int numRows)
	{
		for (int col = 0; col < Columns; col++)
		{
			_grid[row + numRows, col] = _grid[row, col];
			_grid[row, col] = 0;
		}
	}

	// usuwanie pełnych wierszy
	public int ClearFullRows()
	{
		// oznacza ilość usuniętych wierszy
		int cleared = 0;

		// przejście od dolnego wiersza w kierunku góry
		for (int row = Rows - 1; row >= 0; row--)
		{
			// jeśli wiersz jest pełny - czyszczenie go i zwiększanie cleared +1
			if (IsRowFull(row))
			{
				ClearRow(row);
				cleared++;
			}
			// jeśeli wiersz nie jest pełny, sprawdź czy trzeba go przesunąć w dół o ilość wcześniej wyczyszczonych wierszy
			else if (cleared > 0)
			{
				MoveRowDown(row, cleared);
			}
		}

		// zwrócenie ilości wyczyszczonych wierszy
		return cleared;
	}
}