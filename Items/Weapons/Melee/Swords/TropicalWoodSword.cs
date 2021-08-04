using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class TropicalWoodSword : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tropical Wood Sword");
        }

        public override void SetDefaults()
        {
            Item.melee = true;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.knockBack = 9f; // 5 and 1/4
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.damage = 55;
            Item.width = 64;
            Item.height = 64;
            Item.UseSound = SoundID.Item1;
        }
    }
}