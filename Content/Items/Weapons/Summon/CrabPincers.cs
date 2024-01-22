using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Weapons.Summon;

public class CrabPincers : ModItem
{
    public override void SetDefaults() {
        Item.noUseGraphic = true;
        Item.sentry = true;
        Item.noMelee = true;
        
        Item.DamageType = DamageClass.Summon;
        Item.damage = 20;
        Item.knockBack = 2f;
        Item.mana = 20;
        
        Item.width = 42;
        Item.height = 40;

        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.shoot = ModContent.ProjectileType<Projectiles.Summon.CrabPincers>();

        Item.UseSound = SoundID.Item1;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        Projectile.NewProjectile(source, Main.MouseWorld, velocity, type, damage, knockback, player.whoAmI);
        
        player.UpdateMaxTurrets();

        return false;
    }
}
