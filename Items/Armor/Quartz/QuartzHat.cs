using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Items.Armour
{
    [AutoloadEquip(new EquipType[]
	{
		EquipType.Head
	})]
	public class QuartzHat : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Quartz Hat");
            Tooltip.SetDefault("3 defense\n6% increased magic damage\n6% reduced mana usage\n+20 Mana");
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
            player.setBonus = "6% increased magic critical strike chance\n10% reduced mana usage";
            player.magicCrit += 6;
            player.manaCost *= 0.9f;

        }

        public override void UpdateEquip(Player player)
		{
            player.statDefense += 3;
            player.magicDamage *= 1.06f;
            player.manaCost *= 0.94f;
            player.statManaMax2 += 20;
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
