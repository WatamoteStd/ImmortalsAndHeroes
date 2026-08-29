
using System.Numerics;
using Server.World.Zone.Entities;
using Shared.Characters;

namespace Server.World.Zone.Projectile;

public struct Projectile
{
    
    public required ushort Id {get; set;}
    public Vector3 Position {get; set;}
    public float Speed {get; set;}
    public LivingEntity Target;
    public LivingEntity Caster;
    public float Damage;
    public DamageTypes DamageType;
    

}