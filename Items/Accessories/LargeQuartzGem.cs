using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Accessories
{
    public class LargeQuartzGem : ModItem
    {
        public bool butt;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Large Quartz Gem");
            Tooltip.SetDefault("Grants a regeneration potion effect when equipped");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.Pink;
            item.width = 32;
            item.height = 34;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            butt = true;
        }

        // empty method

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // hideVisual = true; // not a ref param
            player.AddBuff(BuffID.Regeneration, -1);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 15);
            recipe.AddIngredient(ModContent.ItemType<QuartzicLifeFragment>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
