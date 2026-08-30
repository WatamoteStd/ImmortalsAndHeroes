
using System.Numerics;
using Server.World.Zone.Entities;
using Shared.Characters;
using Shared.ProjectilesData;

namespace Server.World.Zone.Projectiles;

public struct Projectile
{
    
    public ushort Id;
    public Vector3 Position;
    public float Speed;
    public LivingEntity Target;
    public LivingEntity Caster;
    public float Damage;
    public DamageTypes DamageType;
    public ProjectileType Type;

    public float Radius;
    public float Height;
    

}