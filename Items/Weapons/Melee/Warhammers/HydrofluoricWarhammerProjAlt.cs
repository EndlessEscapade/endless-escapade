using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Items.Weapons.Melee.Warhammers
{
    public class HydrofluoricWarhammerProjAlt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Warhammer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;       //projectile width
            Projectile.height = 40;  //projectile height
            Projectile.friendly = true;      //make that the projectile will not damage you
            Projectile.magic = true;     //
            Projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            Projectile.penetrate = 1;      //how many npc will penetrate
                                           //how many time this projectile has before disepire
            Projectile.light = 0.3f;    // projectile light
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 240)
            {
                Projectile.velocity = Vector2.Normalize(Projectile.Center - Main.player[Projectile.owner].Center) * -16;
                Projectile.rotation += 0.4f;
                Projectile.tileCollide = false;
                if (Vector2.Distance(Projectile.Center, Main.player[Projectile.owner].Center) <= 8)
                    Projectile.Kill();
            }
            float legoYoda = Projectile.velocity.X > 0 ? MathHelper.PiOver4 : MathHelper.Pi - MathHelper.PiOver4;
            if (Projectile.ai[0] >= 20 && Projectile.ai[0] < 240)
            {
                Projectile.velocity.Y += 1.2f;
                Projectile.rotation = Projectile.velocity.ToRotation() + legoYoda;
            }
            if (Projectile.ai[0] < 20)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + legoYoda;
            }
            if (Projectile.ai[1] > 0) Projectile.ai[1]--;
            if (Projectile.ai[1] == 0)
            {
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = 240;
            Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 8f, true, false, 8);
            Projectile.ai[1] = 5;
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Helpers.DrawChain(mod.GetTexture("Items/Weapons/Melee/Warhammers/HydrofluoricWarhammerChain"), Main.player[Projectile.owner].Center, Projectile.Center, 0);
            if (Projectile.ai[0] >= 120)
                AfterImage.DrawAfterimage(spriteBatch, Main.projectileTexture[Projectile.type], 0, Projectile, 1.5f, 1f, 1, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 100));
            return true;
        }
    }
}
