using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Prophecy;
namespace Prophecy.Accessories
{

    public class LargeQuartzGem : ModItem
    {
        public bool butt;



        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            base.OnHitNPC(player, target, damage, knockBack, crit);
            butt = true;

        }

        public override void UpdateEquip(Player player)
        {
            base.UpdateEquip(player);
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Large Quartz Gem");
            Tooltip.SetDefault("Grants a regeneration potion effect when equipped");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.accessory = true;
            item.rare = 5;
            item.width = 32;
            item.height = 34;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            hideVisual = true;

            player.AddBuff(BuffID.Regeneration, -1);
        }
        public override void AddRecipes()
        {
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(mod.ItemType("QuartzGem"), 15);
                recipe.AddIngredient(mod.ItemType("QuartzicLifeFragment"), 1);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
