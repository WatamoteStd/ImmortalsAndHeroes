using System.Numerics;
using Server.World.Zone.Entities;
using Shared.Characters;
using Shared.StatusEffect;

namespace Server.World.Effects;

public class DamagePerTimeEffect : StatusEffectBase
{
    private ushort _maxStacks;
    private ushort _curStacks;
    private float _damage;
    private float _damageInterval;
    private float _curDamageInterval;
    private DamageTypes _dmgType;
    private float _maxDuration;
    private LivingEntity? _target;
    private LivingEntity? _caster;
    
    public DamagePerTimeEffect(StatusEffect effect, float duration, ushort maxStack, float damage, float damageInterval, DamageTypes dmgType, bool isStackable = true) : base(effect, duration)
    {
        
        _maxStacks = maxStack;
        _damage = damage;
        _damageInterval = damageInterval;
        _dmgType = dmgType;
        _maxDuration = duration;

    }

    public override void OnApply(LivingEntity caster, LivingEntity target, Vector3? targetPos)
    {
        
        if (_curStacks < _maxStacks) _curStacks++;
        Duration = _maxDuration;
        _caster = caster;
        _target = target;

    }

    public override void OnUpdate(float deltaTime)
    {
        
        if (!_target.IsValidEntity())
        {
            _curStacks = 0;
            Duration = 0.0f;
            return;
        }

        Duration -= deltaTime;
        _curDamageInterval += deltaTime;

        if (_curDamageInterval >= _damageInterval)
        {
            
            _curDamageInterval = 0.0f;
            float finalDmg = _curStacks * _damage;
            _target!.TakeDamage(_dmgType, finalDmg, _caster!);

        }


    }

    public override void OnLeave()
    {
        
        

    }

}