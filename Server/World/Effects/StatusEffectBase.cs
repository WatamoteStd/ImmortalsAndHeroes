
using System.Numerics;
using Server.World.Zone.Entities;
using Shared.StatusEffect;

namespace Server.World.Effects;

public abstract class StatusEffectBase
{
    public float Duration {get; protected set;}
    public StatusEffect EffectId {get; protected set;}

    public StatusEffectBase(StatusEffect effect, float duration)
    {
        EffectId = effect;
        Duration = duration;
    }
    
    public abstract void OnApply(LivingEntity caster, LivingEntity target, Vector3? targetPos);
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnLeave();

}