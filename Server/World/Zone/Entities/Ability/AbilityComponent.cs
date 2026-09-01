using Shared.MasteryTree.Rewards;
using Server.World.Ability;
using Shared.Ability.Params;
using System.Numerics;
using Shared.Ability;
using Shared.Udp.Packets.Category.Game.Ability;
using Shared.Ability.CastErrors;
using System.Runtime.Intrinsics;
using Server.World.Ability.PlayerAbility;

namespace Server.World.Zone.Entities.Ability;

public class AbilityComponent
{
    
    private AbilityBase[] _abilities;
    private LivingEntity _owner;

    public event Action? OnAbilityUpdate;

    public AbilityComponent(int slotCount, LivingEntity owner)
    {
        
        _abilities = new AbilityBase[slotCount];
        _owner = owner;

    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < _abilities.Length; i++)
        {
            _abilities[i]?.OnUpdate(deltaTime);
        }
    }

    public int AddAbility(AbilityTypes abilityId)
    {

        for (int i = 0; i < _abilities.Length; i++)
        {
   
            if (_abilities[i] != null && _abilities[i].Id == abilityId)
            {
                return -1; 
            }
        }
        
        for (int i = 0; i < _abilities.Length; i++)
        {
            if (_abilities[i] == null)
            {
                _abilities[i] = CreateAbilityInstance(abilityId);
                OnAbilityUpdate?.Invoke();
                return i;
            }
            
        }
        return -1;

    }

    public bool SetAbilityToSlot(int slot, AbilityBase ability)
    {
        
        if (slot < 0 || slot >= _abilities.Length)
            return false;

        if (_abilities[slot] != null) return false;
        _abilities[slot] = ability;
        OnAbilityUpdate?.Invoke();
        return true;

    }

    public CastResult TryCast(int slot, WorldZone region, Vector3? targetPos = null, LivingEntity? targetEntity = null)
    {
        
        if (slot < 0 || slot >= _abilities.Length) return new CastResult {Error = AbilityCastErrors.AbilityNotFound, IsSucces = false};
        if (_abilities[slot] == null) return new CastResult {Error = AbilityCastErrors.AbilityNotFound, IsSucces = false};
        if (_abilities[slot].CurrentCooldown > 0) return new CastResult {Error = AbilityCastErrors.OnCooldown, IsSucces = false};
        if (_abilities[slot].DllData.ManaCost > _owner.Mana) return new CastResult {Error = AbilityCastErrors.NoMana, IsSucces = false};

        
        var abl = _abilities[slot];

        if (abl.DllData.CastType == AbilityCastType.Target && targetEntity is null) return new CastResult{Error = AbilityCastErrors.InvalidTarget, IsSucces = false};
        if (abl.DllData.CastTypeAdditional != AbilityAdditionalCastType.None && targetPos == Vector3.Zero) return new CastResult{ Error = AbilityCastErrors.InvalidPoint, IsSucces = false};


        Vector3 realPosition = _owner.Position;

        if (abl.DllData.CastType == AbilityCastType.Target) realPosition = targetEntity!.Position;
        if (abl.DllData.CastType == AbilityCastType.NonTarget) realPosition = targetPos ?? _owner.Position;


        if (abl.DllData.TargetType != AbilityTarget.Self)
        {
            
            if (!_owner.IsInRadius(_owner.Position.X, _owner.Position.Z, _owner.Radius, realPosition.X, realPosition.Z, targetEntity != null ? targetEntity.Radius : 0f, abl.DllData.CastRange))
            {
                if (_owner is PlayerEntity player)
                player.MoveToPosition(realPosition);
                return new CastResult { Error = AbilityCastErrors.None, IsSucces = false};
            
            };

        }

       _owner.UpdateStat(StatType.Mana, -abl.DllData.ManaCost);
       abl.OnApply(_owner, targetPos, targetEntity, region);

        if (abl.DllData.CastType == AbilityCastType.Target)
        {
            return new CastResult { AbilityId = abl.Id, CastPosition = Vector3.Zero, EnemyId = targetEntity!.EntityId, FinalCooldown = abl.CurrentCooldown, IsSucces = true, Slot = (byte)slot};
        }
        else
        {
            return new CastResult { AbilityId = abl.Id, CastPosition = realPosition, EnemyId = 0, FinalCooldown = abl.CurrentCooldown, IsSucces = true, Slot = (byte)slot};

        }
       

    }

    public AbilitySlotData GetSlot(int index)
    {
        
        if (index < 0 || index >= _abilities.Length)
        return default;

        var ability = _abilities[index];
        if (ability == null) return default;

        return new AbilitySlotData
        {
            AbilityId = ability.Id,
            CooldownRemaining = ability.CurrentCooldown
        };

    }


    private AbilityBase CreateAbilityInstance(AbilityTypes abilityId)
    {
        
        return abilityId switch
        {
            
            AbilityTypes.DefaulthRun => new DefaultRunAbility(abilityId),
            AbilityTypes.Sharp => new SharpAbility(abilityId),
            AbilityTypes.Poke => new PokeAbility(abilityId),
            _ => new AbilityBase(abilityId)

        };

    }

}