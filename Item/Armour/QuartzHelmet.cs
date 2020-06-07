using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Items.Armour
{
    [AutoloadEquip(new EquipType[]
	{
		EquipType.Head
	})]
	public class QuartzHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Quartz Helmet");
            Tooltip.SetDefault("5 defense\n7% increased melee damage\n3% increased melee critical strike chance");
		}

		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 24;
            item.value = Item.buyPrice(0, 12, 35, 0);
            item.rare = 5;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("QuartzChestplate") && legs.type == mod.ItemType("QuartzLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "10% Increased damage for Melee Weapons\nChance to shoot out crystal projectiles on swing";
            player.meleeDamage *= 1.1f;
            player.GetModPlayer<ProphecyPlayer>().isQuartzMeleeOn = true;
        }

        public override void UpdateEquip(Player player)
		{
            player.statDefense += 5;
            player.meleeDamage *= 1.07f;
            player.meleeCrit += 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "QuartzGem", 7);
            recipe.AddIngredient(ItemID.PlatinumHelmet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "QuartzGem", 7);
            recipe.AddIngredient(ItemID.GoldHelmet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
