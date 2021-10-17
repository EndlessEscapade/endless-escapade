using EEMod.Items.Materials;
using EEMod.Items.TreasureBags;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Summon.Minions;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger.Guns;
using EEMod.Items.Weapons.Melee.Yoyos;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Prim;
using EEMod.Projectiles.CoralReefs;
using Terraria.DataStructures;
using EEMod.Gores;

namespace EEMod.NPCs.Bosses.Hydros
{
    public class HydrosCutsceneManager : EENPC
    {
        public override string Texture => "EEMod/Empty";

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.lifeMax = 1;
            NPC.defense = 1;
            NPC.damage = 0;
            NPC.knockBackResist = 0;
            NPC.immortal = true;

            NPC.width = 1;
            NPC.height = 1;

            NPC.boss = true;
            NPC.noGravity = true;
            NPC.friendly = true;

            Music = MusicLoader.GetMusicSlot("EEMod/Sounds/Music/HydrosFight");

            NPC.noTileCollide = true;
        }

        public int ticks;

        public Vector2[] positions = new Vector2[3] { new Vector2(52, -28), new Vector2(108, -12), new Vector2(-4, -12) };

        public Vector2[] positionsPhase1 = new Vector2[3]  { new Vector2(52, -28), new Vector2(108, -12), new Vector2(-4, -12) };

        public Vector2 phase2Orig = new Vector2(52, -96);

        public int[] frames = new int[3];
        public int frameCounter;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(!Main.raining) Main.StartRain();

            if(Main.windSpeedCurrent < 0.25f) Main.windSpeedCurrent += 0.05f;
            if (Main.cloudAlpha < 0.6f) Main.cloudAlpha += 0.03f;
            if (Main.cloudAlpha >= 0.5f) Main.cloudAlpha = 0.6f;

            #region Updating lythen ore frames
            frameCounter++;
            if (frameCounter >= 3)
            {
                frameCounter = 0;

                for (int l = 0; l < frames.Length; l++)
                {
                    frames[l]++;

                    if (frames[l] >= 10) frames[l] = 0;
                }
            }
            #endregion

            if (ticks <= 250)
            {
                for (int l = 0; l < 3; l++)
                {
                    Vector2 orig = new Vector2(NPC.position.X - 64, NPC.position.Y) + positions[l] - Main.screenPosition + new Vector2(0, (float)Math.Sin((Main.GameUpdateCount / 20f) + l) * 2f);

                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 initRot = Vector2.UnitY * 4f;

                        Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig + initRot.RotatedBy((Main.GameUpdateCount / 60f) + (k * 1.57f)), new Rectangle(0, 0, 24, 24), Color.Gold * 0.5f * MathHelper.Clamp((360 - ticks) / 20f, 0f, 1f));
                    }

