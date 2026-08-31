
using System.Numerics;
using Server.World.Zone;
using Server.World.Zone.Entities;
using Shared.Ability;

namespace Server.World.Ability;

public class AbilityBase
{
    
    public AbilityTypes Id {get; protected set;}
    public AbilityData DllData {get; protected set;} 
    protected LivingEntity? _caster;
    protected WorldZone? _region;
    
    public float CurrentCooldown {get; protected set;}
    

    public AbilityBase(AbilityTypes abilityId)
    {

        if (AbilityRegistry.TryGetAbility(abilityId, out var dll))
        {
            DllData = dll;
            Id = abilityId;
        }
          
    }

    public virtual void OnApply(LivingEntity caster, Vector3? targetPos, LivingEntity? targetEntity, WorldZone region)
    {
        
        _caster = caster;
        _region = region;

    }
    public virtual void OnUpdate(float deltaTime)
    {
        CurrentCooldown -= deltaTime;
    }


}