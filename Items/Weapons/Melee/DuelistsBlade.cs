using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Weapons.Melee
{
    public class DuelistsBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist's Blade");
            Tooltip.SetDefault("It once belonged to an ancient warrior of times past");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.rare = ItemRarityID.LightRed;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 7f; // 5 and 1/4
            item.useTime = 15;
            item.useAnimation = 15;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.damage = 17;
            item.width = 20;
            item.height = 20;
            item.UseSound = SoundID.Item1;
        }
    }
}