using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Items
{
    public class HockeyStick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hockey Stick");
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
            Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<HockeyStick>(), 0, 0);
        }
    }
}