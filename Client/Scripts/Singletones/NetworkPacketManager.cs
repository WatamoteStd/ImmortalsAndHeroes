using Godot;
using Shared.Network.Packets;
using System;

public partial class NetworkPacketManager : Node
{

	public override void _Process(double delta)
	{
		
		while(NetworkUdpClient.Instance.PacketQueue.TryDequeue( out INetworkPacket packet))
		{
			
			switch (packet)
			{
				
				case S2C_HandshakeResponse response:
					{
						
						GD.Print($"SUCCESFULL HANDSHAKE! Status:{response.Status}");

					}
				break;

			}

		}

	}


}
