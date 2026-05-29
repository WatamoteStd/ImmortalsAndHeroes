using System;

namespace IaH.Server.Entities.Interfaces
{
    
    public interface IDamagable
    {
        
        void TakeDamage(float damage, HeroEntity attacker);

    }

}