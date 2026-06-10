using System;
using System.Numerics;
using Iah.Shared.Packets;
using IaH.Server.Ability;
using IaH.Server.Entities;
using IaH.Server.PlayerClasses;
using IaH.Server.World;

namespace IaH.Server.Entities
{
    
    public class HeroEntity : BaseEntity
    {
        public Player MainPlayer;
        
        public HeroEntity(ushort id, UnitList unitType, Team team, Player player, WorldMatch m) : base (id, unitType, team)
        {
            _match = m;
            MainPlayer = player;

        }

       
        public override void TakeDamage(float dmg, DamageType type, BaseEntity attacker)
        {
            base.TakeDamage(dmg, type,attacker);
            Console.WriteLine($"[{Unit}] Take {dmg} damage. Health: {Health}/{GetMaxHealth()}");


    }

}}