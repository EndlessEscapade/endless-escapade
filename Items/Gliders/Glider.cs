using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Gliders
{
    public class Glider : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glider");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = 50;

            Item.maxStack = 1;

            Item.holdStyle = 1;

            //item.flame = true; needs a flame texture.
            Item.noWet = true;
        }
        int lerpage;
        public override void UpdateInventory(Player player)
        {
            // player.GetModPlayer<EEPlayer>().isHoldingGlider = false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            EEMod.MainParticles.SetSpawningModules(new SpawnPeriodically(2));
            EEMod.MainParticles.SpawnParticles(position, default);
        }

        public override void HoldStyle(Player player)
        {
            player.GetModPlayer<EEPlayer>().isHoldingGlider = true;
            player.itemLocation += new Vector2(-100 * player.direction, 0);

            if (Main.rand.Next(4) == 0)
                Dust.NewDust(player.position + new Vector2(-30 * player.direction, -5), 2, 2, DustID.GemDiamond, 0, 0, 0, default, Math.Abs(player.velocity.X) / 40f);

            Tile tile = Framing.GetTileSafely((int)player.position.X / 16, (int)player.position.Y / 16 + 3);
            if (tile.HasTile
                && Main.tileSolid[tile.TileType]
                && Math.Abs(player.fullRotation) > 0.01f)
            {
                player.fullRotation -= player.fullRotation / 16f;
                lerpage = 0;
            }
            else
            {
                if (player.velocity.Y > 0)
                {
                    lerpage += 1;
                    float rotFactor = Math.Max(0, player.velocity.Y / 150f);
                    player.fullRotation += (float)Math.Sin(lerpage / 20f - .1f) * rotFactor * Helpers.Clamp(lerpage / 200f, 0, 1);
                }
            }
            if (player.velocity.Y > 0)
            {

                player.gravity = 0.133f;
                player.bodyFrame.Y = 4 * 56;

                player.velocity.Y = 1;
                if (Math.Abs(player.velocity.X) < 8)
                {
                    if ((player.controlRight && player.velocity.X > 0) || (player.controlLeft && player.velocity.X < 0))
                    {
                        player.velocity.X *= 1.02f;
                    }
                }
            }
        }
    }
}
