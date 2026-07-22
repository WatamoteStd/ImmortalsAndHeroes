using Godot;
using Shared.Network.Packets;
using System;

public partial class NetworkPacketManager : Node
{

	public event Action<S2C_HandshakeResponse>? OnHandshakeResponse;
	public event Action<S2C_RegionEnter> OnServerEnterResponse;

	public static NetworkPacketManager Instance {get; set;}

	public override void _EnterTree()
	{
		
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		else
		{
			Instance = this;
		}

	}


	public override void _Process(double delta)
	{
		
		while(NetworkUdpClient.Instance.PacketQueue.TryDequeue( out INetworkPacket packet))
		{
			
			switch (packet)
			{
				
				case S2C_HandshakeResponse response:
					{
						
						if (response.Status == 1)
						{
							
							OnHandshakeResponse?.Invoke(response);

						}
						else GD.PrintErr("Can't connect to the UDP server. Please try again later.");

					}
				break;

				case S2C_RegionEnter response:
					{
						
						GameSession.Instance.NetworkId = response.MyNetworkId;
						OnServerEnterResponse?.Invoke(response);

					}
				break;

			}

		}

	}


}
