using Godot;
using System;
using IaH.Shared.Networking;
using System.Collections.Generic;

public static partial class EventBus 
{
	
	public static event Action<PlayerJoinedPacket> OnPlayerJoined;
	public static event Action<CharacterType> OnHeroSelected;
	public static event Action OnPlayerConnectedToWorld;
	public static event Action<Vector3> OnPlayerRMB;
	public static event Action<EntityStatsPacket> OnStatsPacketReceived;
	public static event Action<ushort, Vector3> OnPositionsUpdated;
	public static event Action<ushort> OnDisconnectPacketReceived;
	
	// LOBBY
	public static event Action OnJoinTheQueue;
	public static void PublishOnJoinQueue()
	{
		OnJoinTheQueue?.Invoke();
	}

	public static void PublishDisconnectedPacketReceived(ushort id)
	{
		OnDisconnectPacketReceived?.Invoke(id);
	}

	public static void PublishStatsPacketReceived(EntityStatsPacket packet)
	{
		OnStatsPacketReceived?.Invoke(packet);
	}

	public static void PublishPositionsUpdated(ushort id, Vector3 position)
	{
		OnPositionsUpdated?.Invoke(id, position);
	}

	public static void PublishPlayerRMB(Vector3 cords)
	{
		OnPlayerRMB?.Invoke(cords);
	}

	public static void PublishPlayerConnectedToWorld()
	{
		OnPlayerConnectedToWorld?.Invoke();
	} 
	
	public static void PublishHeroSelected(CharacterType hero)
	{
		OnHeroSelected?.Invoke(hero);
	}

	public static void PublishPlayerJoined(PlayerJoinedPacket packet)
	{
		
		OnPlayerJoined?.Invoke(packet);

	}

}
