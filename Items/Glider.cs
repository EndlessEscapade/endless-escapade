using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

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
            if (player.velocity.Y != 0)
            {
                player.gravity = 0.133f;
                player.bodyFrame.Y = 4 * 56;
                //player.bodyFrameCounter = 5;
                /*player.headFrame.Y = 5 * 56;
                player.headFrameCounter = 5;
                player.legFrame.Y = 5 * 56;
                player.legFrameCounter = 5;*/

                player.velocity.Y = 1;
                if (Math.Abs(player.velocity.X) < 3)
                    player.velocity.X *= 1.05f;
            }
        }
    }
}