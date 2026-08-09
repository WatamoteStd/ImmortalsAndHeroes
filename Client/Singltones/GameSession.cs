using Godot;
using Shared.Udp.Packets.Category;
using System;

public partial class GameSession : Node
{

	public static GameSession Instance {get; private set;}
	public string Username {get; set;}
	public uint NetworkId {get; set;}
	public long GlobalId { get; set;}
	
	public string MasterToken { get; set; }
	public string UdpToken { get; set; }
	public string UdpIp { get; set; }
	public int UdpPort { get; set; }

	public S2C_HandshakeSuccessPacket PlayerCache;

	public enum State
	{
		Authorizing,
		Menu,
		Loading,
		InGame,
		Disconnected,
		Afk
	}
	public State CurrentSessionState = State.Authorizing;

	public override void _EnterTree()
	{
		
		if (Instance != null)
		{
			
			QueueFree();
			return;

		}
		else
		{
			Instance = this;
		}

	}



}
