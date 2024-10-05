using EndlessEscapade.Content.Projectiles.Beach;
using Terraria.DataStructures;

namespace EndlessEscapade.Content.Items.Beach;

public class CrabPincersItem : ModItem
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

        Item.shoot = ModContent.ProjectileType<CrabPincersProjectile>();

        Item.UseSound = SoundID.Item1;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
        var tileCoordinates = Main.MouseWorld.ToTileCoordinates();

        position = Main.MouseWorld;

        for (var i = tileCoordinates.Y; i < Main.maxTilesY; i++) {
            var tile = Framing.GetTileSafely(tileCoordinates.X, i);

            if (WorldGen.SolidTile(tile)) {
                position = new Vector2(tileCoordinates.X, i - 1) * 16f;
                break;
            }
        }

        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

        player.UpdateMaxTurrets();

        return false;
    }
}
