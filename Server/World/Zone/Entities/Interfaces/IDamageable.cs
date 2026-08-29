using Server.World.Zone.Entities;
using Shared.Characters;

namespace Server.World.Zone.Intefaces;

public interface IDamageable
{
    
    void TakeDamage(DamageTypes dmgType, float damage, LivingEntity attacker);

}