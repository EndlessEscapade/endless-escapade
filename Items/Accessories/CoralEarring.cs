using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Accessories
{
    public class CoralEarring : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Earring");
            Tooltip.SetDefault("Forged by master merfolk fisherman");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Blue;
            item.width = 32;
            item.height = 34;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.fishingSkill += 30;
        }
    }
}