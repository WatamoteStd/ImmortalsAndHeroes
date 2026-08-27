using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Shared.Ability;
using Shared.Udp.Interfaces;

namespace Shared.Udp.Packets.Category.Game.Ability;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct S2C_PlayerAbilitySyncPacket : INetworkPacket
{
    public int Length {get; private set;}

    public AbilitySlotData Slot0;
    public AbilitySlotData Slot1;
    public AbilitySlotData Slot2;
    public AbilitySlotData Slot3;
    public AbilitySlotData Slot4;
    public AbilitySlotData Slot5;



    public int Serialize(Span<byte> buffer)
    {
        
        ReadOnlySpan<byte> bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref this, 1));
        bytes.CopyTo(buffer);

        Length = bytes.Length;
        return Length;

    }
    public void Deserialize(ReadOnlySpan<byte> buffer)
    {
        
        this = MemoryMarshal.Read<S2C_PlayerAbilitySyncPacket>(buffer);
        Length = Unsafe.SizeOf<S2C_PlayerAbilitySyncPacket>();

    }
  

}