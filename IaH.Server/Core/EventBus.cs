using System;
using System.Collections.Generic;
using System.Text;
using IaH.Shared.Networking.Events;

namespace IaH.Server.Core
{
    public static class EventBus
    {

        public static event Action<EntityHpChangedEvent> OnHpChanged;

        public static void PublishHp(EntityHpChangedEvent data)
        {
            OnHpChanged?.Invoke(data);
        }

    }
}
