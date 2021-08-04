using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class WaterDragonsScale : EEItem
    {
        //commit check
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Dragon's Scale");
            Tooltip.SetDefault("The power of an ancient dragon stirs within you\nReminds you of Lake Floria");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.width = 32;
            Item.height = 34;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EEPlayer>().dragonScale = true;
        }
    }
}