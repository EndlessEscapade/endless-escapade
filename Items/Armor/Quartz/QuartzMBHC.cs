/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Armor.Quartz
{
    [AutoloadEquip(EquipType.Head)]
    public class QuartzMBHC : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Moonbrace Hair Clip");
            Tooltip.SetDefault("1 defense\n6% increased summon damage\n+1 max minions");
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
            player.setBonus = "7% Increased summon damage and a chance for minions to inflict On Fire on enemy hits";
            player.minionDamage *= 1.07f;
            player.GetModPlayer<EEPlayer>().isQuartzSummonOn = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.statDefense += 1;
            player.minionDamage *= 1.06f;
            player.maxMinions += 1;
        }
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = drawAltHair = true;  //this make so the player hair does not show when the vanity mask is equipped.  add true if you want to show the player hair.
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 7);
            recipe.AddIngredient(ItemID.PlatinumHelmet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 7);
            recipe.AddIngredient(ItemID.GoldHelmet, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
*/