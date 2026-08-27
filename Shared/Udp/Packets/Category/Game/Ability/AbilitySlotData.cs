
using System.Runtime.InteropServices;
using Shared.Ability;

namespace Shared.Udp.Packets.Category.Game.Ability;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AbilitySlotData
{
    public AbilityTypes AbilityId;
    public float CooldownRemaining;
}