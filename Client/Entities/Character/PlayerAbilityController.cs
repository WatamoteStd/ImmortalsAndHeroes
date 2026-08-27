using Godot;
using Shared.Udp.Packets.Category.Game.Ability;
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

}
