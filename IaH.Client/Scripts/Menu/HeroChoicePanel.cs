using Godot;
using System;

public partial class HeroChoicePanel : PanelContainer
{
	[Export] public Button ConnectButton;

	public override void _Input(InputEvent @event)
	{
		if (Modulate.A < 0.9f) return;
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			
			if (!GetGlobalRect().HasPoint(GetGlobalMousePosition()))
			{
				ClosePanel();
			}

		}

	}

	private void ClosePanel()
	{
		
		var tween = GetTree().CreateTween().SetParallel(true);
		MouseFilter = MouseFilterEnum.Ignore;
		ConnectButton.MouseFilter = Control.MouseFilterEnum.Stop;

		tween.TweenProperty(this, "modulate:a", 0.0f, 0.15f);
		tween.TweenProperty(ConnectButton, "modulate:a", 1.0f, 0.15f);

		tween.Finished += () =>
		{
			Visible = false;
			ConnectButton.Visible = true;
		};

	}


}
