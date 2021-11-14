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

namespace EEMod.NPCs.Aquamarine
{
    public class AquamarineOrb : EENPC
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Helpers.DrawAdditiveFunky(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradientWide").Value, NPC.Center.ForDraw(), new Color(48, 25, 52), 1.4f, 0.8f);
            alpha += 0.05f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            EEMod.WhiteOutline.CurrentTechnique.Passes[0].Apply();
            EEMod.WhiteOutline.Parameters["alpha"].SetValue(((float)Math.Sin(alpha) + 1) * 0.5f);
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value, NPC.Center.ForDraw(), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale * 1.01f, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            EEMod.ReflectionShader.Parameters["alpha"].SetValue(alpha * 2 % 6);
            EEMod.ReflectionShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.ReflectionShader.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/SpikyOrbLightMap").Value);
            EEMod.ReflectionShader.Parameters["lightColour"].SetValue(drawColor.ToVector3());
            EEMod.ReflectionShader.Parameters["shaderLerp"].SetValue(1f);
            EEMod.ReflectionShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value, NPC.Center.ForDraw(), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.friendly = true;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.alpha = 20;
            NPC.lifeMax = 1000000;
            NPC.width = 128;
            NPC.height = 130;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            NPC.damage = 0;
            NPC.knockBackResist = 0f;
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
            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, NPC.Center) < 1000 * 1000)
            {

            }
            else
            {
                dustHandler.Clear();
                rotHandler.Clear();
                rotHandlerSquare.Clear();
            }
            NPC.ai[0] += 0.05f;
            if (!otherPhase)
            {
                NPC.position.Y += (float)Math.Sin(NPC.ai[0]) / 2f;

                NPC.ai[3]++;
                float lasdlasld = (0.3f + ((int)Math.Sin(NPC.ai[3])));
                Lighting.AddLight(NPC.Center, new Vector3(0.1f * lasdlasld, 0.1f * lasdlasld, 1 * lasdlasld));
            }

            if (NPC.life == 0)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Shockwave"].IsActive())
                {
                    Filters.Scene["EEMod:Shockwave"].Deactivate();
                }
            }
            if (Main.player[(int)NPC.ai[1]].GetModPlayer<EEPlayer>().isPickingUp && isPicking)
            {
                NPC.Center = Main.player[(int)NPC.ai[1]].Center - new Vector2(0, 80);
                if (Main.player[(int)NPC.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
                {
                    Main.player[(int)NPC.ai[1]].bodyFrame.Y = 56 * 5;
                }
            }
            if (isPicking && !Main.player[(int)NPC.ai[1]].GetModPlayer<EEPlayer>().isPickingUp)
            {
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos == Vector2.Zero)
                {
                    otherPhase = true;
                    Holder[0] = NPC.Center;
                    Holder[1] = Main.MouseWorld;
                }
                else
                {
                    otherPhase2 = true;
                    Holder[0] = NPC.Center;
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
                    NPC.Center = Helpers.TraverseBezier(Holder[1], Holder[0], mid - new Vector2(0, 300), mid - new Vector2(0, 300), t);
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(NPC.Center, 16f, false, true, 0);
                }
                else if (t <= 1.3f)
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(NPC.Center, 16f, true, false, 10);
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


            if (Helpers.isCollidingWithWall(NPC))
            {
                NPC.life = 0;
                NPC.timeLeft = 0;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                NPC.NPCLoot();
                if (!a)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        //Gore gore = Gore.NewGorePerfect(NPC.Center, Vector2.Zero, Mod.GetGoreSlot("Gores/SpikyOrb" + (i + 1)), 1);
                        //gore.velocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
                    }
                    a = true;
                }
            }
            if (Vector2.DistanceSquared(Main.LocalPlayer.Center, NPC.Center) < 1000 * 1000)
            {
                isPicking = Main.player[(int)NPC.ai[1]].GetModPlayer<EEPlayer>().isPickingUp;

            }
        }
        bool a;
        public override void OnKill()
        {
            Item.NewItem((int)NPC.Center.X, (int)NPC.Center.Y, NPC.width, NPC.height, ModContent.ItemType<LythenOre>(), Main.rand.Next(10, 15));
            switch (Main.rand.Next(3))
            {
                case 0:
                    Item.NewItem((int)NPC.Center.X, (int)NPC.Center.Y, NPC.width, NPC.height, ItemID.Sapphire, Main.rand.Next(1, 4));
                    break;
                case 1:
                    Item.NewItem((int)NPC.Center.X, (int)NPC.Center.Y, NPC.width, NPC.height, ItemID.Emerald, Main.rand.Next(1, 4));
                    break;
                case 2:
                    Item.NewItem((int)NPC.Center.X, (int)NPC.Center.Y, NPC.width, NPC.height, ItemID.Diamond, Main.rand.Next(1, 4));
                    break;
            }
        }

        public int size = 128;
        public int sizeGrowth;
        public float num88 = 1;
    }
}