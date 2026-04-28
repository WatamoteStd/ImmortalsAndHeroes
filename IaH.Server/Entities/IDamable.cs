using IaH.Shared.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Server.Entities
{
    public interface IDamable
    {
        public void TakeDamage(DamageType type, float damage);
    }
}
