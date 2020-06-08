using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.ModLoader;

namespace EEMod.Items.Accessories.Runes
{
    public class LeafRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leaf Rune");
            Tooltip.SetDefault("3% increased damage while on forest" + "\n9% increased movement speed while on forest" + "\nDecreased 6% damage while out of the forest");
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
            if (!player.ZoneJungle
                && !player.ZoneDungeon
                && !player.ZoneCorrupt
                && !player.ZoneCrimson
                && !player.ZoneHoly
                && !player.ZoneSnow
                && !player.ZoneUndergroundDesert
                && !player.ZoneGlowshroom
                && !player.ZoneMeteor
                && !player.ZoneBeach
                && player.ZoneOverworldHeight)
            {
                player.allDamage += 0.03f;
                player.moveSpeed += 0.09f;
                player.maxRunSpeed += 0.09f;
            }
            else
            {
                player.allDamage -= 0.06f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 12);
            recipe.AddIngredient(ItemID.StoneBlock, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}