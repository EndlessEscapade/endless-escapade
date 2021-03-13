using EEMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class Telescope : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Telescope");
            Tooltip.SetDefault("Can be held\nVanity item\n'Set sail!'");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Orange;
            item.width = 40;
            item.height = 14;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.holdStyle = 7;
        }

        public override bool HoldItemFrame(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 2;
            return true;
        }

        public override void HoldStyle(Player player)
        {
            player.ChangeDir(Main.MouseWorld.X > player.Center.X ? 1 : -1);

            player.itemLocation = player.MountedCenter + new Vector2(2 * player.direction, -2);
        }
    }
}