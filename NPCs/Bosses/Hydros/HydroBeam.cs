using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Hydros
{
    public class HydroBeam : EEProjectile // Thanks to Dan Yami for the code
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydro Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            // Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.timeLeft = 3600;
            // Projectile.tileCollide = false;
            Projectile.hostile = true;
        }

        internal const float charge = 240;
        public float LaserLength { get => Projectile.localAI[1]; set => Projectile.localAI[1] = value; }
        public const float LaserLengthMax = 2000f;
        private int multiplier = 1;

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        private float attackCounter = 0;
        private float chargeCounter = 0;

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

        private NPC OwnerNpc => Main.npc[(int)Projectile.ai[1]];

        public override void AI()
        {
            Projectile.Center = OwnerNpc.Center;
            if (Projectile.ai[0] == 0)
            {
                chargeCounter = 5;
            }
            else if (Projectile.ai[0] >= 20)
            {
                chargeCounter += 5f * multiplier;
            }

            Projectile.ai[0]++;
            if (chargeCounter == charge)
            {
                Projectile.hostile = true;
            }
            if (chargeCounter >= charge + 60f && multiplier == 1)
            {
                multiplier = -1;
            }
            if (multiplier == -1 && chargeCounter <= 0)
            {
                Projectile.Kill();
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            float[] sampleArray = new float[2];
            Collision.LaserScan(Projectile.Center, Projectile.velocity, 0, LaserLengthMax, sampleArray);
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
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * LaserLength, projHitbox.Width, ref collisionPoint);
        }

        public override bool? CanCutTiles()
        {
            DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
            //Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * LaserLength, Projectile.width * Projectile.scale * 2, new Utils.PerLinePoint(CutTilesAndBreakWalls));
            return true;
        }

        private bool CutTilesAndBreakWalls(int x, int y)
        {
            return DelegateMethods.CutTiles(x, y);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            Texture2D projectileTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D beamTexture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Bosses/Hydros/HydroBeam_Beam").Value;
            Texture2D beamEndTexture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Bosses/Hydros/HydroBeam_End").Value;
            float laserLength = LaserLength;
            Color color44 = Color.White * 0.8f;
            Texture2D projectileTexture2 = projectileTexture;
            Vector2 arg_AF99_2 = Projectile.Center + new Vector2(0, Projectile.gfxOffY) - Main.screenPosition;
            Rectangle? sourceRectangle2 = null;
            Main.spriteBatch.Draw(projectileTexture2, arg_AF99_2, sourceRectangle2, color44, Projectile.rotation, projectileTexture.Size() / 2f, new Vector2(Math.Min(chargeCounter, charge) / charge, 1f), SpriteEffects.None, 0f);
            laserLength -= (projectileTexture.Height / 2 + beamEndTexture.Height) * Projectile.scale;
            Vector2 value20 = Projectile.Center + new Vector2(0, Projectile.gfxOffY);
            value20 += Projectile.velocity * Projectile.scale * projectileTexture.Height / 2f;
            if (laserLength > 0f)
            {
                float num229 = 0f;
                Rectangle rectangle7 = new Rectangle(0, 16 * (Projectile.timeLeft / 3 % 5), beamTexture.Width, 16);
                while (num229 + 1f < laserLength)
                {
                    if (laserLength - num229 < rectangle7.Height)
                    {
                        rectangle7.Height = (int)(laserLength - num229);
                    }
                    Main.spriteBatch.Draw(beamTexture, value20 - Main.screenPosition, new Rectangle?(rectangle7), color44, Projectile.rotation, new Vector2(rectangle7.Width / 2, 0f), new Vector2(Math.Min(chargeCounter, charge) / charge, 1f), SpriteEffects.None, 0f);
                    num229 += rectangle7.Height * Projectile.scale;
                    value20 += Projectile.velocity * rectangle7.Height * Projectile.scale;
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
            spriteBatch2.Draw(arg_B1FF_1, arg_B1FF_2, sourceRectangle2, color44, Projectile.rotation, beamEndTexture.Frame(1, 1, 0, 0).Top(), new Vector2(Math.Min(chargeCounter, charge) / charge, 1f), SpriteEffects.None, 0f);
            return false;
        }
    }
}