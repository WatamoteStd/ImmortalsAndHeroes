using System.Numerics;
using Server.World.Effects;
using Server.World.Effects.AbilityStatusEffects;
using Server.World.Zone.Entities;
using Shared.Ability;
using Shared.MasteryTree.Rewards;

namespace Server.World.Ability;

public class DefaultRunAbility : AbiltiyBase
{
    
    public DefaultRunAbility(AbilityTypes abilityId) : base (abilityId)
    {
        

    }

    public override void OnApply(LivingEntity caster, Vector3? targetPos, LivingEntity? targetEntity)
    {
        
        var effect = new StatModifierEffect(StatType.HealthRegen, 20, DllData.Duration, DllData.Title);
        caster.ApplyStatusEffect(effect, caster);
        CurrentCooldown = DllData.Cooldown;

    }
    
    

}