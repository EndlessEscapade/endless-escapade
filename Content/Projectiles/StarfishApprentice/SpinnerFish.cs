using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Projectiles.StarfishApprentice;

public class SpinnerFish : ModProjectile
{
    private ref float Target => ref Projectile.ai[0];
    private ref float Timer => ref Projectile.ai[1];
    
    private Vector2 offset;

    public bool StickingToNPC { get; private set; }
    public bool StickingToTile { get; private set; }

    public bool StickingToAnything => StickingToNPC || StickingToTile;

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

    public override void SendExtraAI(BinaryWriter writer) {
        writer.Write(StickingToNPC);
        writer.Write(StickingToTile);
    }

    public override void ReceiveExtraAI(BinaryReader reader) {
        StickingToNPC = reader.ReadBoolean();
        StickingToTile = reader.ReadBoolean();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        Projectile.scale = 1.25f;
        Projectile.rotation += MathHelper.ToRadians(Main.rand.NextFloat(5f, 15f));
        
        if (StickingToAnything) {
            return;
        }
        
        offset = target.Center - Projectile.Center + Projectile.velocity;

        Target = target.whoAmI;
        StickingToNPC = true;

        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        if (!StickingToAnything) {
            return false;
        }
        
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

        StickingToTile = true;
            
        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);

        return false;
    }

    public override void AI() {
        Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.2f);

        if (Projectile.timeLeft < 255 / 25) {
            Projectile.alpha += 25;
        }
        
        UpdateTargetStick();
        UpdateTileStick();

        if (StickingToAnything) {
            return;
        }
        
        Projectile.rotation += Projectile.velocity.X * 0.1f;
        
        UpdateGravity();
    }

    private void UpdateTargetStick() {
        if (!StickingToNPC) {
            return;
        }
        
        var target = Main.npc[(int)Target];

        if (!target.active) {
            Projectile.Kill();
            return;
        }

        Projectile.tileCollide = false;

        Projectile.Center = target.Center - offset;
        Projectile.gfxOffY = target.gfxOffY;
    }

    private void UpdateTileStick() {
        if (!StickingToTile) {
            return;
        }
        
        Projectile.velocity *= 0.5f;
    }

    private void UpdateGravity() {
        Timer++;

        if (Timer < 10f) {
            return;
        }
        
        Projectile.velocity.Y += 0.2f;
    }
}
