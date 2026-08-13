

using Server.Pools.Session;
using Shared.Udp.Packets;

namespace Server.Network.Interfaces;
public struct NetworkCommand
    {
        public UserSession Session;
        public byte[] Data;
        public PacketTypes PacketType;
        public ushort Length;
    }