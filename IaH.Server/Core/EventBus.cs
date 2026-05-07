using System;
using System.Collections.Generic;
using System.Text;
using IaH.Server.Entities;
using IaH.Shared.Networking.Events;
using LiteNetLib;

namespace IaH.Server.Core
{
    public static class EventBus
    {
        public static event Action<Player> OnPlayerJoinedQueue;
        public static void PublishPlayerJoinedQueue(Player player)
        {
            OnPlayerJoinedQueue?.Invoke(player);
        }

        // GAMEPLAY
        public static event Action<EntityHpChangedEvent> OnHpChanged;

        public static void PublishHp(EntityHpChangedEvent data)
        {
            OnHpChanged?.Invoke(data);
        }

    }
}
