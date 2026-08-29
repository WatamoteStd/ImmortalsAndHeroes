
using Shared.Ability.Params;
using Shared.Characters;
using Shared.MasteryTree.Rewards;

namespace Shared.Ability;

public readonly struct AbilityData
{
    
    public required AbilityTypes AbilityId {get; init;}
    public required string Title {get; init;}
    public required string IconPath { get; init; } 
    public required string ScenePath {get; init;}
    public required string Description {get; init;}
    public required AbilityCastType CastType {get; init;}
    public required AbilityAdditionalCastType CastTypeAdditional {get; init;}
    public required AbilityTarget TargetType {get; init;}
    public required AbilityTargetRelation TargetRelation {get; init;}

    // STATS
    public required float ManaCost {get; init;}
    public required float MpsCost {get; init;}
    public required float Cooldown {get; init;}
    public required float Radius {get; init;}
    public required float CastRange {get; init;}
    public required float CastTime {get; init;}
    public required bool IsInterruptible {get; init;}
    public required bool IsMoveWhileCast {get; init;}
    public required float Duration {get; init;}
    public required float MoveSpeed {get; init;}

    public required DamageTypes DamageType {get; init;}
    public required StatType ScaleStat {get; init;}
    public required float ScalePercent {get; init;}



}

