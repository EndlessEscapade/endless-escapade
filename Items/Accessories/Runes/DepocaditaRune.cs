using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace InteritosMod.Items.Accessories.Runes
{
    public class DepocaditaRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Depocadita Rune");
            Tooltip.SetDefault("3% increased critical strike" + "\nchance while on the Mushroom Biome" + "\n5% increased damage while" + "\non the Mushroom Biome" + "7% velocity decreased while out of" + "\nthe Mushroom Biome");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(0, 0, 32);
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            return rand.Next(new int[] { PrefixID.Arcane, PrefixID.Lucky, PrefixID.Menacing, PrefixID.Quick, PrefixID.Violent, PrefixID.Warding });
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
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GlowingMushroom, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}