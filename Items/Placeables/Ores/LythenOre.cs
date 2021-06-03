using EEMod.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace EEMod.Items.Placeables.Ores
{
    public class LythenOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Ore");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 10));
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 0, 3);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 20;
            item.useTime = 15;
            item.consumable = true;
            item.material = true;
            item.autoReuse = true;
            item.maxStack = 999;
            item.createTile = ModContent.TileType<LythenOreTile>();
        }
    }
}