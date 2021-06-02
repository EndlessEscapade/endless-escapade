using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Flails
{
    public class KelpFlail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Flail");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.rare = ItemRarityID.LightRed;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 9f; // 5 and 1/4
            item.useTime = 40;
            item.useAnimation = 40;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.damage = 55;
            item.width = 44;
            item.height = 50;
            item.UseSound = SoundID.Item1;
        }
    }
}