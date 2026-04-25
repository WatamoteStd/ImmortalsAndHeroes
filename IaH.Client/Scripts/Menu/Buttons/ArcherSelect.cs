using Godot;
using System;
using IaH.Shared.Networking;

public partial class ArcherSelect : Button
{
	
	public override void _Ready()
	{
		
		Pressed += SendEvent;

	}

	private void SendEvent()
	{
		
		EventBus.PublishHeroSelected(CharacterType.VoidlessStar);
		GetTree().ChangeSceneToFile("Scenes/world.tscn");

	}

	
	public override void _Process(double delta)
	{
	}
}
