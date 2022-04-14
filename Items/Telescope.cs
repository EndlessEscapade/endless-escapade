using EEMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class Telescope : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Telescope");
            Tooltip.SetDefault("Can be held\nVanity item\n'Set sail!'");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.width = 40;
            Item.height = 14;
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.holdStyle = 7;
        }

        public override void HoldItemFrame(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 2;
        }

        public override void HoldStyle(Player player)
        {
            player.ChangeDir(Main.MouseWorld.X > player.Center.X ? 1 : -1);

            player.itemLocation = player.MountedCenter + new Vector2(2 * player.direction, -2);
        }
    }
}