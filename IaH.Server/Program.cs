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

NetworkManager _netManager = new NetworkManager();
_netManager.Start();
HeroDataManager.Initialize();

// LOBBY
List<Lobby> allLobbies = new List<Lobby>();

LobbyManager lobbyManager = new LobbyManager(allLobbies, _netManager);
_netManager.SetLobbyManager(lobbyManager);


while (true)
{
    // DELTATIME
    var currenTime = sw.ElapsedMilliseconds;
    float deltaTimeMS = currenTime - lastTime;
    lastTime = currenTime;
    float deltaTime = deltaTimeMS / 1000.0f;

    _netManager.Update();

    //LOBBY
    foreach (var lobby in allLobbies)
    {
        lobby.Update(deltaTime);
    }    
    
    Thread.Sleep(15);
}




