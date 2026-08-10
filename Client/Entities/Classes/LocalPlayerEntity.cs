using Godot;
using System;

public partial class LocalPlayerEntity : Entity
{
	
	public uint LocalPlayerId {get; set;}

	private int _mana;
	private int _maxMana;
	public int Mana
	{
		get => _mana;
		set
		{
			_mana = Math.Clamp(value, 0, _maxMana);
		}
	}

	public void InitEntity(uint id, int health, int maxHealth, uint locPlayeId, int mana, int maxMana)
	{
		base.InitEntity(id, health, maxHealth);

		_mana = mana;
		_maxMana = maxMana;
		LocalPlayerId = locPlayeId;

	}

	public override void Move(Vector3 position)
	{
		
		

	}



}
