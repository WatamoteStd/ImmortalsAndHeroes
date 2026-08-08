using Server.Network;
using Server.World;


NetworkManager networkManager = new NetworkManager(29555);
networkManager.NetStart();

Console.WriteLine("================= Server Started ==============");

Loop loop = new Loop(60);
loop.Start();

long lastCleanupTime = Environment.TickCount64;

while(true)
{
    
    long now = Environment.TickCount64;

    if (now - lastCleanupTime >= 2500)
    {
        networkManager.sessionManager.Cleaner(15000, 30000);
        lastCleanupTime = now;
    }
    Thread.Sleep(15);

}