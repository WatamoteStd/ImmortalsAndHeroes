using Godot;
using Shared.Characters;
using Shared.Udp.Packets.Category;
using System;
using System.Collections.Generic;

public partial class WorldHandler : Node3D
{
	
	public uint RegionId {get; private set;} = 0;
	[Export] public Camera3D RegionCamera { get; set; }



	public override void _Ready()
	{
		ServerMaster.Instance.WorldManager = this;
	}



	public void SpawnLocalPlayer(S2C_HandshakeSuccessPacket playerPacket)
	{
		
		PackedScene model = ResourceLoader.Load<PackedScene>(EntityRegistry.GetEntityData(playerPacket.Type).ScenePath);
		var locPlayer = model.Instantiate<CharacterBody3D>();
		AddChild(locPlayer);

		Vector3 dataPos = new Vector3(playerPacket.PosX, playerPacket.PosY, playerPacket.PosZ);
		Vector3 cameraPos = dataPos + new Vector3(0f, 7f, 7f);
		locPlayer.GlobalPosition = dataPos;

		RegionCamera.GlobalPosition = cameraPos;
		RegionCamera.RotationDegrees = new Vector3(-40f, 0f, 0f);
		


	}

	public override void _ExitTree()
	{
		
		if (ServerMaster.Instance?.WorldManager == this)
		{
			ServerMaster.Instance.WorldManager = null;
		}

	}



}
