
using System.Collections.Frozen;
using Shared.Ability;
using Shared.Ability.Params;
using Shared.MasteryTree.Rewards;

namespace Shared.Ability;

public static class AbilityRegistry
{
    
    private static FrozenDictionary<AbilityTypes, AbilityData> _abilities = FrozenDictionary<AbilityTypes, AbilityData>.Empty;
    public readonly static uint Count;

    static AbilityRegistry()
    {
        
        var list = new AbilityData[]
        {
            
            new AbilityData
            {
                AbilityId = AbilityTypes.DefaulthRun, Title = "Run", IconPath = "res://Assets/Icons/Ability/Run/RunIcon.png", ScenePath = "res://AbilityScenes/StaticAbilities/DefaulthRunAbility/DefaultRunAbility.tscn",
                Description = "Just run faster. Increase move speed at 3", CastType = AbilityCastType.NonTarget, CastTypeAdditional = AbilityAdditionalCastType.None, TargetType = AbilityTarget.Self,
                TargetRelation = AbilityTargetRelation.None, ManaCost = 25, MpsCost = 0, Cooldown = 22f, Radius = 0f, CastRange = 0f,
                CastTime = 0f, IsInterruptible = false, IsMoveWhileCast = false, Duration = 5f, DamageType = Characters.DamageTypes.None,
                ScaleStat = StatType.None, ScalePercent = 1.0f, MoveSpeed = 0.0f
            },
            new AbilityData
            {
                AbilityId = AbilityTypes.ZoneOfBlood, Title = "Zone Of Blood", IconPath = "res://Assets/Icons/Ability/ZoneOfBlood.png", ScenePath = "",
                Description = "Zone of blood that give damage to all enemy every 0.5s", CastType = AbilityCastType.NonTarget, CastTypeAdditional = AbilityAdditionalCastType.Point, TargetType = AbilityTarget.None,
                CastTime = 0f, IsInterruptible = true, IsMoveWhileCast = false, Duration = 8f, DamageType = Characters.DamageTypes.Magic, 
                ScaleStat = StatType.None, ScalePercent = 1.0f, MoveSpeed = 0.0f, CastRange = 7.0f, Cooldown = 15f, ManaCost = 50f, MpsCost = 0f, Radius = 10f, TargetRelation = AbilityTargetRelation.Any
            }

        };

        _abilities = list.ToFrozenDictionary(b => b.AbilityId);
        Count = (uint)list.Length;

    }

      public static bool TryGetAbility(AbilityTypes id, out AbilityData ability)
        {
        return _abilities.TryGetValue(id, out ability);
        }


}