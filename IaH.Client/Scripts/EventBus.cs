using Godot;
using System;
using IaH.Shared.Networking;

public static partial class EventBus 
{
	
	public static event Action<PlayerJoinedPacket> OnPlayerJoined;
	public static event Action<CharacterType> OnHeroSelected;
	
	public static void PublishHeroSelected(CharacterType hero)
	{
		OnHeroSelected?.Invoke(hero);
	}

	public static void PublishPlayerJoined(PlayerJoinedPacket packet)
	{
		
		OnPlayerJoined?.Invoke(packet);

	}

}
