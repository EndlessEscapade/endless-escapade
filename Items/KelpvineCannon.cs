using EEMod.Buffs.Buffs;
using EEMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class KelpvineCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Kelpvine Cannon");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.sellPrice(silver: 20);
        }
    }
}