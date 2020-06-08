using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Ranger
{
    public class SandBuster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sand Buster");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 80);
            item.damage = 57;
            item.useTime = 7;
            item.useAnimation = 7;
            item.width = 20;
            item.height = 20;
            item.shoot = 10;
            item.rare = ItemRarityID.Lime;
            item.knockBack = 1.25f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 13f;
            item.UseSound = SoundID.Item11;
            item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
    }
}