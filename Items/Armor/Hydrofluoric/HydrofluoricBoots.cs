using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Armor.Hydrofluoric
{
    [AutoloadEquip(EquipType.Legs)]
    public class HydrofluoricBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Boots");
            Tooltip.SetDefault("10% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Lime;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.maxRunSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydroFluoricBar>(), 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}