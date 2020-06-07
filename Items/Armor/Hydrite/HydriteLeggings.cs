using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using InteritosMod.Items.Placeables;
using InteritosMod.Items.Placeables.Ores;

namespace InteritosMod.Items.Armor.Hydrite
{
	[AutoloadEquip(EquipType.Legs)]
	public class HydriteLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydrite Leggings");
			Tooltip.SetDefault("6% increased movement speed");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 30);
			item.rare = ItemRarityID.LightRed;
			item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.06f;
			player.maxRunSpeed += 0.06f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HydriteBar>(), 13);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}