using Shared.StatusEffect;
using System.Numerics;
using Server.World.Effects;
using Server.World.Zone;
using Server.World.Zone.Entities;
using Shared.Ability;
using Shared.Characters;

namespace Server.World.Ability.PlayerAbility;

public class PokeAbility : AbilityBase
{
    
    public PokeAbility(AbilityTypes abilityId) : base (abilityId)
    {
        
    }

    public override void OnApply(LivingEntity caster, Vector3? targetPos, LivingEntity? targetEntity, WorldZone region)
    {
        base.OnApply(caster, targetPos, targetEntity, region);


        if (targetEntity == null || !targetEntity.IsValidEntity()) {Console.WriteLine("Target entity is invalid. Poke ability responsee"); return;}

        targetEntity.TakeDamage(DamageTypes.Physical, 15f, caster);
        DamagePerTimeEffect dot = new DamagePerTimeEffect(StatusEffect.BloodDot, DllData.Duration, 4, 5f, 1.0f, DamageTypes.Physical);
        targetEntity.ApplyStatusEffect(dot, caster);
        CurrentCooldown = DllData.Cooldown;

    }

}