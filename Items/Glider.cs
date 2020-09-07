using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class Glider : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glider");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = 50;

            item.maxStack = 1;

            item.holdStyle = 2;

            //item.flame = true; needs a flame texture.
            item.noWet = true;
        }

        public override void HoldStyle(Player player)
        {
            if (player.velocity.Y > 0)
            {
                player.gravity = 0.133f;
                player.bodyFrame.Y = 4 * 56;

                player.velocity.Y = 1;
                if (Math.Abs(player.velocity.X) < 18)
                    if ((player.controlRight && player.velocity.X > 0) || (player.controlLeft && player.velocity.X < 0))
                        player.velocity.X *= 1.02f;
            }
        }
    }
}