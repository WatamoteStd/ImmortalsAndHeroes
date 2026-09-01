using Godot;
using Shared.Ability;
using Shared.Udp.Packets.Category.Game.Ability;
using Shared.Ability.Params;
using System;
using System.Formats.Tar;

public partial class PlayerAbilityController : Node
{
	[Export] private LocalPlayerEntity _player;
	public static event Action OnAbilitiesSynced;
	public static event Action<byte, float> OnAbilityReloadStarted; // slot & duration
	public static AbilitySlotData[] Slots {get; private set;} = new AbilitySlotData[6];

	private ExecutableCommand _lastCommand = new ExecutableCommand(0, Vector3.Zero, null);
	private float _timeFromLastCheck;



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

	public void UpdateSingleAbility(S2C_CastAbilitySuccessfulPacket data)
	{
		
		Slots[data.Slot].CooldownRemaining = data.CurrentCooldown;
		if (data.CurrentCooldown > 0)
		{
			OnAbilityReloadStarted?.Invoke(data.Slot, data.CurrentCooldown);
		}

	}




	public void ExecuteSkill(byte slot, Vector3 pos, Entity entity)
	{
	
		if (slot >= Slots.Length || Slots[slot].AbilityId == 0) 
		{
			return;
		}

		if (IsCanCast(slot, pos, entity))
		{

			uint entityId = entity == null ? 0 : entity.Id;

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

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;

		for (int i = 0; i < Slots.Length; i++)
		{
			if (Slots[i].CooldownRemaining > 0)
			{
				Slots[i].CooldownRemaining -= dt;

				if (Slots[i].CooldownRemaining < 0)
				{
					Slots[i].CooldownRemaining = 0;
				}
				
			}
		}

		if (_lastCommand.IsActual)
		{
			
			_timeFromLastCheck += dt;

			if (_timeFromLastCheck >= 0.2f)
			{
				
				ExecuteSkill(_lastCommand.Slot, _lastCommand.Pos, _lastCommand.TarEntity);
				_timeFromLastCheck = 0.0f;

			}

		}

	}






	private bool IsCanCast(byte slot, Vector3 pos, Entity entity)
	{
		
		var abData = Slots[slot];

		if (abData.CooldownRemaining > 0) return false;

		if (!AbilityRegistry.TryGetAbility(abData.AbilityId, out var dllData)) return false;
		
		

		if (dllData.CastType == AbilityCastType.Target && entity == null) {GD.Print("Can't cast target skill. Target is null"); return false;}
			
		if (dllData.CastTypeAdditional != AbilityAdditionalCastType.None && pos == Vector3.Zero) return false;

		if (dllData.TargetType == AbilityTarget.Self)
		{
			return true;
		}


		// DISTANCE CHECKOUT


		Vector3 tarPos = Vector3.Zero;
		bool isInCastR = false;
		bool isEntityTargetSkill = false;

		if (dllData.CastType == AbilityCastType.Target)
		{
			
			isInCastR = _player.IsInRadius(_player.GlobalPosition.X, _player.GlobalPosition.Z, _player.Radius,	
				entity.GlobalPosition.X, entity.GlobalPosition.Z, entity.Radius, dllData.CastRange);

			isEntityTargetSkill = true;
			tarPos = entity.GlobalPosition;

		}
		else if (dllData.CastTypeAdditional != AbilityAdditionalCastType.None)
		{
			
			isInCastR = _player.IsInRadius(_player.GlobalPosition.X, _player.GlobalPosition.Z, _player.Radius,	
				pos.X, pos.Z, 0.0f, dllData.CastRange);

			tarPos = pos;

		}

		if (isInCastR) return true;

		ExecutableCommand cmd = new ExecutableCommand();
		cmd.Slot = slot;

		if (!isEntityTargetSkill)
		{

			if (!_lastCommand.IsActual)
			{
				
				cmd.Pos = tarPos;
				cmd.TarEntity = null;
				cmd.IsEntityTarget = false;
				cmd.IsActual = true;

				_lastCommand = cmd;

				ServerMaster.Instance.LocalPlayerMoveRequest(tarPos);

				_player.SetMoveTarget(tarPos);

			}
			

		}
		else
		{

			float targetShift = entity.GlobalPosition.DistanceSquaredTo(_lastCommand.Pos);

			if (!_lastCommand.IsActual || targetShift > 0.8f)
			{
				
				cmd.Pos = entity.GlobalPosition;
				cmd.TarEntity = entity;
				cmd.IsEntityTarget = true;
				cmd.IsActual = true;

				_lastCommand = cmd;

				ServerMaster.Instance.LocalPlayerMoveRequest(tarPos);

				_player.SetMoveTarget(tarPos);

			}

			
		}
		

		return false;
		

	}

	public void CancelPendingCommand()
	{
		_lastCommand.IsActual = false;
	}
	private struct ExecutableCommand(byte slot, Vector3 pos, Entity entity)
	{
		
		public byte Slot = slot;
		public Vector3 Pos = pos;
		public Entity TarEntity = entity;
		public bool IsActual = false;
		public bool IsEntityTarget = false;

	}

}
