using IaH.Shared.Networking;
using IaH.Server.Core;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Threading;
using System.Diagnostics;
using IaH.Server.Entities;

long lastTime = 0;
Stopwatch sw = new Stopwatch();
sw.Start();

Console.WriteLine("---IaH Server Starting---");

// Передай его в конструктор!
NetworkManager _netManager = new NetworkManager();

_netManager.Start();


while (true)
{
    var currenTime = sw.ElapsedMilliseconds;
    float deltaTimeMS = currenTime - lastTime;
    lastTime = currenTime;
    float deltaTime = deltaTimeMS / 1000.0f;

    // CYCLE OF MOVE ENTITIES
    var entities = _netManager._entityManager.GetActiveEntities();
    foreach (var entity in entities)
    {
        if (entity is Hero _hero)
        {
            _hero.Update(deltaTime);
            
        }
    }
    _netManager.BroadcastPosition(entities);
    _netManager.Update();
    Thread.Sleep(15);
}




