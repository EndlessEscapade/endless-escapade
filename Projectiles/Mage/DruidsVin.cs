using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InteritosMod.Projectiles.Mage
{
    public class DruidsVin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Druid's Vine");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 14;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 3600;
            projectile.tileCollide = false;
        }

        internal const float charge = 240;
        public float LaserLength { get => projectile.localAI[1]; set => projectile.localAI[1] = value; }
        public const float LaserLengthMax = 2000f;
        int multiplier = 1;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }
        float attackCounter = 0;
        float chargeCounter = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
            writer.Write(chargeCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadSingle();
            chargeCounter = reader.ReadSingle();
        }

        private NPC OwnerNpc => Main.npc[(int)projectile.ai[1]];

        public override void AI()
        {
            projectile.Center = OwnerNpc.Center;
            if (projectile.ai[0] == 0)
            {
                chargeCounter = 5;
            }
            else if (projectile.ai[0] >= 20)
                chargeCounter += 5f * multiplier;
            projectile.ai[0]++;
            if (chargeCounter == charge)
            {
                projectile.hostile = true;
            }
            if (chargeCounter >= charge + 60f && multiplier == 1)
            {
                multiplier = -1;
            }
            if (multiplier == -1 && chargeCounter <= 0)
                projectile.Kill();

            projectile.rotation = projectile.velocity.ToRotation() - 1.57079637f;
            projectile.velocity = Vector2.Normalize(projectile.velocity);

            float[] sampleArray = new float[2];
            Collision.LaserScan(projectile.Center, projectile.velocity, 0, LaserLengthMax, sampleArray);
            float sampledLength = 0f;
            for (int i = 0; i < sampleArray.Length; i++)
            {
                sampledLength += sampleArray[i];
            }
            sampledLength /= sampleArray.Length;
            float amount = 0.75f; // last prism is 0.75 rather than 0.5?
            LaserLength = MathHelper.Lerp(LaserLength, sampledLength, amount);
            LaserLength = LaserLengthMax;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * LaserLength, projHitbox.Width, ref collisionPoint);
        }
        public override bool? CanCutTiles()
        {
            DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * LaserLength, projectile.width * projectile.scale * 2, new Utils.PerLinePoint(CutTilesAndBreakWalls));
            return true;
        }

        private bool CutTilesAndBreakWalls(int x, int y)
        {
            return DelegateMethods.CutTiles(x, y);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            Texture2D projectileTexture = Main.projectileTexture[projectile.type];
            Texture2D beamTexture = mod.GetTexture("Projectiles/Mage/DruidsVin_Beam");
            Texture2D beamEndTexture = mod.GetTexture("Projectiles/Mage/DruidsVin_End");
            float laserLength = LaserLength;
            Color color44 = Color.White * 0.8f;
            Texture2D projectileTexture2 = projectileTexture;
            Vector2 arg_AF99_2 = projectile.Center + new Vector2(0, projectile.gfxOffY) - Main.screenPosition;
            Rectangle? sourceRectangle2 = null;
            spriteBatch.Draw(projectileTexture2, arg_AF99_2, sourceRectangle2, color44, projectile.rotation, projectileTexture.Size() / 2f, new Vector2(Math.Min(chargeCounter, charge) / charge, 1f), SpriteEffects.None, 0f);
            laserLength -= (projectileTexture.Height / 2 + beamEndTexture.Height) * projectile.scale;
            Vector2 value20 = projectile.Center + new Vector2(0, projectile.gfxOffY);
            value20 += projectile.velocity * projectile.scale * projectileTexture.Height / 2f;
            if (laserLength > 0f)
            {
                float num229 = 0f;
                Rectangle rectangle7 = new Rectangle(0, 16 * (projectile.timeLeft / 3 % 5), beamTexture.Width, 16);
                while (num229 + 1f < laserLength)
                {
                    if (laserLength - num229 < rectangle7.Height)
                    {
                        rectangle7.Height = (int)(laserLength - num229);
                    }
                    Main.spriteBatch.Draw(beamTexture, value20 - Main.screenPosition, new Rectangle?(rectangle7), color44, projectile.rotation, new Vector2(rectangle7.Width / 2, 0f), new Vector2(Math.Min(chargeCounter, charge) / charge, 1f), SpriteEffects.None, 0f);
                    num229 += rectangle7.Height * projectile.scale;
                    value20 += projectile.velocity * rectangle7.Height * projectile.scale;
                    rectangle7.Y += 16;
                    if (rectangle7.Y + rectangle7.Height > beamTexture.Height)
                    {
                        rectangle7.Y = 0;
                    }
                }
            }
            SpriteBatch spriteBatch2 = Main.spriteBatch;
            Texture2D arg_B1FF_1 = beamEndTexture;
            Vector2 arg_B1FF_2 = value20 - Main.screenPosition;
            sourceRectangle2 = null;
            spriteBatch2.Draw(arg_B1FF_1, arg_B1FF_2, sourceRectangle2, color44, projectile.rotation, beamEndTexture.Frame(1, 1, 0, 0).Top(), new Vector2(Math.Min(chargeCounter, charge) / charge, 1f), SpriteEffects.None, 0f);
            return false;
        }
    }
}