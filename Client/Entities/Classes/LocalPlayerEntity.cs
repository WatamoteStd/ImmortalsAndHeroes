using Godot;
using Shared.Characters;
using System;

public partial class LocalPlayerEntity : Entity
{
	
	public uint LocalPlayerId {get; set;}
	public float Speed {get; set;} = 3.0f;

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

	public void InitEntity(uint id, int health, int maxHealth, string name, EntityType type, Vector3 pos, uint locPlayeId, int mana, int maxMana)
	{
		base.InitEntity(id, health, maxHealth, name, type, pos);

		_mana = mana;
		_maxMana = maxMana;
		LocalPlayerId = locPlayeId;

	}


	public override void Regenerate(float delta)
	{
		base.Regenerate(delta);
		SceneManager.Instance.PlayerHud.UpdateHealth((uint)_health);
	}


}
