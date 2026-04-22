using System;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Networking;

namespace IaH.Server.Core
{
    internal class EntityManager
    {

        private EntityPositionPacket[] _dense = new EntityPositionPacket[1000]; // плотный массив

        private int[] _sparse = new int[1000]; // index = ID | значени = ondex in _dense;

        private int _count = 0;

        public int AddEntity(ushort id, short x, short z)
        {

            if (_count >= 1000) return -1; // обработать через default что -1 означает отсутсвие места

            _dense[_count].EntityId = id;
            _dense[_count].X = x;
            _dense[_count].Z = z;

            _sparse[id] = _count;
            _count++;
            return id;


        }
        
        public void RemoveEntity(ushort idToRemove)
        {


            int indexInDense = _sparse[idToRemove];

            ushort lastEntityId = _dense[_count - 1].EntityId;
            _dense[indexInDense] = _dense[_count - 1];

            _sparse[lastEntityId] = indexInDense;
            _count--;

        }

        public void PrintAllPositions()
        {

            Console.WriteLine($"--- Текущий склад (Живых: {_count}) ---");
            for (int i = 0; i < _count; i++)
            {
                var e = _dense[i];
                Console.WriteLine($"Полка {i}: [ID {e.EntityId}] Позиция {e.X}, {e.Z}");
            }

        }

    }
}
