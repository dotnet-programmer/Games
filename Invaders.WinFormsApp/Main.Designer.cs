namespace Invaders.WinFormsApp;

partial class Main
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
		components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
		AnimationTimer = new System.Windows.Forms.Timer(components);
		GameTimer = new System.Windows.Forms.Timer(components);
		SuspendLayout();
		// 
		// AnimationTimer
		// 
		AnimationTimer.Interval = 90;
		AnimationTimer.Tick += AnimationTimer_Tick;
		// 
		// GameTimer
		// 
		GameTimer.Interval = 90;
		GameTimer.Tick += GameTimer_Tick;
		// 
		// Main
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1059, 761);
		DoubleBuffered = true;
		FormBorderStyle = FormBorderStyle.FixedSingle;
		Icon = (Icon)resources.GetObject("$this.Icon");
		MaximizeBox = false;
		Name = "Main";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Invaders";
		Paint += Main_Paint;
		KeyDown += Main_KeyDown;
		KeyUp += Main_KeyUp;
		ResumeLayout(false);
	}

	#endregion

	private System.Windows.Forms.Timer AnimationTimer;
	private System.Windows.Forms.Timer GameTimer;
}
