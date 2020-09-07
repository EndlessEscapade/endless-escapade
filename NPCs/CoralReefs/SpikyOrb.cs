using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.Tiles.Furniture.OrbHolder;

namespace EEMod.NPCs.CoralReefs
{
    public class SpikyOrb : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SpikyOrb");
        }

        public int rippleCount = 2;
        public int rippleSize = 13;
        public int rippleSpeed = 200;
        public float distortStrength = 5;
        private float alpha;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            alpha += 0.05f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            EEMod.White.CurrentTechnique.Passes[0].Apply();
            EEMod.White.Parameters["alpha"].SetValue(((float)Math.Sin(alpha) + 1) * 0.5f);
            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center.ForDraw() + new Vector2(0, 3), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale * 1.05f, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.alpha = 20;
            npc.lifeMax = 1000000;
            npc.width = 128;
            npc.height = 130;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.knockBackResist = 0f;
        }

        public override bool CheckActive()
        {
            return false;
        }

        private bool isPicking;
        private bool otherPhase;
        private bool otherPhase2;
        private float t;
        private readonly Vector2[] Holder = new Vector2[2];
        private readonly List<List<Dust>> dustHandler = new List<List<Dust>>();
        private readonly List<float> rotHandler = new List<float>();
        private readonly List<float> rotHandlerSquare = new List<float>();
        private readonly float[] PerlinStrip = new float[720];

        public override void AI()
        {
            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, npc.Center) < 1000 * 1000)
            {
                Dust dust = Dust.NewDustPerfect(npc.Center, DustID.PurpleCrystalShard, new Vector2(Main.rand.NextFloat(-2f, 2f), -5));
                dust.velocity *= 0.99f;
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.noLight = false;
                /*if (dustHandler.Count == 0)
                {
                PerlinStrip = EEWorld.EEWorld.PerlinArrayNoZero(3720, 1, new Vector2(50, 100),100);
                }

                if (dustHandler.Count < 72)
                {
                    int rot = Main.rand.Next(accuracy);
                    rotHandler.Add(rot);
                    rotHandlerSquare.Add(0);
                    List<Dust> Vertices = new List<Dust>();
                    for (int i = 0; i < noOfSubParts; i++)
                    {
                        Vertices.Add(Dust.NewDustPerfect(npc.Center, DustID.PurpleCrystalShard));
                    }
                    dustHandler.Add(Vertices);
                }
                for (int i = 0; i < rotHandler.Count; i++)
                {
                    rotHandler[i]++;
                    rotHandlerSquare[i]++;
                    if (rotHandler[i] > 3720 - 1)
                    {
                        for (int j = 0; j < rotHandlerSquare.Count; j++)
                        {
                            rotHandlerSquare[j] = 0;
                        }
                    }
                    for (int j = 0; j < noOfSubParts; j++)
                    {
                        float per = PerlinStrip[(int)rotHandlerSquare[i]];
                        float baseOfMovement = i * 10 * (MathHelper.Pi / (accuracy * 0.5f));
                        float Extra = (float)(Math.Sin(j / 4f * per) * 155 * per);
                        float xdist = (int)(Math.Sin(baseOfMovement) * (150 - (per * 50) + Extra));
                        float ydist = (int)(Math.Cos(baseOfMovement) * (150 - (per * 50) + Extra));
                        Vector2 offset = new Vector2(xdist * (per + 1) * 0.8f, ydist * (per + 1) * 0.8f);
                        dustHandler[i][j].position = npc.Center + offset;
                    }
                }
                foreach (List<Dust> Dusts in dustHandler)
                {
                    foreach (Dust dust in Dusts)
                    {
                        dust.noGravity = true;
                        dust.velocity *= 0.94f;
                        dust.noLight = false;
                        dust.fadeIn = 1f;
                    }
                }*/
            }
            else
            {
                dustHandler.Clear();
                rotHandler.Clear();
                rotHandlerSquare.Clear();
            }
            npc.ai[0] += 0.05f;
            if (!otherPhase)
            {
                npc.position.Y += (float)Math.Sin(npc.ai[0]) / 4f;
            }

            if (npc.life == 0)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Shockwave"].IsActive())
                {
                    Filters.Scene["EEMod:Shockwave"].Deactivate();
                }
            }
            if (Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
            {
                npc.Center = Main.player[(int)npc.ai[1]].Center - new Vector2(0, 80);
                if (Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
                {
                    Main.player[(int)npc.ai[1]].bodyFrame.Y = 56 * 5;
                }
            }
            if (isPicking && !Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
            {
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos == Vector2.Zero)
                {
                    otherPhase = true;
                    Holder[0] = npc.Center;
                    Holder[1] = Main.MouseWorld;
                }
                else
                {
                    otherPhase2 = true;
                    Holder[0] = npc.Center;
                    Holder[1] = Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos + new Vector2(70, 60);
                }
            }
            if (otherPhase)
            {
                t += 0.01f;
                if (t <= 1)
                {
                    Vector2 mid = (Holder[0] + Holder[1]) / 2;
                    npc.Center = Helpers.TraverseBezier(Holder[1], Holder[0], mid - new Vector2(0, 300), mid - new Vector2(0, 300), t);
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, false, true, 0);
                }
                else if (t <= 1.3f)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, true, false, 10);
                }
                else
                {
                    t = 0;
                    otherPhase = false;
                }
            }
            else if (otherPhase2)
            {
                t += 0.01f;
                if (t <= 1)
                {
                    Vector2 mid = (Holder[0] + Holder[1]) / 2;
                    npc.Center = Helpers.TraverseBezier(Holder[1], Holder[0], mid - new Vector2(0, 300), mid - new Vector2(0, 300), t);
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, false, true, 0);
                }
                else if (t <= 1.3f)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(npc.Center, 16f, true, false, 10);
                }
                else
                {
                    Tile tile = Main.tile[(int)(Holder[1].X / 16), (int)(Holder[1].Y / 16)];
                    int index = ModContent.GetInstance<OrbHolderTE>().Find((int)(Holder[1].X / 16 - tile.frameX / 16), (int)(Holder[1].Y / 16 - tile.frameY / 16));
                    if (index != -1)
                    {
                        OrbHolderTE TE = (OrbHolderTE)TileEntity.ByID[index];
                        TE.hasOrb = true;
                    }
                    t = 0;
                    otherPhase2 = false;
                    for (int i = 0; i < 50; i++)
                    {
                        Vector2 position = npc.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 50 * i)) * 30;
                        //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                        Dust dust = Dust.NewDustPerfect(position, DustID.PurpleCrystalShard);
                        dust.noGravity = true;
                        dust.velocity = Vector2.Normalize(dust.position - npc.Center) * 4;
                        dust.noLight = false;
                        dust.fadeIn = 1f;
                    }
                    npc.life = 0;
                    npc.timeLeft = 0;
                }
            }
            else
            {
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
            }
            isPicking = Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp;
        }

        public int size = 128;
        public int sizeGrowth;
        public float num88 = 1;
    }
}