using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Weapons.Melee
{
    public class Rapture : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapture");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.LightRed;
            item.melee = true;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.width = 20;
            item.height = 20;
            item.useTime = 16;
            item.useAnimation = 16;
            item.knockBack = 3f;
            item.damage = 68;
        }
    }
}