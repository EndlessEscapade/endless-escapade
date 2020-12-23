using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture.Coral;

namespace EEMod.NPCs.CoralReefs
{
    public class AquamarineSpire : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Spire");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.lifeMax = 1000000;
            npc.width = 320;
            npc.height = 416;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.behindTiles = true;
        }
        float alpha;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            alpha += 0.05f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            EEMod.SolidOutline.CurrentTechnique.Passes[0].Apply();
            EEMod.SolidOutline.Parameters["alpha"].SetValue(((float)Math.Sin(alpha / 2f) + 1) * 0.5f);
            // Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center.ForDraw() + new Vector2(0,4), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale * 1.01f, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }
        public override bool CheckActive()
        {
            return false;
        }


        float HeartBeat;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float timeBetween = 70;
            float bigTimeBetween = 200;
            if (Main.GameUpdateCount % 200 < timeBetween)
            {
                HeartBeat = Math.Abs((float)Math.Sin((Main.GameUpdateCount % bigTimeBetween) * (6.28f / timeBetween))) * (1 - (Main.GameUpdateCount % bigTimeBetween) / (timeBetween * 1.5f));
            }
            else
            {
                HeartBeat = 0;
            }
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireGlow"), npc.Center.ForDraw() + new Vector2(0, 5), npc.frame, Color.White * HeartBeat, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            /*for (int i = (int)Main.screenPosition.X / 16; i < (Main.screenPosition.X + Main.screenWidth) / 16; i++)
            {
                for (int j = (int)Main.screenPosition.Y / 16; j < (Main.screenPosition.Y + Main.screenHeight) / 16; j++)
                {
                    if (Main.tile[i, j].type == ModContent.TileType<AquamarineLamp1>())
                    {
                        int frameX = Main.tile[i, j].frameX;
                        int frameY = Main.tile[i, j].frameY;
                        if (frameX == 0 && frameY == 0)
                        {
                            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                            if (Main.drawToScreen)
                            {
                                zero = Vector2.Zero;
                            }

                            Texture2D tex = EEMod.instance.GetTexture("Tiles/Furniture/Coral/AquamarineLamp1Glow");
                            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, (j - 1) * 16 - (int)Main.screenPosition.Y) + zero;
                            Main.spriteBatch.Draw(tex, position + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4), tex.Bounds, Color.White * ((HeartBeat / 2) + 0.5f), 0f, default, 1f, SpriteEffects.None, 1f);
                        }
                    }
                }
            }*/

            Helpers.DrawAdditive(ModContent.GetInstance<EEMod>().GetTexture("Masks/SmoothFadeOut"), npc.position.ForDraw(), Color.White, 1f);
        }

        public override void AI()
        {
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.18f));
            Vector2 one = new Vector2(-10, Main.rand.Next(-10, 10)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
            Vector2 two = new Vector2(10, Main.rand.Next(-10, 10)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
            Vector2 three = new Vector2(Main.rand.Next(-10, 10), 10).RotatedBy(1.57f / 2f + HeartBeat / 60f);
            Vector2 four = new Vector2(Main.rand.Next(-10, 10), -10).RotatedBy(1.57f / 2f + HeartBeat / 60f);
            Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f) + 2 + HeartBeat / 60f);
            int scale = 4;
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + one * scale + offset, -Vector2.Normalize(one) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f));
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + two * scale + offset, -Vector2.Normalize(two) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f));
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + three * scale + offset, -Vector2.Normalize(three) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f));
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + four * scale + offset, -Vector2.Normalize(four) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f));
        }
    }
}