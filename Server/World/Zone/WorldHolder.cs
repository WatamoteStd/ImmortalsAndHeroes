using System;
using Server.Network.Interfaces;

namespace Server.World.Zone;

public class WorldHolder
{
    
    private readonly IWorldBroadcaster _broadcaster;

    public WorldHolder(IWorldBroadcaster broadcaster)
    {
        _broadcaster = broadcaster;
    }

}