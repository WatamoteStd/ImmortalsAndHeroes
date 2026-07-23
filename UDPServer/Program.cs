using UDPServer;
using UDPServer.Network;
using UDPServer.World;

WorldHolder world = new WorldHolder();
NetworkUdpManager NetManager = new NetworkUdpManager(world);
world.Initialize(NetManager);

UdpGate Server = new UdpGate(NetManager.OnPacketReceived);

ServerLoop serverLoop = new ServerLoop(world);

Server.Start(29555);
serverLoop.Start();

Console.WriteLine("======================== SERVER STARTED ====================");


Console.ReadLine();