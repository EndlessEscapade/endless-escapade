using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles;
using Microsoft.Xna.Framework;
using EEMod.Mounts;

namespace EEMod.Items
{
    public class LobsterbikeKeys : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lobsterbike Keys");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = 30000;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item79;
            item.noMelee = true;
            item.mountType = ModContent.MountType<Lobsterbike>();
        }
    }
}