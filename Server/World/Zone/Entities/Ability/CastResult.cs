
using System.Numerics;
using Shared.Ability;
using Shared.Ability.CastErrors;

namespace Server.World.Zone.Entities.Ability;

public struct CastResult
{
    
    public bool IsSucces;
    public AbilityTypes AbilityId;
    public float FinalCooldown;
    public AbilityCastErrors Error;
    public byte Slot;
    public Vector3 CastPosition;
    public uint EnemyId;

}