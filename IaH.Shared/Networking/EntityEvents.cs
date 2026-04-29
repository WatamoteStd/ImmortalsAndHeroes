using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Shared.Networking.Events
{
    
    public struct EntityHpChangedEvent
    {
        public ushort EntityId;
        public float NewHp;
        public float NewMp;

        public EntityHpChangedEvent(ushort id, float hp, float mp)
        {
            EntityId = id;
            NewHp = hp;
            NewMp = mp;
        }

    }

}
