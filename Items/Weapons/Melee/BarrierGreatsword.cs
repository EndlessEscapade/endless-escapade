using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Weapons.Melee
{
    public class BarrierGreatsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier Greatsword");
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
            item.width = 64;
            item.height = 64;
            item.UseSound = SoundID.Item1;
        }
    }
}