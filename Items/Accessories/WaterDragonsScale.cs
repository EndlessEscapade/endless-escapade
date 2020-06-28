using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.Accessories
{

    public class WaterDragonsScale : ModItem
    {
        //commit check
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Water Dragon's Scale");
            Tooltip.SetDefault("The power of an ancient dragon stirs within you\nReminds you of Lake Floria");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Expert;
            item.width = 32;
            item.height = 34;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<EEPlayer>().dragonScale = true;
        }
    }
}

