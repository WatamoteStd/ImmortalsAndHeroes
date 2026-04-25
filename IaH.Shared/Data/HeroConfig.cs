using System;
using System.Collections.Generic;
using System.Text;

namespace IaH.Shared.Data
{
    public class HeroConfig
    {

        public ushort HeroId { get; set; }
        public string Name { get; set; }
        public HeroStats Stats { get; set; }
        public HeroCombat Combat { get; set; }

    }
}
