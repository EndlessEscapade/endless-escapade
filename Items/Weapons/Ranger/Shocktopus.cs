using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger
{
    public class Shocktopus : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shocktopus");
            Tooltip.SetDefault("Keeps away the buccaneers");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 12;
            item.useTime = 21;
            item.useAnimation = 21;
            item.width = 20;
            item.height = 20;
            item.shoot = 10;
            item.rare = ItemRarityID.Cyan;
            item.knockBack = 4f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 17f;
            item.UseSound = SoundID.Item11;
            item.useAmmo = AmmoID.Bullet;
        }
    }
}