using Godot;
using System;

public partial class HelpPopUp : TextureButton
{
	
	[Export] private Control _window;

	public override void _Ready()
	{
		
		MouseEntered += () =>
		{
			_window.GlobalPosition = GlobalPosition + new Vector2(-100, -120f);
			_window.Visible = true;
		};
		MouseExited += () =>
		{
			_window.Visible = false;
		};

	}


}
