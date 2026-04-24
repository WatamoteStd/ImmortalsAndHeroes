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
		EventBus.OnPositionsUpdated += UpdatePosition;

	}

	private void UpdatePosition(ushort id, Vector3 pos)
	{
		
		// 1. Проверяем, заходит ли сюда код вообще
	GD.Print($"DEBUG: Пришли данные для ID: {id}. Позиция: {pos}");

	if (_entities.TryGetValue(id, out Entity entity))
	{
		// 2. Если нашли — двигаем и рапортуем
		entity.GlobalPosition = entity.GlobalPosition.Lerp(pos, 0.5f);
		GD.Print($"DEBUG: Герой {id} найден и передвинут в {pos}");
	}
	else 
	{
		// 3. Если не нашли — вот тут и затык!
		GD.PrintErr($"DEBUG: Пиздец! Пришел ID {id}, но в словаре его НЕТ. В словаре сейчас: {string.Join(", ", _entities.Keys)}");
	}
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
