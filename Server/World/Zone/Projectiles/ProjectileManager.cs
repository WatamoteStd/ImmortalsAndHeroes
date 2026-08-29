using Shared.Characters;
using System.Collections;
using System.Numerics;
using Server.World.Zone.Entities;

namespace Server.World.Zone.Projectile;

public class ProjectileManager
{
    
    private Projectile[] _dense = new Projectile[512];
    private ushort[] _sparse = new ushort[ushort.MaxValue + 1];
    public ushort Count {get; private set;} = 0;

    private Queue<ushort> _freeIds = new Queue<ushort>(ushort.MaxValue);


    public ProjectileManager()
    {
        
        Array.Fill(_sparse, ushort.MaxValue);

        for (ushort i = 0; i < ushort.MaxValue; i++)
        {
            _freeIds.Enqueue(i);
        }

    }

    public void AddProjectile(Projectile projectile)
    {
        
        if (Count == _dense.Length)
        {
            Array.Resize(ref _dense, _dense.Length * 2);
        }

        projectile.Id = _freeIds.Dequeue();
        _dense[Count] = projectile;
        _sparse[projectile.Id] = Count;
        Count++;

    }

    public void RemoveProjectile(ushort id)
    {
        
        if (_sparse[id] == ushort.MaxValue) return;

        ushort denseId = _sparse[id];

        _dense[denseId] = _dense[Count - 1];
        _sparse[_dense[denseId].Id] = denseId;

        _sparse[id] = ushort.MaxValue;
        _freeIds.Enqueue(id);

        _dense[Count-1] = default;
        Count--;

    }

    public void Update(float deltaTime)
    {
        
        for (int i = Count -1; i >= 0; i--)
        {
            
            if (_dense[i].Target.IsValidEntity())
            {
                ref var prj = ref _dense[i];

                var distance = Vector3.DistanceSquared(prj.Target.Position, prj.Position);

                if (distance > 1.6f)
                {
                    
                    var direction = Vector3.Normalize(prj.Target.Position - prj.Position);
                    _dense[i].Position += direction * (prj.Speed * deltaTime);

                }
                else
                {
                    prj.Target.TakeDamage(prj.DamageType, prj.Damage, prj.Caster);
                    RemoveProjectile(prj.Id);
                }
                

            }
            else
            {
                RemoveProjectile(_dense[i].Id);
            }

        }

    }

}