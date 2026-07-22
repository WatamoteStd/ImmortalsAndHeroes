using Godot;
using System;

public partial class GameSession : Node
{

	public static GameSession Instance {get; private set;}
	
	public uint NetworkId {get; set;}
	public long GlobalId { get; set;}
	public string AuthToken {get; set;} = ""; // for https
	public long AuthTicket {get; set;} // for udp

	public EntityClient Character { get; set;}

	public enum State
	{
		Authorizing,
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
