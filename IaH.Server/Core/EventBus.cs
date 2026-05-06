using System;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Networking.Events;
using LiteNetLib;

namespace IaH.Server.Core
{
    public static class EventBus
    {
        public static event Action<NetPeer> OnPlayerJoinedQueue;
        public static void PublishPlayerJoinedQueue(NetPeer peer)
        {
            OnPlayerJoinedQueue?.Invoke(peer);
        }

        // GAMEPLAY
        public static event Action<EntityHpChangedEvent> OnHpChanged;

        public static void PublishHp(EntityHpChangedEvent data)
        {
            OnHpChanged?.Invoke(data);
        }

    }
}
