using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public abstract class EEItem : ModItem
    {
        public Item Item => base.item; // for 1.4 port
    }
}
