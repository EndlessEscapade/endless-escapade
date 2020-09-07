using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class HydrosEyeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // The following sets are only applicable to yoyo that use aiStyle 99.
            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player.
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 180f;
            // YoyosTopSpeed is top speed of the yoyo projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 12f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;
            // aiStyle 99 is used for all yoyos, and is Extremely suggested, as yoyo are extremely difficult without them
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
        }

        // notes for aiStyle 99:
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.
        private Vector2 closestNPCPos;

        private void Trail(Vector2 from, Vector2 to, float scale)
        {
            float distance = Vector2.Distance(from, to);
            float step = 1 / distance;
            for (float w = 0; w < distance; w += 4)
            {
                Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 16, Vector2.Zero, 0, default(Color), scale).noGravity = true;
            }
        }

        private Vector2 center;

        public override void PostAI()
        {
            alphaCounter += 0.04f;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Vector2.DistanceSquared(Main.LocalPlayer.Center, Main.npc[i].Center) <= Vector2.DistanceSquared(Main.LocalPlayer.Center, closestNPCPos) && Main.npc[i].active)
                {
                    closestNPCPos = Main.npc[i].Center;
                }
            }
            if (Vector2.DistanceSquared(projectile.Center, closestNPCPos) < 200 * 200)
            {
                projectile.ai[1] = 1 - Vector2.Distance(projectile.Center, closestNPCPos) / 200f;
                if (Main.myPlayer == projectile.owner)
                {
                    if (center != Vector2.Zero)
                    {
                        Trail(center, projectile.Center, 1 - Vector2.Distance(projectile.Center, closestNPCPos) / 200f);
                    }
                    if (alphaCounter % 3 <= 0.04f)
                    {
                        int pieCut = Main.rand.Next(6, 8);
                        for (int m = 0; m < pieCut; m++)
                        {
                            int projID = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<HydrosEyeSubProj>(), 15, 0, Main.myPlayer);
                            Main.projectile[projID].velocity = new Vector2(0.5f, 0f).RotatedBy(m / (float)pieCut * Math.PI * 2);
                            Main.projectile[projID].netUpdate = true;
                        }
                    }
                }
            }
            else
            {
                projectile.ai[1] = 0;
                alphaCounter = 0;
            }
            center = projectile.Center;
        }

        private float alphaCounter = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(TextureCache.Extra_49, projectile.Center - Main.screenPosition, null, new Color((int)(4 * sineAdd), (int)(2 * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), Math.Abs(0.33f * (sineAdd + 1)) * projectile.ai[1], SpriteEffects.None, 0f);
            return base.PreDraw(spriteBatch, lightColor);
        }
    }
}