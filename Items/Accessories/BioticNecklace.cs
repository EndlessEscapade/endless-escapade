using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class BioticNecklace : ModItem
    {
        //commit check
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Biotic Necklace");
            Tooltip.SetDefault("5% increased summon damage\nYou feel more alive than usual");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Blue;
            item.width = 32;
            item.height = 34;
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamageMult += 0.05f;
        }
    }
}