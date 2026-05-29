using Godot;
using Iah.Shared.Packets;
using LiteNetLib;
using System;
using System.Collections.Generic;

public partial class EventBus : Node
{
	
	public static EventBus Instance {get; private set;}
	[Signal] public delegate void JoinQueueRequestedEventHandler(byte packetType);
	public void PublishJoinQueueRequested(PacketType type)
	{
		EmitSignal(SignalName.JoinQueueRequested, (byte)type);
	}

	[Signal] public delegate void LoginRequestEventHandler(byte packetType, string name);
	public void PublishLoginRequest(PacketType type, string name)
	{
		EmitSignal(SignalName.LoginRequest, (byte)type, name);
	}

	[Signal] public delegate void LobbyHeroSelectedEventHandler(byte hero);
	public void PublishLobbyHeroSelected(byte hero)
	{
		EmitSignal(SignalName.LobbyHeroSelected, hero);
	}
	[Signal] public delegate void LobbyHeroChangedEventHandler();
	public void PublishLobbyHeroChanged()
	{
		EmitSignal(SignalName.LobbyHeroChanged);
	}
	[Signal] public delegate void LobbyTimerEventHandler(ushort time);
	public void PublishLobbyTimer(ushort time)
	{
		EmitSignal(SignalName.LobbyTimer, time);
	}

	// GAME EVENTS
	[Signal] public delegate void ReadyToGameEventHandler();
	public void PublishReadyToGame()
	{
		EmitSignal(SignalName.ReadyToGame);
	}
	public override void _Ready()
	{
		Instance = this;
	}


}
