using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Armor.Quartz
{
    [AutoloadEquip(EquipType.Head)]
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
            item.rare = ItemRarityID.Pink;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<QuartzChestplate>() && legs.type == ModContent.ItemType<QuartzLeggings>();
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
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 7);
            recipe.AddIngredient(ItemID.AshBlock, 25);
            recipe.AddIngredient(ModContent.ItemType<QuartzicLifeFragment>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
