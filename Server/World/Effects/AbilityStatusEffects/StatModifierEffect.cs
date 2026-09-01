using System.Numerics;
using Server.World.Ability;
using Server.World.Zone.Entities;
using Shared.MasteryTree.Rewards;
using Shared.StatusEffect;

namespace Server.World.Effects.AbilityStatusEffects;

public class StatModifierEffect : StatusEffectBase
{
    private LivingEntity? _target;
    private float _value;
    private StatType _stat;
    
    public StatModifierEffect(StatType stat, float value, float duration, StatusEffect effect) : base (effect, duration)
    {
        
        _value = value;
        _stat = stat;

    }
    
    public override void OnApply(LivingEntity caster, LivingEntity target, Vector3? targetPos)
    {
       
        _target = target;
        _target.UpdateStat(_stat, _value);

    }
    public override void OnUpdate(float deltaTime)
    {  
        
        Duration -= deltaTime;

    }
    public override void OnLeave()
    {
        if (_target != null && _target.IsAlive)
        {
            _target.UpdateStat(_stat, -_value);
        }

    }

}