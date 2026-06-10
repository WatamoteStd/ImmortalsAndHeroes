using System;
using Iah.Shared.Packets;

namespace Iah.Shared.Entities
{
    
    public static class EntityRegistry
    {
        
        // MAGES
        public static readonly EntityStats VoidlessStar = new EntityStats(0.9f, 2.3f, 225f, 0.65f, 3.7f, 22f, 200, 
        1.1f, 1.7f,50f,1,15f, AttackTypes.Ranged);
        public static readonly EntityStats Lilith = new EntityStats(1f, 2.1f, 210f, 0.5f, 3.6f, 23f, 230, 
        1.3f, 1.6f,50f,2,13f, AttackTypes.Ranged);

        // DD & RDD
        public static readonly EntityStats Frozen = new EntityStats(0.8f, 2.1f, 240f, 0.55f, 3.5f, 29f, 152, 
        1.15f, 1.55f, 50f, 3f , 20f, AttackTypes.Ranged);
        public static readonly EntityStats Ozonid = new EntityStats(1f, 2.2f, 275f, 0.7f, 3.9f, 21f, 140, 
        1.25f, 1.45f, 50f, 3, 13f, AttackTypes.Ranged);

        // TANK
        public static readonly EntityStats Whip = new EntityStats(1.2f, 2.6f, 420f, 1.1f, 3.75f, 27f, 121, 
        0.95f, 1.75f,50f,4, 3f, AttackTypes.Melee);

        public static EntityStats GetStats(UnitList unit)
        {
            return unit switch
            {
                UnitList.VoidlessStar => VoidlessStar,
                UnitList.Frozen => Frozen,
                UnitList.Whip => Whip,
                UnitList.Ozonid => Ozonid,
                UnitList.Lilith => Lilith,
                _ => new EntityStats(1.2f, 2.6f, 67f, 1.1f, 3.75f, 27f, 121, 
        0.95f, 1.75f,50f,4,20f, AttackTypes.Melee) //def 
            };
        }
       

    }

}