                    spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Items/Placeables/Ores/LythenOre").Value, orig, new Rectangle(0, frames[l] * 28, 28, 28), Color.White * MathHelper.Clamp((300 - ticks) / 5f, 0f, 1f));
                }
            }

            ticks++;

            if (ticks < 120)
            {
                for (int l = 0; l < 3; l++)
                {
                    positions[l] = Vector2.Lerp(positionsPhase1[l], phase2Orig + (Vector2.UnitY.RotatedBy((l * (2 * MathHelper.Pi / 3f)) + ((ticks - 120) / (-10 - (MathHelper.Clamp(240 - ticks - 10, -10, 110) / 24f))) - MathHelper.Pi) * ((250 - ticks) / 2f)), ticks / 120f);
                }
            }
            else if (ticks < 240)
            {
                for (int l = 0; l < 3; l++)
                {
                    positions[l] = phase2Orig + (Vector2.UnitY.RotatedBy((l * (2 * MathHelper.Pi / 3f)) + ((ticks - 120) / (-10 - (MathHelper.Clamp(240 - ticks - 10, -10, 110) / 24f))) - MathHelper.Pi) * ((250 - ticks) / 2f));
                }

                if (ticks == 239)
                {
                    int lightningproj = Projectile.NewProjectile(new ProjectileSource_NPC(NPC), new Vector2(NPC.position.X - 64 + 12, NPC.position.Y + 12) + phase2Orig + new Vector2(0, -Main.screenHeight / 2f), Vector2.Zero, ModContent.ProjectileType<TeslaCoralProj>(), 20, 2.5f);

                    if (Main.netMode != NetmodeID.Server)
                    {
                        PrimSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj], 6f));
                    }

                    TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                    zappy.target = phase2Orig + new Vector2(NPC.position.X - 64 + 28, NPC.position.Y + 28);
                }
            }
            else if (ticks < 255)
            {
                Texture2D tex = ModContent.Request<Texture2D>("EEMod/Textures/GodrayMask").Value;

                if (ticks == 248)
                {
                    NPC.NewNPC((int)phase2Orig.X + (int)NPC.position.X - 64 + 12, (int)phase2Orig.Y + (int)NPC.position.Y + 12 + (162 / 2), ModContent.NPCType<Hydros>());

                    Gore.NewGore(phase2Orig + NPC.position + new Vector2(-64, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), ModContent.GoreType<LythenGore1>());
                    Gore.NewGore(phase2Orig + NPC.position + new Vector2(-64 + 12, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), ModContent.GoreType<LythenGore2>());

                    Gore.NewGore(phase2Orig + NPC.position + new Vector2(-64, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), ModContent.GoreType<LythenGore1>());
                    Gore.NewGore(phase2Orig + NPC.position + new Vector2(-64 + 12, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), ModContent.GoreType<LythenGore2>());

                    Gore.NewGore(phase2Orig + NPC.position + new Vector2(-64, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), ModContent.GoreType<LythenGore1>());
                    Gore.NewGore(phase2Orig + NPC.position + new Vector2(-64 + 12, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), ModContent.GoreType<LythenGore2>());

                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(NPC.Center + new Vector2(NPC.position.X - 64 + 12, NPC.position.Y + 12) + phase2Orig, 16f, true, false, 10);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f), (float)Math.Cos(Main.GameUpdateCount / 60f) * 0.1f));
                EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
                EEMod.RadialField.Parameters["alpha"].SetValue(0.8f);
                EEMod.RadialField.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise2").Value);
                EEMod.RadialField.Parameters["color"].SetValue(new Vector4(Color.Gold.R, Color.Gold.G, Color.Gold.B, Color.Gold.A) / 255f);
                EEMod.RadialField.CurrentTechnique.Passes[0].Apply();

                if(ticks >= 248) spriteBatch.Draw(tex, phase2Orig + new Vector2(NPC.position.X - 64 + 12, NPC.position.Y + 12) - Main.screenPosition, null, Color.Gold, (Main.GameUpdateCount / 20f), tex.Bounds.Size() / 2f, (MathHelper.Clamp(ticks, 248, 255) - 240) / 2.5f, SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f), (float)Math.Cos(Main.GameUpdateCount / 60f) * 0.1f));
                EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
                EEMod.RadialField.Parameters["alpha"].SetValue(0.5f);
                EEMod.RadialField.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise2").Value);
                EEMod.RadialField.Parameters["color"].SetValue(new Vector4(Color.Gold.R, Color.Gold.G, Color.Gold.B, Color.Gold.A) / 255f);
                EEMod.RadialField.CurrentTechnique.Passes[0].Apply();

                if (ticks >= 248) spriteBatch.Draw(tex, phase2Orig + new Vector2(NPC.position.X - 64 + 12, NPC.position.Y + 12) - Main.screenPosition, null, Color.Gold, -(Main.GameUpdateCount / 20f), tex.Bounds.Size() / 2f, (MathHelper.Clamp(ticks, 248, 255) - 240) / 5f, SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }
            else if (ticks < 290)
            {
                if (ticks >= 263) Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();

                Texture2D tex = ModContent.Request<Texture2D>("EEMod/Textures/GodrayMask").Value;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f), (float)Math.Cos(Main.GameUpdateCount / 60f) * 0.1f));
                EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
                EEMod.RadialField.Parameters["alpha"].SetValue(0.8f * ((480 - ticks) / 120f));
                EEMod.RadialField.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise2").Value);
                EEMod.RadialField.Parameters["color"].SetValue(new Vector4(Color.Gold.R, Color.Gold.G, Color.Gold.B, Color.Gold.A) / 255f);
                EEMod.RadialField.CurrentTechnique.Passes[0].Apply();

                spriteBatch.Draw(tex, phase2Orig + new Vector2(NPC.position.X - 64 + 12, NPC.position.Y + 12) - Main.screenPosition, null, Color.Gold, (Main.GameUpdateCount / 20f), tex.Bounds.Size() / 2f, ((290 - ticks) / 7.5f), SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f), (float)Math.Cos(Main.GameUpdateCount / 60f) * 0.1f));
                EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
                EEMod.RadialField.Parameters["alpha"].SetValue(0.5f * ((480 - ticks) / 120f));
                EEMod.RadialField.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise2").Value);
                EEMod.RadialField.Parameters["color"].SetValue(new Vector4(Color.Gold.R, Color.Gold.G, Color.Gold.B, Color.Gold.A) / 255f);
                EEMod.RadialField.CurrentTechnique.Passes[0].Apply();

                spriteBatch.Draw(tex, phase2Orig + new Vector2(NPC.position.X - 64 + 12, NPC.position.Y + 12) - Main.screenPosition, null, Color.Gold, -(Main.GameUpdateCount / 20f), tex.Bounds.Size() / 2f, ((290 - ticks) / 15f), SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }
            else
            {
                NPC.life = 0;
            }

            return false;
        }
    }
}