namespace Hangman.WinFormsApp;

partial class MainForm
{
	/// <summary>
	///  Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	///  Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Windows Form Designer generated code

	/// <summary>
	///  Required method for Designer support - do not modify
	///  the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
		TxtWord = new TextBox();
		BtnSetWord = new Button();
		TxtLetter = new TextBox();
		BtnCheck = new Button();
		TxtUsedLetters = new TextBox();
		BtnNewGame = new Button();
		PBAttempts = new PictureBox();
		((System.ComponentModel.ISupportInitialize)PBAttempts).BeginInit();
		SuspendLayout();
		// 
		// TxtWord
		// 
		TxtWord.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
		TxtWord.Location = new Point(12, 12);
		TxtWord.Name = "TxtWord";
		TxtWord.Size = new Size(210, 33);
		TxtWord.TabIndex = 1;
		TxtWord.TextAlign = HorizontalAlignment.Center;
		TxtWord.KeyUp += TxtWord_KeyUp;
		// 
		// BtnSetWord
		// 
		BtnSetWord.Location = new Point(12, 51);
		BtnSetWord.Name = "BtnSetWord";
		BtnSetWord.Size = new Size(210, 30);
		BtnSetWord.TabIndex = 2;
		BtnSetWord.Text = "Ustaw hasło";
		BtnSetWord.UseVisualStyleBackColor = true;
		BtnSetWord.Click += BtnSetWord_Click;
		// 
		// TxtLetter
		// 
		TxtLetter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
		TxtLetter.Location = new Point(12, 87);
		TxtLetter.Name = "TxtLetter";
		TxtLetter.Size = new Size(210, 23);
		TxtLetter.TabIndex = 3;
		TxtLetter.TextAlign = HorizontalAlignment.Center;
		TxtLetter.Visible = false;
		TxtLetter.KeyUp += TxtLetter_KeyUp;
		// 
		// BtnCheck
		// 
		BtnCheck.Location = new Point(12, 116);
		BtnCheck.Name = "BtnCheck";
		BtnCheck.Size = new Size(210, 30);
		BtnCheck.TabIndex = 4;
		BtnCheck.Text = "Sprawdź";
		BtnCheck.UseVisualStyleBackColor = true;
		BtnCheck.Click += BtnCheck_Click;
		// 
		// TxtUsedLetters
		// 
		TxtUsedLetters.Enabled = false;
		TxtUsedLetters.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
		TxtUsedLetters.Location = new Point(12, 190);
		TxtUsedLetters.Name = "TxtUsedLetters";
		TxtUsedLetters.ReadOnly = true;
		TxtUsedLetters.Size = new Size(210, 23);
		TxtUsedLetters.TabIndex = 6;
		TxtUsedLetters.TabStop = false;
		TxtUsedLetters.TextAlign = HorizontalAlignment.Center;
		TxtUsedLetters.Visible = false;
		// 
		// BtnNewGame
		// 
		BtnNewGame.Location = new Point(12, 219);
		BtnNewGame.Name = "BtnNewGame";
		BtnNewGame.Size = new Size(210, 30);
		BtnNewGame.TabIndex = 5;
		BtnNewGame.Text = "Nowa gra";
		BtnNewGame.UseVisualStyleBackColor = true;
		BtnNewGame.Visible = false;
		BtnNewGame.Click += BtnNewGame_Click;
		// 
		// PBAttempts
		// 
		PBAttempts.Location = new Point(228, 12);
		PBAttempts.Name = "PBAttempts";
		PBAttempts.Size = new Size(244, 237);
		PBAttempts.SizeMode = PictureBoxSizeMode.Zoom;
		PBAttempts.TabIndex = 7;
		PBAttempts.TabStop = false;
		// 
		// MainForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		BackColor = Color.White;
		ClientSize = new Size(484, 261);
		Controls.Add(PBAttempts);
		Controls.Add(BtnNewGame);
		Controls.Add(TxtUsedLetters);
		Controls.Add(BtnCheck);
		Controls.Add(TxtLetter);
		Controls.Add(BtnSetWord);
		Controls.Add(TxtWord);
		FormBorderStyle = FormBorderStyle.FixedSingle;
		MaximizeBox = false;
		Name = "MainForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Hangman";
		((System.ComponentModel.ISupportInitialize)PBAttempts).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	private TextBox TxtWord;
	private Button BtnSetWord;
	private TextBox TxtLetter;
	private Button BtnCheck;
	private TextBox TxtUsedLetters;
	private Button BtnNewGame;
	private PictureBox PBAttempts;
}
