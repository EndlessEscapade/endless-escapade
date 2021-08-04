using EEMod.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace EEMod.Items.Placeables.Ores
{
    public class LythenOre : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Ore");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 10));
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 3);
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useAnimation = 20;
            Item.useTime = 15;
            Item.consumable = true;
            Item.material = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<LythenOreTile>();
        }
    }
}