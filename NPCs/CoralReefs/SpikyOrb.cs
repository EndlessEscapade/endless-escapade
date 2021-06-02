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
using EEMod.Items.Placeables.Ores;

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
            Helpers.DrawAdditiveFunky(ModContent.GetInstance<EEMod>().GetTexture("Textures/RadialGradientWide"), npc.Center.ForDraw(), new Color(48, 25, 52), 1.4f, 0.8f);

            alpha += 0.05f;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            EEMod.White.CurrentTechnique.Passes[0].Apply();
            EEMod.White.Parameters["alpha"].SetValue(((float)Math.Sin(alpha) + 1) * 0.5f);
            EEMod.White.Parameters["color"].SetValue(new Vector3(1, 1, 1));
            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center.ForDraw(), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale * 1.05f, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            EEMod.ReflectionShader.Parameters["alpha"].SetValue(alpha * 2 % 6);
            EEMod.ReflectionShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.ReflectionShader.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().GetTexture("Textures/SpikyOrbLightMap"));
            EEMod.ReflectionShader.Parameters["lightColour"].SetValue(drawColor.ToVector3());
            EEMod.ReflectionShader.Parameters["shaderLerp"].SetValue(1f);
            EEMod.ReflectionShader.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center.ForDraw(), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
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

        public bool isPicking;
        private bool otherPhase;
        private bool otherPhase2;
        private float t;
        private readonly Vector2[] Holder = new Vector2[2];
        private readonly List<List<Dust>> dustHandler = new List<List<Dust>>();
        private readonly List<float> rotHandler = new List<float>();
        private readonly List<float> rotHandlerSquare = new List<float>();
        private readonly float[] PerlinStrip = new float[720];
        bool flag;
        public override void AI()
        {
            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, npc.Center) < 1000 * 1000)
            {

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
                npc.position.Y += (float)Math.Sin(npc.ai[0]) / 2f;

                npc.ai[3]++;
                float lasdlasld = (0.3f + ((int)Math.Sin(npc.ai[3])));
                Lighting.AddLight(npc.Center, new Vector3(0.1f * lasdlasld, 0.1f * lasdlasld, 1 * lasdlasld));
            }

            if (npc.life == 0)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Shockwave"].IsActive())
                {
                    Filters.Scene["EEMod:Shockwave"].Deactivate();
                }
            }
            if (Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp && isPicking)
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
                flag = false;
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
            else if (!flag)
            {
                flag = true;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
            }


            if (Helpers.isCollidingWithWall(npc))
            {
                npc.life = 0;
                npc.timeLeft = 0;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                npc.NPCLoot();
                if (!a)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Gore gore = Gore.NewGorePerfect(npc.Center, Vector2.Zero, mod.GetGoreSlot("Gores/SpikyOrb" + (i + 1)), 1);
                        gore.velocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
                    }
                    a = true;
                }
            }
            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, npc.Center) < 1000 * 1000)
            {
                isPicking = Main.player[(int)npc.ai[1]].GetModPlayer<EEPlayer>().isPickingUp;

            }
        }
        bool a;
        public override void NPCLoot()
        {
            Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ModContent.ItemType<LythenOre>(), Main.rand.Next(10, 15));
            switch (Main.rand.Next(3))
            {
                case 0:
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ItemID.Sapphire, Main.rand.Next(1, 4));
                    break;
                case 1:
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ItemID.Emerald, Main.rand.Next(1, 4));
                    break;
                case 2:
                    Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ItemID.Diamond, Main.rand.Next(1, 4));
                    break;
            }
        }

        public int size = 128;
        public int sizeGrowth;
        public float num88 = 1;
    }
}