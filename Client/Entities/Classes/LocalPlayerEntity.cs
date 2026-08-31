using Godot;
using Shared.Characters;
using Shared.Udp.Packets.Category.Game;
using System;

public partial class LocalPlayerEntity : Entity
{
	
	public enum State {Idle, Move, Attack, Chase, Cast, Strunned, ProtectedCast, Dead}
	public State CurrentState {get; protected set; } = State.Idle;
	[Export] private Label _stateLabel;
	private float _stateTimeGone;

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



	public void SetMoveTarget(Vector3 position)
	{
		
		_moveTarget = position;
		_attackTarget = null;
		CurrentState = State.Move;
		_stateTimeGone = 0.0f;

	}
	public void SetAttackTarget(Entity target)
	{
		
		_attackTarget = target;
		CurrentState = State.Chase;

	}

	public override void ServerMove(Vector3 serverPosition)
	{
		
		if (GlobalPosition.DistanceSquaredTo(serverPosition) > 6.25f)
	{
		GD.Print($"[DESYNC] Client: {GlobalPosition} | Server: {serverPosition}. Hard sync!");
		GlobalPosition = serverPosition;
	}

	}




	public override void _PhysicsProcess(double delta)
	{
		Regenerate((float)delta);

		if (_currentAttackCooldown > 0)
		{
			_currentAttackCooldown -= (float)delta;
		}

		
		switch(CurrentState)
		{
			
			case State.Idle:
				{
					_stateTimeGone += (float)delta;
					_stateLabel.Text = $"Idle: {_stateTimeGone:F1}";
				}
			break;

			case State.Move:
				{
					
					Vector3 toTarget = _moveTarget - GlobalPosition;
					toTarget.Y = 0;

					float distanceSq = toTarget.LengthSquared();

					if (distanceSq > 0.005f)
					{
						
						Move((float)delta);
						_stateTimeGone += (float)delta;
						_stateLabel.Text = $"Move: {_stateTimeGone:F1}";

					}
					else
					{
						
						Velocity = Vector3.Zero;
						MoveAndSlide();

						CurrentState = State.Idle;
						_stateTimeGone = 0.0f;
						_stateLabel.Text = $"Idle: {_stateTimeGone:F1}";

					}

				}
			break;

			case State.Chase:
				{
					
					if (!IsInstanceValid(_attackTarget))
					{
						CurrentState = State.Idle;
						return;
					}
					if (IsInAttackRadius(_attackTarget))
					{
						CurrentState = State.Attack;
						return;
					}

					_stateTimeGone += (float)delta;
					_stateLabel.Text = $"Chase: {_stateTimeGone:F1}";

					_moveTarget = _attackTarget.GlobalPosition;
					Move((float)delta);

				}
			break;

			case State.Attack:
				{
					
					if (!IsInstanceValid(_attackTarget)) {CurrentState = State.Idle; return;}
					if (!IsInAttackRadius(_attackTarget)) {CurrentState = State.Chase; return;}

					Velocity = Vector3.Zero;

					_stateTimeGone += (float)delta;
					_stateLabel.Text = $"Attack:{_currentAttackCooldown:F1}";

					if (_currentAttackCooldown > 0) return;

					_currentAttackCooldown = _attackCooldown;

				}
			break;

		}

	}

	private void Move(float delta)
	{
		
		Vector3 direction = (_moveTarget - GlobalPosition);
		direction.Y = 0;

		float distance = direction.Length();
		direction = direction.Normalized();

		float currentSpeed = MathF.Min(_speed, distance / delta);
		Velocity = direction * currentSpeed;

		MoveAndSlide();

	}



	public void UpdateStats( in S2C_StatsSyncPacket data)
	{

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
