using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Armor.Dalantinium
{
    [AutoloadEquip(EquipType.Head)]
    public class DalantiniumGreathelm : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Greathelm");
            Tooltip.SetDefault("5% increased melee speed");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DalantiniumPlatemail>() && legs.type == ModContent.ItemType<DalantiniumGreaves>();
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += 0.05f;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "When below 40% life, you gain 8 defense";
            if (player.statLife <= player.statLifeMax * 0.40f)
            {
                player.statDefense += 8;
            }
            player.GetModPlayer<EEPlayer>().dalantiniumSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DalantiniumBar>(), 11).AddTile(TileID.Anvils).Register();
        }
    }
}