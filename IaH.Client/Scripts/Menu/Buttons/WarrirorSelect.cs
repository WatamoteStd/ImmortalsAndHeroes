using Godot;
using System;
using IaH.Shared.Networking;

public partial class WarrirorSelect : Button
{
	
	public override void _Ready()
	{
		Pressed += SendEvent;
	}

	private void SendEvent()
	{
		EventBus.PublishHeroSelected(CharacterType.Frozen);
		GetTree().ChangeSceneToFile("Scenes/world.tscn");
	}
}
