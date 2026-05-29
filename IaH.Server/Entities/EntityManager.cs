using System;
using System.Collections.Generic;
using IaH.Server.Entities;
using IaH.Server.Managers;
using IaH.Server.World;
using IaH.Server.PlayerClasses;
using System.ComponentModel;
using System.Numerics;

namespace IaH.Server.Entities
{
    
    public class EntityManager
    {
        private BaseEntity[] _dense;
        private ushort[] _sparse;
        private ushort _entitiesCount = 0;
        private Queue<ushort> _freeIds;
        
        public EntityManager()
        {
            
            _dense = new BaseEntity[1000];
            _sparse = new ushort[1000];

            _freeIds = new Queue<ushort>();
            for (ushort i = 0; i < 1000; i++)
            {
                _freeIds.Enqueue(i);
            }

        }

        public BaseEntity? EntityById(ushort id)
        {
            if (id >= _sparse.Length) return null;
            ushort index = _sparse[id];
            return _dense[index];
        }

        public void AddEntity(BaseEntity entity)
        {
            entity.ID = _freeIds.Dequeue();
            
            _dense[_entitiesCount] = entity;
            _sparse[entity.ID] = _entitiesCount;
            _entitiesCount++;

        }
        public void RemoveEntity(BaseEntity entity)
        {
            
            ushort targetIndex = _sparse[entity.ID];
            int lastIndex = _entitiesCount - 1;

            if (targetIndex < lastIndex)
            {
                BaseEntity lastEntity = _dense[lastIndex];
                _dense[targetIndex] = lastEntity;
                _sparse[lastEntity.ID] = targetIndex;
            }

            _dense[lastIndex] = null;
            _entitiesCount--;
            _freeIds.Enqueue(entity.ID);

        }

        public List<BaseEntity> GetEntitiesInRange(Vector3 center, float radius)
        {
            
            List<BaseEntity> found = new();
            float scaledRadius = radius * 100f;
            float radiusSquared = scaledRadius * scaledRadius;

            for (int i = 0; i < _entitiesCount; i++)
            {
                
                BaseEntity entity = _dense[i];

                if (entity == null || entity.Health <= 0 ) continue;

                float dx = entity.X - center.X;
                float dy = entity.Y - center.Y;
                float dz = entity.Z - center.Z;
                float distanceSquared = (dx * dx) + (dy * dy) + (dz * dz);

                if (distanceSquared <= radiusSquared)
                {
                    found.Add(entity);
                }

            }
            return found;

        }

    }

}