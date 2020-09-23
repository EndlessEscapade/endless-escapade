using EEMod.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Mounts
{
    public class Surfboard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surfboard");
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
            item.mountType = ModContent.MountType<SurfboardMount>();
        }
    }
}