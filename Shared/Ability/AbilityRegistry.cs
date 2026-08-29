
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