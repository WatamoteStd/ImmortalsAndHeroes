using Godot;
using Shared.Characters;
using Shared.Udp.Packets.Category;
using System;
using System.Collections.Generic;

public partial class WorldHandler : Node3D
{
	
	public uint RegionId {get; private set;} = 0;
	[Export] private PackedScene _remotePlayerScene;
	[Export] private PackedScene _localPlayerScene;

	public override void _Ready()
	{
		ServerMaster.Instance.WorldManager = this;
	}



	public void SpawnLocalPlayer(S2C_HandshakeSuccessPacket playerPacket)
	{
		
		PackedScene model = ResourceLoader.Load<PackedScene>(EntityRegistry.GetEntityData(playerPacket.Type).ScenePath);
		var localPlayer = _localPlayerScene.Instantiate<LocalPlayerEntity>();
		var locPlayerModel = model.Instantiate<Node3D>();
		
		AddChild(localPlayer);
		localPlayer.GetNode<Node3D>("Model").AddChild(locPlayerModel);

		Vector3 dataPos = new Vector3(playerPacket.PosX, playerPacket.PosY, playerPacket.PosZ);
		localPlayer.InitEntity(playerPacket.Id, playerPacket.CurrentHp, playerPacket.CurrentHp, playerPacket.UserId, playerPacket.CurrentMp, playerPacket.CurrentMp);
		localPlayer.GlobalPosition = dataPos;

		SceneManager.Instance.InitPlayerHud((uint)playerPacket.CurrentHp, (uint)playerPacket.CurrentMp, playerPacket.Silver, (uint)playerPacket.Lvl, playerPacket.Name);
		


	}

	public override void _ExitTree()
	{
		
		if (ServerMaster.Instance?.WorldManager == this)
		{
			ServerMaster.Instance.WorldManager = null;
		}

	}



}
