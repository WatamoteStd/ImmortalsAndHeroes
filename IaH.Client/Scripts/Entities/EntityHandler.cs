using Godot;
using IaH.Shared.Networking;
using System;
using System.Collections.Generic;

public partial class EntityHandler : Node3D
{
	[Export] public PackedScene Warrior;
	[Export] public PackedScene Archer;

	private Dictionary<ushort, Entity> _entities = new();

	public override void _Ready()
	{
		
		EventBus.OnPlayerJoined += SpawnPlayer;

	}

	private void SpawnPlayer(PlayerJoinedPacket packet)
	{
		if (_entities.ContainsKey(packet.EntityId)) return;

		Entity newEntity = null;
		
		switch (packet.SelectedHero)
		{
			
			case CharacterType.Warrior:

				newEntity = Warrior.Instantiate<Entity>();
				

			break;

			case CharacterType.Archer:

				newEntity = Archer.Instantiate<Entity>();

			break;

		}
		if (newEntity != null)
		{
			newEntity.EntityID = packet.EntityId;
			newEntity.GlobalPosition = new Vector3(packet.X, packet.Y, packet.Z);
			GetParent().AddChild(newEntity);
			_entities.Add(newEntity.EntityID, newEntity);
		}

	}

	
}
