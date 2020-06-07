using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Items.Armour
{
    [AutoloadEquip(new EquipType[]
	{
		EquipType.Head
	})]
	public class QuartzHellhelm : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Quartzic Hellhelm");
            Tooltip.SetDefault("5 defense\n7% increased prophet damage\n4% increased prophet critical strike chance\nProphet class feature not out yet. Have some vanity. =)");
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
            player.setBonus = "Vanity.";

        }

        public override void UpdateEquip(Player player)
		{
            player.statDefense += 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "QuartzGem", 7);
            recipe.AddIngredient(ItemID.AshBlock, 25);
            recipe.AddIngredient(null, "QuartzicLifeFragment", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
