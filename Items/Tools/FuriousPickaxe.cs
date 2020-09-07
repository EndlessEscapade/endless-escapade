using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools
{
    public class FuriousPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Furious Pickaxe");
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.melee = true;
            item.width = 64;
            item.height = 64;
            item.useAnimation = 10;
            item.useTime = 2;
            item.pick = 10000;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 1, 8, 0);
            item.rare = ItemRarityID.Expert;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
        }
    }
}