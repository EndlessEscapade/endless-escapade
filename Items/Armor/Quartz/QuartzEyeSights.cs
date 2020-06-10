using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Armor.Quartz
{
    [AutoloadEquip(EquipType.Head)]
	public class QuartzEyeSights : ModItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Quartz Eyesights");
            Tooltip.SetDefault("2 defense\n6% increased throwing damage\n5% increased throwing critical strike chance");
		}

		public override void SetDefaults()
		{
            item.width = 20;
            item.height = 8;
            item.value = Item.buyPrice(0, 12, 35, 0);
            item.rare = ItemRarityID.Pink;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = drawAltHair = true;  //this make so the player hair does not show when the vanity mask is equipped.  add true if you want to show the player hair.
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<QuartzChestplate>() && legs.type == ModContent.ItemType<QuartzLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Is to be announced lol";

        }

        public override void UpdateEquip(Player player)
		{
            player.statDefense += 2;
            player.thrownDamage *= 1.06f;
            player.thrownCrit += 5;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 7);
            recipe.AddIngredient(ItemID.GoldHelmet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 7);
            recipe.AddIngredient(ItemID.PlatinumHelmet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
