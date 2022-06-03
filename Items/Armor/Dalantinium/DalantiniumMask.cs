using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Dalantinium
{
    [AutoloadEquip(EquipType.Head)]
    public class DalantiniumMask : EEItem
    {
        private bool flag;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Mask");
            Tooltip.SetDefault("6% reduced ammo consumption");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DalantiniumPlatemail>() && legs.type == ModContent.ItemType<DalantiniumGreaves>();
        }

        public override void UpdateEquip(Player player)
        {
            flag = true;
        }

        /*public override bool CanConsumeAmmo(Player player)
        {
            if (flag)
            {
                return Main.rand.NextFloat() < .06f;
            }
            else
            {
                return true;
            }
        }*/

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When below 40% life, you gain 8 defense";
            if (player.statLife <= player.statLifeMax * 0.40f)
            {
                player.statDefense += 8;
            }
            player.GetModPlayer<DalantiniumSetPlayer>().dalantiniumSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DalantiniumBar>(), 11).AddTile(TileID.Anvils).Register();
        }
    }
}