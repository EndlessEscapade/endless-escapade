using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InteritosMod.Items.Placeables;
using InteritosMod.Items.Placeables.Ores;

namespace InteritosMod.Items.Armor.Hydrite
{
	[AutoloadEquip(EquipType.Body)]
	public class HydriteChestplate : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Hydrite Chestplate");
			Tooltip.SetDefault("6% increased damage");
		}

		public override void SetDefaults() {
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 30);
			item.rare = ItemRarityID.LightRed;
			item.defense = 12;
		}

		public override void UpdateEquip(Player player) 
		{
			player.allDamage += 0.06f;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HydriteBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}