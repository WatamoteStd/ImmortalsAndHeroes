using Godot;
using IaH.Shared.Networking;
using System;
using System.Collections.Generic;

public partial class EntityHandler : Node3D
{
	[Export] public PackedScene FrozenScene;
	[Export] public PackedScene VoidlessStarScene;

	private Dictionary<ushort, Entity> _entities = new();

	public override void _Ready()
	{
		EventBus.OnPlayerJoined += SpawnPlayer;
		EventBus.OnPositionsUpdated += UpdatePosition;

	}

	private void UpdatePosition(ushort id, Vector3 pos)
	{
		

	if (_entities.TryGetValue(id, out Entity entity))
	{
		
		entity.GlobalPosition = entity.GlobalPosition.Lerp(pos, 0.5f);
	}
	
	}

	private void SpawnPlayer(PlayerJoinedPacket packet)
	{
		if (_entities.ContainsKey(packet.EntityId)) return;

		Entity newEntity = null;
		
		switch (packet.SelectedHero)
		{
			
			case CharacterType.Frozen:

				newEntity = FrozenScene.Instantiate<Entity>();
				

			break;

			case CharacterType.VoidlessStar:

				newEntity = VoidlessStarScene.Instantiate<Entity>();

			break;
			default:
				GD.PrintErr($"Попытка спавна неизвестного героя: {packet.SelectedHero}");
			break;

		}
		if (newEntity != null)
		{
			float posX = packet.X / 100f;
			float posZ = packet.Z / 100f;
			float posY = packet.Y / 100f;
			newEntity.EntityID = packet.EntityId;
			GetParent().AddChild(newEntity);
			newEntity.GlobalPosition = new Vector3(posX, posY, posZ);
			_entities.Add(newEntity.EntityID, newEntity);
			GD.Print($"Entity {newEntity.EntityID} added to dictionary");
		}

	}

	
}
