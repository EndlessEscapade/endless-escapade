using EEMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger.Guns
{
    public class CoconutCannon : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut Cannon");
        }

        public override void SetDefaults()
        {
            Item.melee = false;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.ranged = true;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 35;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.width = 20;
            Item.height = 20;
            Item.shoot = ModContent.ProjectileType<CoconutProjectile>();
            Item.rare = ItemRarityID.Orange;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.HoldingOut;
            Item.shootSpeed = 17f;
            Item.UseSound = SoundID.Item11;
            Item.ammo = ModContent.ItemType<Coconut>();
            Item.notAmmo = true;
        }
    }
}