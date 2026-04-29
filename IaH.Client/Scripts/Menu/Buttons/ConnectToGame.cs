using Godot;
using System;

public partial class ConnectToGame : Button
{
	[Export] public PanelContainer Container;
	public override void _Ready()
	{
		
		Container.Modulate = new Color(1,1,1,0);
		Container.Visible = true;
		Container.MouseFilter = Control.MouseFilterEnum.Stop;

		Pressed += OpenConnectPanel;

	}

	private void OpenConnectPanel()
	{
		MouseFilter = Control.MouseFilterEnum.Ignore;
		Container.MouseFilter = Control.MouseFilterEnum.Stop;

		var tween = GetTree().CreateTween().SetParallel(true);
		
		tween.TweenProperty(Container, "modulate:a", 1.0f, 0.15f);
		tween.TweenProperty(this, "modulate:a", 0.0f, 0.15f);
		
		tween.Finished += () => Visible = false;

	}
	public override void _Process(double delta)
	{
	}
}
