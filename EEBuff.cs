using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public abstract class EEBuff : ModBuff
    {
        public virtual void SetDefaults()
        {
            SetStaticDefaults();
        }
    }
}
