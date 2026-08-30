

using System.Numerics;
using Server.World.Zone.Entities.Mobs;

namespace Server.World.Zone.RegionController;

public class RegionSpawner
{
    
    public DensityModes Density {get;}
    private float _densitySquared;
    public  int MaxCapacity {get;}


    public float RespawnTime {get;}
    private List<MonsterEntity> _deadMonsters = new();


    public WorldZone Region {get;}
    public IReadOnlyList<MobSpawnConfig> Mobs {get;}



    private Vector3[] _corruptedPoints;
    private int _corruptedCount = 0;


    internal RegionSpawner(DensityModes densityMode, int capacity, WorldZone region, IReadOnlyList<MobSpawnConfig> mobsConfigs, float respawnTime)
    {
        
        Density = densityMode;
        MaxCapacity = capacity;
        Region = region;
        Mobs = mobsConfigs;
        RespawnTime = respawnTime;

        _densitySquared = Density.GetDensityDistanceSq();

        _corruptedPoints = new Vector3[capacity];

        Initialize();

    }

    public void Update(float deltaTime)
    {
        
        for (int i = _deadMonsters.Count -1; i >= 0; i--)
        {
            
            var mob = _deadMonsters[i];

            mob.RespawnTimer -= deltaTime;

            if (mob.RespawnTimer <= 0)
            {
                Region.RespawnMonster(mob, RandomOriginPoint());
                _deadMonsters.RemoveAt(i);
            }
 
        }

    }

    private void Initialize()
    {

        for (int i = 0; i <= Mobs.Count -1; i++)
        {
            
            for(int h = 0; h < Mobs[i].Count; h++)
            {
                
                Vector3 pos = RandomOriginPoint();

                _corruptedPoints[_corruptedCount] = pos;
                _corruptedCount++;

                Region.CreateEntity(Mobs[i].Type, pos);

            }

        }
        

    }

    public void EntityDie(MonsterEntity entity)
    {
        
        if (entity.IsAlive) return;

        entity.RespawnTimer = RespawnTime;
        _deadMonsters.Add(entity);

    }

    private Vector3 RandomSpawnPoint()
    {
        
        float x = (Random.Shared.NextSingle() * 2 - 1) * 120;
        float z = (Random.Shared.NextSingle() * 2 - 1) * 120;

        return new Vector3(x, 1, z);

    }

    private Vector3 RandomOriginPoint()
    {
        
        Vector3 rawPos;

        for (int attempts = 0; attempts < 30; attempts++)
        {
            
            rawPos = RandomSpawnPoint();
            bool isValid = true;

            for(int h = 0; h < _corruptedCount; h++)
            {
            
                if (Vector3.DistanceSquared(rawPos, _corruptedPoints[h]) < _densitySquared)
                { 
                    isValid = false;
                    break;
                }

            }
             if (isValid)
            {
                return rawPos;
            } 

        }
        return RandomSpawnPoint();
        
        
    }

}