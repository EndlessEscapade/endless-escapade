using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools
{
    public class FuriousPickaxe : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Furious Pickaxe");
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.melee = true;
            Item.width = 64;
            Item.height = 64;
            Item.useAnimation = 10;
            Item.useTime = 2;
            Item.pick = 10000;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.knockBack = 1;
            Item.value = Terraria.Item.sellPrice(0, 1, 8, 0);
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
        }
    }
}