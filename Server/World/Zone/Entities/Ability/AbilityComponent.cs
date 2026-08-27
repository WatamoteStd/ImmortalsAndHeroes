using Shared.MasteryTree.Rewards;
using Server.World.Ability;
using Shared.Ability.Params;
using System.Numerics;
using Shared.Ability;
using Shared.Udp.Packets.Category.Game.Ability;

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
                _abilities[i] = new AbilityBase(abilityId);
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

    public bool TryCast(int slot, Vector3? targetPos = null, LivingEntity? targetEntity = null)
    {
        
        if (slot < 0 || slot >= _abilities.Length) return false;
        if (_abilities[slot] == null) return false;
        if (_abilities[slot].CurrentCooldown > 0) return false;
        if (_abilities[slot].DllData.ManaCost > _owner.Mana) return false;

       _owner.UpdateStat(StatType.Mana, -_abilities[slot].DllData.ManaCost);
       _abilities[slot].OnApply(_owner, targetPos, targetEntity);
       return true;

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

}