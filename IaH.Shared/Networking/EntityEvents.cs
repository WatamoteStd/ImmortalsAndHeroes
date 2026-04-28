using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Shared.Networking.Events
{
    
    public struct EntityHpChangedEvent
    {
        public ushort EntityId;
        public float NewHp;

        public EntityHpChangedEvent(ushort id, float hp)
        {
            EntityId = id;
            NewHp = hp;
        }

    }

}
