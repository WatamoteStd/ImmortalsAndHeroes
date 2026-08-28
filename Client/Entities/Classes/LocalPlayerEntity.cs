using Godot;
using Shared.Characters;
using Shared.Udp.Packets.Category.Game;
using System;

public partial class LocalPlayerEntity : Entity
{
	
	public uint LocalPlayerId {get; set;}
	[Export] public PlayerAbilityController AbilityController { get; private set; }

	private float _mana;
	private float _maxMana;
	public float Mana
	{
		get => _mana;
		set
		{
			_mana = Math.Clamp(value, 0, _maxMana);
		}
	}

	private float _healthRegeneration;
	private float _manaRegeneration;

	public void InitEntity(uint id, float health, float maxHealth, string name, EntityType type, Vector3 pos, uint locPlayeId, float mana, float maxMana)
	{
		base.InitEntity(id, health, maxHealth, name, type, pos);

		_mana = mana;
		_maxMana = maxMana;
		LocalPlayerId = locPlayeId;

		if (GameSession.Instance.StatsCache.HealthRegen == 0.0f)
		{
			_healthRegeneration = _dllData.HealthRegeneration;
		}
		if (GameSession.Instance.StatsCache.ManaRegen == 0.0f)
		{
			_manaRegeneration = _dllData.ManaRegeneration;
		}

	}

	public void UpdateStats( in S2C_StatsSyncPacket data)
	{
		GD.Print($"Server sent Mana: {data.Mana}, MaxMana: {data.MaxMana}");

		_healthRegeneration = data.HealthRegen;
		_speed = data.Speed;
		_maxHealth = (int)data.MaxHealth;
		_health = (int)data.Health;

		_maxMana = data.MaxMana;
		_mana = data.Mana;
		_manaRegeneration = data.ManaRegen; 

		SceneManager.Instance.PlayerHud.ReplaceHealth(_health, _maxHealth);
		SceneManager.Instance.PlayerHud.ReplaceMana(_mana, _maxMana);

		
	}


	public override void Regenerate(float delta)
	{
		if (_health < _maxHealth)
		{
			
			_healthRegenBuffer += _healthRegeneration * delta;

			if (_healthRegenBuffer >= 1.0f)
			{
				int amount = (int)_healthRegenBuffer;
				Health += amount;
				_healthRegenBuffer -= (float)amount;

			}

		}

		if (_mana < _maxMana)
		{
			
			_manaRegenBuffer += _manaRegeneration * delta;

			if (_manaRegenBuffer >= 1.0f)
			{
				int amount = (int)_manaRegenBuffer;
				Mana += amount;
				_manaRegenBuffer -= (float)amount;
			}

		}
		SceneManager.Instance.PlayerHud.UpdateHealth((uint)_health);
		SceneManager.Instance.PlayerHud.UpdateMana((uint)_mana);


	}


}
