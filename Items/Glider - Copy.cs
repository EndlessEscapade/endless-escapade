using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Items
{
    public class GliderCopy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glidercopy");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = 50;

            item.maxStack = 1;
            item.useTime = 120;
            item.useAnimation = 120;
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.holdStyle = 2;

            //item.flame = true; needs a flame texture.
            item.noWet = true;
        }

        public override bool UseItem(Player player)
        {
            player.velocity += new Vector2(16 * player.direction, -16);
            return true;
        }
        public override void HoldItem(Player player)
        {
            if (player.itemAnimation > 0)
            {
                if (Math.Abs(player.velocity.Y) >= 0.0001)
                    player.fullRotation += MathHelper.PiOver4;
                else
                    player.fullRotation = 0;
            }
        }
    }
}