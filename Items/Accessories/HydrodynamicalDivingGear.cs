using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;
using InteritosMod.Items.Placeables.Ores;

namespace InteritosMod.Items.Accessories
{
	public class HydrodynamicalDivingGear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hydrodynamical Diving Gear");
			Tooltip.SetDefault("3% increased critical strike chance while on the Mushroom Biome" + "\n5% increased damage while on the Mushroom Biome" + "7% velocity decreased while outside of the Mushroom Biome");
		}

		public override void SetDefaults()
		{
			item.accessory = true;
			item.width = 20;
			item.height = 20;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 32);
			item.scale = 0.2f;
		}

		public override void UpdateEquip(Player player)
		{
			if (player.ZoneGlowshroom)
			{
				player.allDamage += 0.05f;
				player.magicCrit += 3;
				player.rangedCrit += 3;
				player.thrownCrit += 3;
				player.meleeCrit += 3;
			}
			else
			{
				player.maxRunSpeed -= 0.07f;
				player.moveSpeed -= 0.07f;
			}

          //  player.GetModPlayer<InteritosPlayer>().hydroDivingGear = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ArcticDivingGear, 1);
            recipe.AddIngredient(ModContent.ItemType<HydriteBar>(), 7);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
