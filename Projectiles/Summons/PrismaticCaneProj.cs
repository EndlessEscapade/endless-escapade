using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;
using EEMod.NPCs.CoralReefs;
using EEMod.Projectiles.Enemy;
using EEMod.Extensions;
using EEMod.Prim;
using System.Linq;

namespace EEMod.Projectiles.Summons
{
    public class PrismaticCaneProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prismatic Cane");
        }

        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 24;
            projectile.timeLeft = 999999999;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.extraUpdates = 12;
        }

        private NPC spire = null;
        private Vector2 origin = Vector2.Zero;
        private bool active = false;
        private bool firstFrame = true;
        private bool dead = false;
        private float speen;
        private Vector2 desiredTarget;
        private float spid;

        public override void AI()
        {
            projectile.timeLeft = 999999999;

                        Vector2 desiredVector = (spire.Center + new Vector2(-2, 2)) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / projectile.ai[0] * projectile.ai[1]) + Main.GameUpdateCount * 2)) * 48;

                        if (Vector2.Distance(projectile.Center, desiredVector) > 10)
                            projectile.Center += Vector2.Normalize(desiredVector - projectile.Center) * 4;
                        else
                            projectile.Center = desiredVector;

                        var proj = Main.projectile.Where(x => Vector2.DistanceSquared(x.Center, projectile.Center) <= 24 * 24 && x.type == ModContent.ProjectileType<SpireLaser>() && x.ai[0] > 0 && x.active);
                        foreach (var laser in proj)
                        {
                            switch (laser.ai[1])
                            {
                                case 0:
                                    break;
                                case 1: //Blue
                                    strikeColor = Color.Blue;
                                    break;
                                case 2: //Cyan
                                    strikeColor = Color.Cyan;
                                    break;
                                case 3: //Pink
                                    strikeColor = Color.Magenta;
                                    break;
                                case 4: //Purple
                                    strikeColor = Color.Purple;
                                    break;
                            }

                            strikeTime = 60;

                            dead = true;
                            laser.Kill();
                        }
        }


        float HeartBeat;
        private int frame;
        private int frameTimer;
        private int frameSpeed = 0;

        private Color strikeColor;
        private int strikeTime;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (spire != null)
            {
                HeartBeat = spire.ai[3];
                projectile.scale = 1f + (HeartBeat / 5f);

                Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Tiles/Foliage/Coral/AquamarineLamp1Glow");
                Texture2D mask = ModContent.GetInstance<EEMod>().GetTexture("Masks/SmoothFadeOut");

                float sineAdd = (float)Math.Sin(Main.GameUpdateCount / 20f) + 2.5f;
                Main.spriteBatch.Draw(mask, projectile.position, null, new Color(sineAdd, sineAdd, sineAdd, 0) * 0.2f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                frameSpeed = 3 + (int)(Math.Sin(Main.GameUpdateCount / 60f) * 2);
                if (frameSpeed > 0)
                {
                    frameTimer++;
                    if (frameTimer >= frameSpeed)
                    {
                        frame++;
                        frameTimer = 0;
                    }
                    if (frame >= 8) frame = 0;
                }

                Vector2 desiredVector = (spire.Center + new Vector2(-2, 2)) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / projectile.ai[0] * (projectile.ai[1] + 1)) + Main.GameUpdateCount * 2)) * 48;

                if (Vector2.Distance(spire.Center, projectile.Center) <= 52)
                {
                    float n = 1 / (desiredVector - projectile.Center).Length();

                    for (float k = 0; k < 1; k += n)
                    {
                        Main.spriteBatch.Draw(mod.GetTexture("Particles/Square"), projectile.Center + (desiredVector - projectile.Center) * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.Lerp(Color.Cyan, Color.Magenta, (float)Math.Sin(Main.GameUpdateCount / 30f)), (desiredVector - projectile.Center).ToRotation(), Vector2.One, 2f, SpriteEffects.None, 0);
                    }
                }

                frame = 0;

                if (strikeTime > 0) strikeTime--;

                Helpers.DrawAdditive(mask, projectile.Center.ForDraw(), Color.White * (0.5f + (HeartBeat / 2f)), projectile.scale, projectile.rotation);

                AfterImage.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 150));

                Main.spriteBatch.Draw(tex, projectile.Center.ForDraw(), new Rectangle(0, frame * 24, 22, 24), lightColor, 0f, new Vector2(11, 12), projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(tex, projectile.Center.ForDraw(), new Rectangle(0, frame * 24, 22, 24), Color.Lerp(Color.White * HeartBeat, strikeColor, strikeTime / 60f), 0f, new Vector2(11, 12), projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}