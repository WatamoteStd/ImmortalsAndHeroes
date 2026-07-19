using UDPServer.Network;

NetworkUdpManager NetManager = new NetworkUdpManager();
UdpGate Server = new UdpGate(NetManager.OnPacketReceived);

Server.Start(29555);

Console.WriteLine("======================== SERVER STARTED ====================");

Console.ReadLine();