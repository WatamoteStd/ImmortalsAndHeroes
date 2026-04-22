using IaH.Shared.Networking;
using IaH.Server.Core;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Threading;

Console.WriteLine("---IaH Server Starting---");

NetworkManager _netManager = new NetworkManager();
_netManager.Start();

EntityManager _entitymanager = new EntityManager();


while (true)
{
    _netManager.Update();
    Thread.Sleep(15);
}




