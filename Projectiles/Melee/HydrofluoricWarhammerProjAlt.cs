using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles.Melee
{
    public class HydrofluoricWarhammerProjAlt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Warhammer");
        }

        public override void SetDefaults()
        {
            projectile.width = 40;       //projectile width
            projectile.height = 40;  //projectile height
            projectile.friendly = true;      //make that the projectile will not damage you
            projectile.magic = true;     //
            projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = 1;      //how many npc will penetrate
                                           //how many time this projectile has before disepire
            projectile.light = 0.3f;    // projectile light
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            projectile.ai[0]++;
            if (projectile.ai[0] >= 240)
            {
                projectile.velocity = Vector2.Normalize(projectile.Center - Main.player[projectile.owner].Center) * -16;
                projectile.rotation += 0.4f;
                projectile.tileCollide = false;
                if (Vector2.Distance(projectile.Center, Main.player[projectile.owner].Center) <= 8)
                    projectile.Kill();
            }
            float legoYoda = projectile.velocity.X > 0 ? MathHelper.PiOver4 : MathHelper.Pi - MathHelper.PiOver4;
            if (projectile.ai[0] >= 20 && projectile.ai[0] < 240)
            {
                projectile.velocity.Y += 1.2f;
                projectile.rotation = projectile.velocity.ToRotation() + legoYoda;
            }
            if (projectile.ai[0] < 20)
            {
                projectile.rotation = projectile.velocity.ToRotation() + legoYoda;
            }
            if (projectile.ai[1] > 0) projectile.ai[1]--;
            if (projectile.ai[1] == 0)
            {
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.ai[0] = 240;
            Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(projectile.Center, 8f, true, false, 8);
            projectile.ai[1] = 5;
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Helpers.DrawChain(mod.GetTexture("Projectiles/Melee/HydrofluoricWarhammerChain"), Main.player[projectile.owner].Center, projectile.Center, 0);
            if (projectile.ai[0] >= 120)
                AfterImage.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, 1.5f, 1f, 1, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 100));
            return true;
        }
    }
}
