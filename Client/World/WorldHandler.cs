using Godot;
using Shared.Udp.Packets.Category;
using System;
using System.Collections.Generic;

public partial class WorldHandler : Node3D
{
	
	public uint RegionId {get; private set;} = 0;



	public override void _Ready()
	{
		ServerMaster.Instance.WorldManager = this;

	}



	public void SpawnLocalPlayer(S2C_HandshakeSuccessPacket playerPacket)
	{
		
		GD.Print("LOCAL PLAYER ADDED!!!!!!");

	}

	public override void _ExitTree()
	{
		
		if (ServerMaster.Instance?.WorldManager == this)
		{
			ServerMaster.Instance.WorldManager = null;
		}

	}



}
