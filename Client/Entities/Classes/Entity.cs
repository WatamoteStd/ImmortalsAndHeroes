using Godot;
using System;

public partial class Entity : CharacterBody3D
{
	
	[Export] protected ProgressBar _healthBar;
	public uint Id {get; protected set;}
	protected int _health;
	protected int _maxHealth;
	public int Health
	{
		
		get => _health;
		set 
		{
			_health = Math.Clamp(value, 0, _maxHealth);
			if (_healthBar != null) _healthBar.Value = _health;
		}

	}

	public virtual void InitEntity(uint id, int health, int maxHealth)
	{
		
		Id = id;
		_health = health;
		_maxHealth = maxHealth;
		if (_healthBar != null)
		{
			_healthBar.MaxValue = _maxHealth;
			_healthBar.Value = _health;
		}

	}
	

	public virtual void Move(Vector3 position)
	{
		


	}

}
