using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables;

namespace EEMod.Items.Armor.Lythen
{
	[AutoloadEquip(EquipType.Head)]
	public class LythenHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lythen Hat");
			Tooltip.SetDefault("6% increased magic damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 21);
			item.rare = ItemRarityID.Green;
			item.defense = 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<LythenChestplate>() && legs.type == ModContent.ItemType<LythenBoots>();
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.06f;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "All stats increased while submerged on water";
			if (player.wet)
			{
				player.allDamage += 0.02f;
				player.moveSpeed += 0.07f;
				player.maxRunSpeed += 0.07f;
				player.magicCrit += 2;
				player.thrownCrit += 2;
				player.rangedCrit += 2;
				player.meleeCrit += 2;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<LythenBar>(), 11);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}