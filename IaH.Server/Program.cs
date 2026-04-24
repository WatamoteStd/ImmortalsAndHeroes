using IaH.Shared.Networking;
using IaH.Server.Core;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Threading;
using System.Diagnostics;

long lastTime = 0;
Stopwatch sw = new Stopwatch();
sw.Start();

Console.WriteLine("---IaH Server Starting---");

NetworkManager _netManager = new NetworkManager();
_netManager.Start();

EntityManager _entitymanager = new EntityManager();


while (true)
{
    var currenTime = sw.ElapsedMilliseconds;
    float deltaTimeMS = currenTime - lastTime;
    lastTime = currenTime;
    float deltaTime = deltaTimeMS / 1000.0f;

    _netManager.Update();
    Thread.Sleep(15);
}




