using Server.Network;


NetworkManager networkManager = new NetworkManager(29555);
networkManager.NetStart();

Console.WriteLine("================= Server Started ==============");


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