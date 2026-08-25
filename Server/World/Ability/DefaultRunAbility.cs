using System.Numerics;
using Server.World.Zone.Entities;
using Shared.Ability;
using Shared.MasteryTree.Rewards;

namespace Server.World.Ability;

public class DefaultRunAbility : AbiltiyBase
{
    public float _timeLeft {get; private set;}
    private float _baseCasterSpeed;
    
    public DefaultRunAbility(AbilityTypes abilityId) : base (abilityId)
    {
        
        _timeLeft = DllData.Duration;


    }

    public override void OnApply(LivingEntity caster, Vector3? targetPos, LivingEntity? targetEntity)
    {
        
        _baseCasterSpeed = caster.BaseSpeed;
        caster.UpdateStat(StatType.MoveSpeed, caster.BaseSpeed);

    }
    
    

}