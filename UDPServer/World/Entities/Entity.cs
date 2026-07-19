using System;
using System.Numerics;

namespace UDPServer.World.Entities;

public class Entity
{
    
    public readonly long Id;
    public long RegionId {get; set;}

    // GAME INFO & STATS
    public ushort Health {get; set;}
    public Vector3 Position {get; set;} = new Vector3(0, 0, 0);

    public Entity(long id, long regionId)
    {
        
        Id = id;
        RegionId = regionId;

    }


    public virtual void Update(float deltaTime)
    {
        


    }


}
