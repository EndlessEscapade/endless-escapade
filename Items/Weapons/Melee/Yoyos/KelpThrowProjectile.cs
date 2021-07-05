using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Extensions;
using System.Collections.Generic;

namespace EEMod.Items.Weapons.Melee.Yoyos
{
    public class KelpThrowProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;

            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 180f;

            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 12f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 18;
            projectile.height = 18;
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
                    if (alphaCounter % 3 <= 0.04f)
                    {
                        int pieCut = Main.rand.Next(6, 8);
                        for (int m = 0; m < pieCut; m++)
                        {
                            int projID = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<KelpThrowBolt>(), 15, 0, Main.myPlayer);
                            Main.projectile[projID].velocity = new Vector2(0.5f, 0f).RotatedBy(m / (float)pieCut * Math.PI * 2);
                            Main.projectile[projID].netUpdate = true;
                        }

                        rings.Add(new KelpRing(projectile.Center));
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
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("Textures/RadialGradient"), projectile.Center - Main.screenPosition, null, new Color((int)(18f * sineAdd), (int)(12f * sineAdd), (int)(2f * sineAdd), 0), 0f, new Vector2(75, 75), Math.Abs(0.33f * (sineAdd + 1)) * projectile.ai[1], SpriteEffects.None, 0f);

            DrawRings(spriteBatch);

            return base.PreDraw(spriteBatch, lightColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;

            Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Items/Weapons/Melee/Yoyos/KelpThrowGlow");
            Main.spriteBatch.Draw(tex, projectile.Center.ForDraw(), null, Color.White * sineAdd, projectile.rotation, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
        }

        private readonly List<KelpRing> rings = new List<KelpRing>();

        private void DrawRings(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < rings.Count; i++)
            {
                KelpRing ring = rings[i];

                ring.Update();
                ring.Draw(spriteBatch);

                rings[i] = ring;

                if (ring.Alpha <= 0f)
                {
                    rings.RemoveAt(i);
                }
            }
        }

        private struct KelpRing
        {
            public Vector2 Position;

            public float Scale;

            public float Alpha;

            public KelpRing(Vector2 position)
            {
                Position = position;
                Scale = 0f;
                Alpha = 1f;
            }

            public void Update()
            {
                Scale += 0.02f;
                Alpha -= 0.01f;
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                Texture2D ring = ModContent.GetTexture("EEMod/Textures/inverseradial");

                Helpers.DrawAdditive(ring, Position.ForDraw(), Color.Gold * Alpha, Scale);
            }
        }
    }
}