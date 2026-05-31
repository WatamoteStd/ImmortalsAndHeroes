using System;
using Iah.Shared.Packets;

namespace Iah.Shared.Entities
{
    
    public static class EntityRegistry
    {
        
        // MAGES
        public static readonly EntityStats VoidlessStar = new EntityStats(270f, 4f, 19f, 200f);
        public static readonly EntityStats Lilith = new EntityStats(295f, 4.3f, 23f, 155f);

        // DD & RDD
        public static readonly EntityStats Frozen = new EntityStats(322f, 4.2f, 28f, 105f);
        public static readonly EntityStats Ozonid = new EntityStats(355f, 5.0f, 23f, 115f);

        // TANK
        public static readonly EntityStats Whip = new EntityStats(410f, 3.8f, 37f, 95f);

        public static EntityStats GetStats(UnitList unit)
        {
            return unit switch
            {
                UnitList.VoidlessStar => VoidlessStar,
                UnitList.Frozen => Frozen,
                UnitList.Whip => Whip,
                UnitList.Ozonid => Ozonid,
                UnitList.Lilith => Lilith,
                _ => new EntityStats(200f, 5.0f, 0f, 0f) // def stats
            };
        }
       

    }

}