using Godot;
using System;

public partial class AbilityUiPanel : HBoxContainer
{
	
	[Export] private AbilitySlotUi[] _slots;


	public override void _Ready()
	{
		
		PlayerAbilityController.OnAbilitiesSynced += RedrawAbilities;

	}

	private void RedrawAbilities()
	{
		
		for (int i = 0; i < _slots.Length; i++)
		{
			var slotData = PlayerAbilityController.Slots[i];
	
			_slots[i].Init(slotData.AbilityId);

			if (slotData.CooldownRemaining > 0)
			{
				_slots[i].StartCooldown(slotData.CooldownRemaining);
			}
	}

	}


}
