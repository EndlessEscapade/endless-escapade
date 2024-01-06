using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Projectiles.StarfishApprentice;

public class SpinnerFish : ModProjectile
{
    private ref float TargetIndex => ref Projectile.ai[0];

    private bool stickingToNPC;
    private bool stickingToTile;

    private Vector2 offset;

    public override void SetDefaults() {
        Projectile.usesLocalNPCImmunity = true;
        Projectile.friendly = true;

        Projectile.width = 16;
        Projectile.height = 16;

        Projectile.aiStyle = -1;
        AIType = -1;

        Projectile.timeLeft = 180;
        Projectile.penetrate = -1;
        Projectile.localNPCHitCooldown = 30;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        if (!stickingToNPC && !stickingToTile) {
            TargetIndex = target.whoAmI;

            offset = target.Center - Projectile.Center + Projectile.velocity;

            stickingToNPC = true;

            Projectile.netUpdate = true;
        }
        
        Projectile.scale = 1.25f;
        Projectile.rotation += MathHelper.ToRadians(Main.rand.NextFloat(5f, 15f));
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        if (!stickingToTile && !stickingToNPC) {
            stickingToTile = true;
            
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        }

        return false;
    }

    public override void AI() {
        var target = Main.npc[(int)TargetIndex];
        
        if (Projectile.timeLeft < 255 / 25) {
            Projectile.alpha += 25;
        }
        
        if (stickingToNPC) {
            if (target.active && !target.dontTakeDamage) {
                Projectile.tileCollide = false;

                Projectile.Center = target.Center - offset;
                Projectile.gfxOffY = target.gfxOffY;
            }
            else {
                Projectile.Kill();
            }
        }
        else if (!stickingToTile) {
            Projectile.rotation += Projectile.velocity.X * 0.1f;

            Projectile.ai[0]++;

            if (Projectile.ai[0] > 10f) {
                Projectile.velocity.Y += 0.2f;
            }
        }
        else {
            Projectile.velocity *= 0.5f;
        }

        Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.2f);
    }
}
