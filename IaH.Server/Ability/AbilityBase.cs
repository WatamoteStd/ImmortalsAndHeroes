using System;
using Iah.Shared.Packets;
using IaH.Server.Entities;
using IaH.Server.World;

namespace IaH.Server.Ability
{
    
    public class AbilityBase
    {
        public enum SkillType {passive, active};
        public SkillType Type {get; protected set;}
        public AbilityType AbilityGlobalName;
        public HeroEntity Caster;

        // OTHER CLASSES
        protected EntityManager _entityManager;
        protected WorldMatch _match;

        public byte CurrentLevel {get; set;} = 1;

        protected float[] ManaCosts;
        protected float[] Cooldowns;
        protected float[] Ranges;
        public AbilityBase(HeroEntity hero, WorldMatch match)
        {
            _match = match;
            Caster = hero;
        }

        public float CurrentCooldown = 0f;
        
        public float Cooldown => Cooldowns[CurrentLevel - 1];
        public float ManaCost => ManaCosts[CurrentLevel -1];
        public float Range => Ranges[CurrentLevel -1];
        public virtual void Execute(BaseEntity? target, EntityManager em, short targetX, short targetY, short targetZ)
        {
            _entityManager = em;
        }
        public virtual void OnUpdate(float deltaTime)
        {
            
        }
        public virtual void OnRelease() {
            
        }        
            
        

    }

}