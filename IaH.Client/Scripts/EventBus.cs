using Godot;
using System;
using IaH.Shared.Networking;
using System.Collections.Generic;

public static partial class EventBus 
{
	// MENU
	public static event Action<string> OnNicknameChanged;
	public static void PublishNicknameChanged(string nick)
	{
		OnNicknameChanged?.Invoke(nick);
	}
	
	public static event Action<CharacterType> OnHeroSelected;
	public static event Action OnPlayerConnectedToWorld;
	public static event Action<Vector3> OnPlayerRMB;
	public static event Action<EntityStatsPacket> OnStatsPacketReceived;
	public static event Action<ushort, Vector3> OnPositionsUpdated;
	public static event Action<ushort> OnDisconnectPacketReceived;
	
	// LOBBY
	public static event Action OnJoinTheQueue; // when join queue but not in lobby
	public static void PublishOnJoinQueue()
	{
		OnJoinTheQueue?.Invoke();
	}
	public static event Action<string> OnSendMessageToLobby;
	public static void PublishSendMessageToLobby(string message)
	{
		OnSendMessageToLobby?.Invoke(message);
	}
	public static event Action<string, string> OnMessageReceived;
	public static void PublishOnMessageReceived(string sender, string msg)
	{
		OnMessageReceived?.Invoke(sender, msg);
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

	public static void PublishHeroSelected(CharacterType hero)
	{
		OnHeroSelected?.Invoke(hero);
	}

}
