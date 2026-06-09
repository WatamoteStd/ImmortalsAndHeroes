using System;
using Iah.Shared.Packets;

namespace IaH.Server.Entities.Interfaces
{
    
    public interface IDamagable
    {
        
        void TakeDamage(float damage, DamageType type, HeroEntity attacker);

    }

}