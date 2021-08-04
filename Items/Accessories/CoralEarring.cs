using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class CoralEarring : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Earring");
            Tooltip.SetDefault("Forged by master merfolk fisherman");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.width = 32;
            Item.height = 34;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.fishingSkill += 30;
        }
    }
}