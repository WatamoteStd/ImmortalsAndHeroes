using Server.Network;
using Server.Network.Interfaces;
using Server.World;
using Server.World.Zone;


NetworkManager networkManager = new NetworkManager(29555);
networkManager.NetStart();

IWorldBroadcaster broadcaster = networkManager.sessionManager;
WorldHolder worldHolder = new WorldHolder(broadcaster);

networkManager.sessionManager.InitializeWorld(worldHolder);
networkManager.packetReader.InitializeWorld(worldHolder);

Console.WriteLine("================= Server Started ==============");

Loop loop = new Loop(60, worldHolder);
loop.Start();

long lastCleanupTime = Environment.TickCount64;

while(true)
{
    
    long now = Environment.TickCount64;

    if (now - lastCleanupTime >= 2500)
    {
        networkManager.sessionManager.Cleaner(20000, 25000);
        lastCleanupTime = now;
    }
    Thread.Sleep(15);

}