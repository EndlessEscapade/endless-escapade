using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Prophecy.Materials
{
    public class MummifiedRag : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Mummidied Rag");
            Tooltip.SetDefault("dusty");
        }
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 24;
            item.maxStack = 999;
            item.value = 100;
            item.rare = 1;
        }
    }
}
