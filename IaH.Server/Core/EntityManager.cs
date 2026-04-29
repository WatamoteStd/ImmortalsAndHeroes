using System;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Networking;
using IaH.Server.Entities;
using IaH.Shared.Data;
using System.Threading.Channels;

namespace IaH.Server.Core
{
    public class EntityManager
    {

        private BaseEntity[] _dense = new BaseEntity[1000]; // плотный массив

        private int[] _sparse = new int[1000]; // index = ID | значени = index in _dense;

        private int _count = 0;

        public EntityManager ()
        {
            Array.Fill(_sparse, -1);
        }

        public BaseEntity GetEntity(int entityId)
        {
            if (entityId < 0 || entityId >= _dense.Length) return null;

            int indexInDense = _sparse[entityId];

            if (indexInDense >= _count || _dense[indexInDense] == null || _dense[indexInDense].Id != entityId) return null;

            BaseEntity _entity = _dense[indexInDense];
            return _entity;
        }

        public IEnumerable<BaseEntity> GetActiveEntities()
        {
            for (int i = 0; i < _count; i++)
            {

                if (_dense != null)
                {
                    yield return _dense[i];
                }

            }
        }

        public void AddEntity(ushort id, short x, short y, short z, CharacterType entity)
        {

            if (_count >= 1000) return; // обработать через default что -1 означает отсутсвие места

            var config = HeroDataManager.GetConfig(entity);

            if (config == null)
            {
                Console.WriteLine("EntityManager: Error. No hero config. Check JSON...");
                return;
            }

            _dense[_count] = new Hero(id, x, y, z, entity, config);       
            _sparse[id] = _count;
            _count++;



        }

        public void RemoveEntity(ushort idToRemove)
        {

            
            int indexInDense = _sparse[idToRemove];
            if (indexInDense == -1)
            {
                Console.WriteLine("[EntityManager] RemoveEntity: index don't exist's.");
                return;
            }

            ushort lastEntityId = _dense[_count - 1].Id;
            _dense[indexInDense] = _dense[_count - 1];
            _sparse[lastEntityId] = indexInDense;
            _sparse[idToRemove] = -1;
            _count--;

        }

    }
}
