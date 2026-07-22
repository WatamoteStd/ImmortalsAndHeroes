using System;
using System.Numerics;

namespace UDPServer.World.Entities;

public class Entity
{
    
    public  uint NetworkId {get; set;}
    public long GlobalId { get; set; }
    public long RegionId {get; set;}

    // GAME INFO & STATS
    public ushort Health {get; set;}
    public Vector3 Position {get; set;} = new Vector3(0, 0, 0);

    public Entity(uint networkId, long regionId, long globalId)
    {
        
        NetworkId = networkId;
        RegionId = regionId;
        GlobalId = globalId;

    }


    public virtual void Update(float deltaTime)
    {
        


    }


}
