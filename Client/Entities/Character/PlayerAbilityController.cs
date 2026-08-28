using Godot;
using Shared.Ability;
using Shared.Udp.Packets.Category.Game.Ability;
using Shared.Ability.Params;
using System;

public partial class PlayerAbilityController : Node
{
	public static event Action OnAbilitiesSynced;
	public static AbilitySlotData[] Slots {get; private set;} = new AbilitySlotData[6];

	public void UpdateAbilities(S2C_PlayerAbilitySyncPacket packet)
	{
		
		Slots[0] = packet.Slot0;
		Slots[1] = packet.Slot1;
		Slots[2] = packet.Slot2;
		Slots[3] = packet.Slot3;
		Slots[4] = packet.Slot4;
		Slots[5] = packet.Slot5;

		OnAbilitiesSynced?.Invoke();

	}

	public void ExecuteSkill(byte slot, Vector3 pos, Entity entity)
	{
	
		if (slot >= Slots.Length || Slots[slot].AbilityId == 0) 
		{
			return;
		}

		if (IsCanCast(slot, pos, entity))
		{
			uint entityId;

			if (entity == null) entityId = 0;
			else entityId = entity.Id;
			
			var packet = new C2S_CastAbilityRequestPacket
			{
				Slot = slot,
				TargetEntityId = entityId,
				PosX = pos.X,
				PosY = pos.Y,
				PosZ = pos.Z
			};

			ServerMaster.Instance.LP_CastAbilityRequest(packet);

		}

	}

	private bool IsCanCast(byte slot, Vector3 pos, Entity entity)
	{
		
		var abData = Slots[slot];

		if (abData.CooldownRemaining > 0) return false;

		if (AbilityRegistry.TryGetAbility(abData.AbilityId, out var dllData))
		{
			
			if (dllData.CastType == AbilityCastType.Target && dllData.TargetType != AbilityTarget.Self && entity == null) return false;

			if (dllData.CastTypeAdditional != AbilityAdditionalCastType.None && pos == Vector3.Zero) return false;
			if (dllData.TargetType == AbilityTarget.Player && entity is not PlayerEntity) return false;

			return true;

		}
		return false;

	}

}
