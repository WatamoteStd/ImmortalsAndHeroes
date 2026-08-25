
using System.Numerics;
using Server.World.Zone.Entities;
using Shared.Ability;

namespace Server.World.Ability;

public class AbiltiyBase
{
    
    public AbilityTypes Id {get; protected set;}
    public AbilityData DllData {get; protected set;} 
    

    public AbiltiyBase(AbilityTypes abilityId)
    {
        if (AbilityRegistry.TryGetAbility(abilityId, out var dll))
        {
            DllData = dll;
            
        }
            
        
    }

    public virtual void OnApply(LivingEntity caster, Vector3? targetPos, LivingEntity? targetEntity)
    {
        
    }
    public virtual void OnUpdate(float deltaTime)
    {
        
    }
    public virtual void OnLeave()
    {
        
    }

}