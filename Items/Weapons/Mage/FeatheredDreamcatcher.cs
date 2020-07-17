using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;
using EEMod.Projectiles.CoralReefs;
using EEMod.Projectiles.OceanMap;

namespace EEMod.Items.Weapons.Mage
{
    public class FeatheredDreamcatcher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Dreamcatcher");
        }

        public override void SetDefaults()
        {
            item.magic = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 50;
            item.useTime = 25;
            item.useAnimation = 25;
            item.width = 20;
            item.height = 20;
            item.shootSpeed = 0;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item11;
            item.mana = 7;
            //item.shoot = ModContent.ProjectileType<FeatheredDreamcatcherProjectile>();
            item.shoot = ModContent.ProjectileType<Crate>();
        }
    }
}