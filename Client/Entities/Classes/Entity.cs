using Godot;
using Shared.Characters;
using System;

public partial class Entity : CharacterBody3D
{
	
	[Export] protected ProgressBar _healthBar;
	public uint Id {get; protected set;}
	protected int _health;
	protected int _maxHealth;
	public string EntityName {get; private set;}
	[Export] protected Label _name;
	[Export] protected MeshInstance3D _selectedMesh;
	protected float _speed;


	public EntityType Type;
	[Export] public CollisionShape3D CollisionNode { get; set; } = null!;

	protected Vector3 _moveTarget;
	public int Health
	{
		
		get => _health;
		set 
		{
			_health = Math.Clamp(value, 0, _maxHealth);
			if (_healthBar != null) _healthBar.Value = _health;
		}

	}

	public virtual void InitEntity(uint id, int health, int maxHealth, string name, EntityType type, Vector3 pos)
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

	public virtual void TakeDamage(int damage, int actualHealth)
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
