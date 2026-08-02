using System.Numerics;


namespace UDPServer.World.Entities.Enemy;

public class OrkEnemy : Entity
{
    
     public OrkEnemy(uint networkId, long regionId, long globalId) : base(networkId, regionId, globalId)
    {
        
        NetworkId = networkId;
        RegionId = regionId;
        GlobalId = globalId;

        Position = new Vector3(10, 1, 10);
        Health = 200;

    }

}