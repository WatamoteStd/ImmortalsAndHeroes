using Godot;
using Shared.Characters;
using System;

public partial class Entity : CharacterBody3D
{
	
	[Export] protected ProgressBar _healthBar;
	public uint Id {get; protected set;}
	protected float _health;
	protected float _maxHealth;
	public string EntityName {get; private set;}
	[Export] protected Label _name;
	[Export] protected MeshInstance3D _selectedMesh;
	protected float _speed;


	protected float _healthRegenBuffer = 0f;
	protected float _manaRegenBuffer = 0f;

	public EntityType Type;
	public EntityData _dllData;
	[Export] public CollisionShape3D CollisionNode { get; set; } = null!;

	protected Vector3 _moveTarget;
	public float Health
	{
		
		get => _health;
		set 
		{
			_health = Math.Clamp(value, 0, _maxHealth);
			if (_healthBar != null) _healthBar.Value = _health;
		}

	}

	public virtual void InitEntity(uint id, float health, float maxHealth, string name, EntityType type, Vector3 pos)
	{
		
		Id = id;
		_health = health;
		_maxHealth = maxHealth;
		EntityName = name;
		if (_name != null)
		{
			_name.Text = name;
		}
		if (_healthBar != null)
		{
			_healthBar.MaxValue = _maxHealth;
			_healthBar.Value = _health;
		}
		Type = type;
		var data = EntityRegistry.GetEntityData(type);
		_dllData = data;
		SetCollisionSize(data.Height, data.Radius);
		_speed = data.BaseSpeed;

		GlobalPosition = pos;
		if (_selectedMesh != null)
		{
			_selectedMesh.Visible = false;
		}

	}

	public override void _Process(double delta)
	{
		GlobalPosition = GlobalPosition.MoveToward(_moveTarget, (float)delta * _speed);

		Regenerate((float)delta);

	}

	
	public virtual void Regenerate(float delta)
	{
		
		if (_health < _maxHealth)
		{
			
			_healthRegenBuffer += _dllData.HealthRegeneration * delta;

			if (_healthRegenBuffer >= 1.0f)
			{
				int amount = (int)_healthRegenBuffer;
				Health += amount;
				_healthRegenBuffer -= (float)amount;

			}

		}

	}

	public virtual void Move(Vector3 position)
	{
		
		_moveTarget = position;

	}

	protected void SetCollisionSize(float height, float radius)
	{
		CollisionNode ??= GetNodeOrNull<CollisionShape3D>("CollisionShape3D");
		
		if (CollisionNode.Shape is CapsuleShape3D capsule)
		{
			
			if (!CollisionNode.Shape.IsLocalToScene())
			{
				capsule = (CapsuleShape3D)capsule.Duplicate();
				CollisionNode.Shape = capsule;
			}

			capsule.Radius = radius;
			capsule.Height = height;

		}

	}

	public virtual void TakeDamage(float damage, int actualHealth)
	{
		_health = actualHealth;
		if (_healthBar != null)
		{
			_healthBar.Value = actualHealth;
		}
	}

	public void SelectEntity()
	{
		_selectedMesh.Visible = true;
	}
	public void DeselectEntity()
	{
		_selectedMesh.Visible = false;
	}


}
