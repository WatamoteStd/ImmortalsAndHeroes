using Godot;
using System;

public partial class Menu : Control
{
	[Export] public Button Ladder;
	[Export] public Button Heroes;
	[Export] public Button Glossarium;
	[Export] public Button BecomeTester;

	[Export] public Label NicknameText;
	[Export] public LineEdit NicknameEdit;
	[Export] public TextureButton ChangeNickname;
	public override void _Ready()
	{
		ChangeNickname.Pressed += EditNickname;
		NicknameEdit.FocusExited += EditNicknameEnd;
		NicknameEdit.TextSubmitted += (string text) => EditNicknameEnd();	
		}

	private void EditNickname()
	{

		NicknameText.Visible = false;
		NicknameEdit.Visible = true;
		NicknameEdit.GrabFocus();
		
	}
	private void EditNicknameEnd()
	{
		
		NicknameEdit.Visible = false;
		NicknameText.Text = NicknameEdit.Text;
		NicknameText.Visible = true;

	}
	public override void _Input(InputEvent @event)
	{
		
		if (@event is InputEventMouseButton mouseAction && mouseAction.Pressed)
		{
			
			if (NicknameEdit.HasFocus() && !NicknameEdit.GetGlobalRect().HasPoint(mouseAction.GlobalPosition))
			{
				NicknameEdit.ReleaseFocus();
			}

		}

	}

 
	
}
