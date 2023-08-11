using Hangman.WinFormsApp.Properties;

namespace Hangman.WinFormsApp;

public partial class MainForm : Form
{
	private string _searchWord;
	private int _attempts = 0;

	public MainForm()
	{
		InitializeComponent();
	}


	#region Set word

	private void BtnSetWord_Click(object sender, EventArgs e)
	{
		SetWord();
	}

	private void TxtWord_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Enter)
		{
			SetWord();
		}
	}

	private void SetWord()
	{
		if (TxtWord.Text.Length < 1)
		{
			return;
		}

		_searchWord = TxtWord.Text.ToLower();

		TxtWord.Text = string.Empty;
		for (int i = 0; i < _searchWord.Length; i++)
		{
			TxtWord.Text += "_ ";
		}

		SetIngameState();
	}

	#endregion Set word



	#region Check word

	private void BtnCheck_Click(object sender, EventArgs e)
	{
		CheckWord();
	}

	private void TxtLetter_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Enter)
		{
			CheckWord();
		}
	}

	private void CheckWord()
	{
		string letter = TxtLetter.Text.ToLower();
		TxtUsedLetters.Text += $"{letter} ";

		if (_searchWord.Contains(letter))
		{
			GoodAttempt(letter);
		}
		else
		{
			BadAttempt();
		}

		TxtLetter.Text = string.Empty;
		TxtLetter.Focus();
	}

	#endregion Check word


	private void GoodAttempt(string letter)
	{
		string word = TxtWord.Text;

		for (int i = 0; i < _searchWord.Length; i++)
		{
			if (_searchWord[i] == letter[0])
			{
				word = word.Remove(i * 2, 1).Insert(i * 2, letter);
			}
		}

		TxtWord.Text = word;

		if (!TxtWord.Text.Contains("_"))
		{
			EndGame(true);
		}
	}

	private void BadAttempt()
	{
		_attempts++;

		if (_attempts > 5)
		{
			EndGame(false);
			return;
		}

		PBAttempts.Image = _attempts switch
		{
			1 => Resources.step1,
			2 => Resources.step2,
			3 => Resources.step3,
			4 => Resources.step4,
			5 => Resources.step5,
			_ => null
		};
	}

	private void EndGame(bool isWin)
	{
		PBAttempts.Image = isWin ? Resources.win : Resources.lost;
		SetEndGameState();
	}

	private void BtnNewGame_Click(object sender, EventArgs e)
	{
		SetNewGameState();
	}

	private void SetNewGameState()
	{
		_attempts = 0;
		PBAttempts.Image = null;
		TxtWord.Text = TxtUsedLetters.Text = TxtLetter.Text = _searchWord = string.Empty;
		BtnNewGame.Visible = TxtLetter.Visible = BtnCheck.Visible = TxtUsedLetters.Visible = false;
		TxtWord.Enabled = BtnSetWord.Visible = true;
		TxtWord.Focus();
	}

	private void SetIngameState()
	{
		TxtLetter.Visible = BtnCheck.Visible = TxtUsedLetters.Visible = true;
		TxtWord.Enabled = BtnSetWord.Visible = false;
		TxtLetter.Focus();
	}

	private void SetEndGameState()
	{
		TxtLetter.Visible = BtnCheck.Visible = TxtUsedLetters.Visible = false;
		TxtWord.Text = _searchWord;
		BtnNewGame.Visible = true;
		BtnNewGame.Select();
	}
}
