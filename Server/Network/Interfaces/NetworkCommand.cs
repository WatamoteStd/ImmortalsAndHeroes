

using Server.Pools.Session;

namespace Server.Network.Interfaces;
public struct NetworkCommand
    {
        public UserSession Session;
        public byte[] Data;
        public ushort Length;
    }