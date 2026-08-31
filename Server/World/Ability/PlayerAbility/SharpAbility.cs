
using System.Numerics;
using Server.World.Zone;
using Server.World.Zone.Entities;
using Shared.Ability;

namespace Server.World.Ability;

public class SharpAbility : AbilityBase
{

    private float _damageDelay = 0.5f;
    private bool _isAttackCommited = false;
    private bool _isCasting = false;

    
    public SharpAbility(AbilityTypes abilityId) : base(abilityId)
    {
        


    }

    public override void OnApply(LivingEntity caster, Vector3? targetPos, LivingEntity? targetEntity, WorldZone region)
    {
        base.OnApply(caster, targetPos, targetEntity, region);

        _damageDelay = 0.5f;
        _isAttackCommited = false;
        _isCasting = true;

        CurrentCooldown = DllData.Cooldown;

    }

    public override void OnUpdate(float deltaTime)
    {

        if (CurrentCooldown > 0)
        {
            base.OnUpdate(deltaTime);
        }

        if (!_isAttackCommited && _isCasting)
        {
            
            _damageDelay -= deltaTime;

            if (_damageDelay <= 0f)
            {
                
                _isAttackCommited = true;
                DealDamage();

            }

        }


    }

    private void DealDamage()
    {
        
        
        var enemies = _region!.FindEntityInRadius(_caster!.Position, DllData.Radius);

            foreach(var ent in enemies)
            {
                
                if (!ent.IsValidEntity() || ent == _caster) continue;

                ent.TakeDamage(DllData.DamageType, 90, _caster);

            }

    }


}