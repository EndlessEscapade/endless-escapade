using EndlessEscapade.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Pearl;

public class PearlCannon : ModItem
{
    private int shoot;

    public override void SetStaticDefaults() { }

    public override void SetDefaults() {
        Item.DamageType = DamageClass.Ranged;

        Item.useStyle = ItemUseStyleID.Shoot;

        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.autoReuse = true;

        Item.useTime = Item.useAnimation = 75;

        Item.width = 47;
        Item.height = 27;

        Item.damage = 10;
        Item.knockBack = 4;

        Item.shoot = ProjectileID.WoodenArrowFriendly;
        Item.shootSpeed = 10;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        shoot++;
        Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().ScreenShake = 2.8f;

        player.velocity += velocity * -0.25f;

        SoundEngine.PlaySound(SoundID.Item61);

        Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<PearlCannonProjectile>(), 0, 0, player.whoAmI);

        var muzzleOffset = Vector2.Normalize(velocity) * 50f;

        if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
            position += muzzleOffset;
        }

        if (shoot <= 9) {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 2f, ModContent.ProjectileType<PearlProjectile>(), damage, knockback, player.whoAmI);
        }
        else if (shoot == 10) {
            Projectile.NewProjectile(
                source,
                position,
                velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * 2f,
                ModContent.ProjectileType<BigPearlProjectile>(),
                damage *= 4,
                knockback *= 4,
                player.whoAmI
            );
            Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().ScreenShake = 5f;
            player.velocity += velocity * -1.12f;
            shoot = 0;
        }

        return false;
    }
}