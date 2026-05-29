using System;
using IaH.Server.Ability;
using IaH.Server.Entities;
using IaH.Server.World;

namespace IaH.Server.Ability
{
    
    public class ReleaseOfCosmicEnergy : AbilityBase
    {
        
        private float _currentCharges = 0f;
        private float _chargeTimer = 0f;
        private float _maxCharges = 30f;

        public ReleaseOfCosmicEnergy(HeroEntity hero, WorldMatch match) : base(hero, match)
        {
            
            ManaCosts = new float[]{250f, 375f, 500f};
            Cooldowns = new float[] {170f, 130f, 90f};
            Ranges = new float[] {10f, 10f, 10f};
            Type = SkillType.active;
            AbilityGlobalName = Iah.Shared.Packets.AbilityType.ReleaseOfCosmicEnergy;


        }
        public override void Execute(BaseEntity? target, EntityManager em, short targetX, short targetY, short targetZ)
        {
            _entityManager = em;
            if (CurrentCooldown > 0) return;
            _currentCharges = 0f;
            Console.WriteLine($"[RELEASE OF COSMIC ENERGY] SKILL ACTIVATED. CHARGES {_currentCharges}/{_maxCharges}");

        }
        public override void OnUpdate(float deltaTime)
        {
            
            if (_currentCharges >= _maxCharges) 
            {
                OnRelease();
                return;
            }
            _chargeTimer += deltaTime;
            if (_chargeTimer >= 1.0f)
            {
                _chargeTimer = 0f;
                _currentCharges += 1.5f;
                Console.WriteLine($"[RELEASE OF COSMIC ENERGY] CHARGES: {_currentCharges}/{_maxCharges}");

            }
            if (_currentCharges == 30) OnRelease();

        }
        public override void OnRelease()
        {
            
            float absoluteDamage = _currentCharges * 25;

            var targets = _entityManager.GetEntitiesInRange(Caster.GetWorldPosition(), Range);
            foreach (var target in targets)
            {
                
                if (target == Caster) continue;
                target.TakeDamage(absoluteDamage, Caster);
                Console.WriteLine($"[RELEASE OF COSMIC ENERGY] Hit {target.ID} for {absoluteDamage} damage!");
                _match.BroadcastDamage((ushort)absoluteDamage, target.ID);

            }
            
            Console.WriteLine($"[RELEASE OF COSMIC ENERGY] BOOOM! ABSOLUTE DAMAGE: {absoluteDamage}");
            _currentCharges = 0;
            Caster.CurrentState = HeroEntity.State.Idle;
            
            

        }
            



    }

}

