using System;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Networking;
using IaH.Server.Entities;

namespace IaH.Server.Core
{
    internal class EntityManager
    {

        private BaseEntity[] _dense = new BaseEntity[1000]; // плотный массив

        private int[] _sparse = new int[1000]; // index = ID | значени = ondex in _dense;

        private int _count = 0;

        public void AddEntity(ushort id, short x, short y, short z, CharacterType hero)
        {

            if (_count >= 1000) return; // обработать через default что -1 означает отсутсвие места

            _dense[_count] = new BaseEntity(id, x, y, z, hero);
            _dense[_count].Id = id;
            _dense[_count].X = x;
            _dense[_count].Z = z;

            _sparse[id] = _count;
            _count++;
            


        }
        
        public void RemoveEntity(ushort idToRemove)
        {


            int indexInDense = _sparse[idToRemove];

            ushort lastEntityId = _dense[_count - 1].Id;
            _dense[indexInDense] = _dense[_count - 1];

            _sparse[lastEntityId] = indexInDense;
            _count--;

        }

        // DEBUG 
        public void EntityCount()
        {
            Console.WriteLine($"ENTITY IN SERVER: {_count}");
        }
        

    }
}
