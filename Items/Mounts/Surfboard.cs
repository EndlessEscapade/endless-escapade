using EEMod.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Mounts
{
    public class Surfboard : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Surfboard");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.value = 30000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item79;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<SurfboardMount>();
        }
    }
}