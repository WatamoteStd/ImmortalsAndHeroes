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
		EventBus.PublishHeroSelected(CharacterType.Warrior);
		GetTree().ChangeSceneToFile("Scenes/world.tscn");
	}
}
