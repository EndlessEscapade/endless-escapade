using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories
{
    public class HallowedDivingGear : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hallowed Diving Gear");
            Tooltip.SetDefault("Grants freedom underwater");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 0, 32);
            Item.scale = 0.2f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<EEPlayer>().hydroGear = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 8).AddIngredient(ItemID.ArcticDivingGear, 1).AddTile(TileID.Anvils).Register();
        }
    }
}