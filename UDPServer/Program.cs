using UDPServer.Network;
using UDPServer.World;

WorldHolder world = new WorldHolder();
NetworkUdpManager NetManager = new NetworkUdpManager(world);
UdpGate Server = new UdpGate(NetManager.OnPacketReceived);

Server.Start(29555);

Console.WriteLine("======================== SERVER STARTED ====================");

Console.ReadLine();