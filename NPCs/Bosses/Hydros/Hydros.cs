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
using Terraria.GameContent;
using ReLogic.Graphics;

namespace EEMod.NPCs.Bosses.Hydros
{
    [AutoloadBossHead]
    public class Hydros : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void FindFrame(int frameHeight) //Frame counter
        {
            //NPC.TargetClosest(true);
            /*Player player = Main.player[NPC.target];
            if (NPC.frameCounter++ > 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = NPC.frame.Y + frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 7)
            {
                NPC.frame.Y = 0;
                return;
            }*/
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.lifeMax = 1600;
            NPC.defense = 12;
            NPC.damage = 20;
            NPC.knockBackResist = 0;

            NPC.value = Item.buyPrice(0, 3, 5, 0);

            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
            //NPC.Expert = ItemType<HydrosBag>();
            NPC.width = 314;
            NPC.height = 162;

            NPC.boss = true;
            NPC.noGravity = true;

            NPC.noTileCollide = true;

            Music = MusicLoader.GetMusicSlot("EEMod/Sounds/Music/HydrosFight");

            NPC.scale = 1f;
        }

        public int frameNumber = 0;
        public float alpha;

        public int cutsceneTicks = 1;

        public Player target;
        public override void AI()
        {
            target = Main.LocalPlayer;

            NPC.velocity = (Vector2.Normalize(target.Center - NPC.Center) * 2f) + new Vector2(0, (float)Math.Sin(Main.GameUpdateCount / 60f) / 3f);

            NPC.rotation = NPC.velocity.Y / 6f;

            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.spriteDirection = -1;
            }

            NPC.ai[0]++;

            if(NPC.ai[0] == 60)
            {
                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), NPC.Center, new Vector2(0, 0), ModContent.ProjectileType<TeslaBall>(), 0, 0f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (cutsceneTicks <= 1) return false;

            return true;
        }

        public Color lythenGold = new Color(231, 197, 60);
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            #region Intro management stuffens
            cutsceneTicks++;

            alpha = 1 - (cutsceneTicks / 30f);

            alpha = MathHelper.Clamp(alpha, 0f, 1f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            EEMod.WhiteOutline.CurrentTechnique.Passes[0].Apply();
            EEMod.WhiteOutline.Parameters["alpha"].SetValue(alpha);
            EEMod.WhiteOutline.Parameters["color"].SetValue(new Vector4(1f, 1f, 1f, alpha));

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 162, 314, 162), Color.White, NPC.rotation, new Vector2(157, 81), NPC.scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            //Hydros layer 1

            ApplyIntroShader(1f, new Vector2(314, 162), Vector2.Zero, new Vector2(1f, 1f), false, alpha);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 162, 314, 162), Color.White * alpha, NPC.rotation, new Vector2(157, 81), NPC.scale, SpriteEffects.None, 0f);

            //Hydros layer 2

            ApplyIntroShader(1f, new Vector2(314, 162), new Vector2(0.8f, 0.5f), new Vector2(-1f, -1f), false, alpha);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 162, 314, 162), Color.White * alpha, NPC.rotation, new Vector2(157, 81), NPC.scale, SpriteEffects.None, 0f);

            //Hydros layer 3

            ApplyIntroShader(1f, new Vector2(314, 162), new Vector2(0.3f, 0.8f), new Vector2(1f, -1f), false, alpha);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 162, 314, 162), Color.White * alpha, NPC.rotation, new Vector2(157, 81), NPC.scale, SpriteEffects.None, 0f);

            //Hydros layer 4

            ApplyIntroShader(1f, new Vector2(314, 162), new Vector2(0.7f, 0.6f), new Vector2(-1f, 1f), false, alpha);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/Hydros").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 162, 314, 162), Color.White * alpha, NPC.rotation, new Vector2(157, 81), NPC.scale, SpriteEffects.None, 0f);

            //Outline

            ApplyIntroShader(1f, new Vector2(314, 162), Vector2.Zero, Vector2.One, true, alpha * 0.5f);

            for (int k = 0; k < 4; k++)
            {
                Vector2 initRot = Vector2.UnitY * 2f * alpha;

                spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/HydrosOutline").Value, NPC.Center + initRot.RotatedBy((cutsceneTicks / 30f) + (k * 1.57f)) - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 166, 318, 166), lythenGold * alpha, NPC.rotation, new Vector2(159, 83), NPC.scale, SpriteEffects.None, 0f);
            }

            /*for (int k = 0; k < 4; k++)
            {
                Vector2 initRot = Vector2.UnitY * 2f;

                spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorseOutlineEyes").Value, NPC.Center + initRot.RotatedBy((ticker / 30f) + (k * 1.57f)) - Main.screenPosition, new Rectangle(0, frameNumber * 76, 130, 76), Color.White, NPC.rotation, new Vector2(65, 38), 1f, SpriteEffects.None, 0f);
            }*/

            ApplyIntroShader(1f, new Vector2(318, 166), Vector2.Zero, Vector2.One, true, alpha * 0.5f);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/HydrosOutline").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 166, 318, 166), lythenGold * alpha, NPC.rotation, new Vector2(159, 83), NPC.scale, SpriteEffects.None, 0f);

            //spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorseOutlineEyes").Value, NPC.Center - Main.screenPosition, new Rectangle(0, frameNumber * 76, 130, 76), Color.White, NPC.rotation, new Vector2(65, 38), 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/HydrosOutline").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), new Rectangle(0, frameNumber * 166, 318, 166), lythenGold * alpha, NPC.rotation, new Vector2(159, 83), NPC.scale, SpriteEffects.None, 0f);



            //Vector2 orig = NPC.Center - Main.screenPosition;

            //Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/SmooshedRadialGradient").Value, orig + new Vector2(0, -176 + 37), Color.Gold * 0.75f, 2f);

            //string msg1 = "Hydros";
            //string msg2 = "King of the Seahorses";

            //Main.spriteBatch.DrawString(FontAssets.DeathText.Value, msg1, orig + new Vector2(-FontAssets.DeathText.Value.MeasureString(msg1).X / 2f, -200), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            //Main.spriteBatch.DrawString(FontAssets.MouseText.Value, msg2, orig + new Vector2(-FontAssets.MouseText.Value.MeasureString(msg2).X / 2f, -144), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            /*if(ticker > 40 && ticker < 160)
            {
                
            }*/
            #endregion
        }

        public override void OnKill()
        {

        }

        public void ApplyIntroShader(float lerpVal, Vector2 scale, Vector2 offset, Vector2 timeMultiplier, bool invert = false, float alpha = 1f)
        {
            EEMod.HydrosEmerge.Parameters["newColor"].SetValue(new Vector4(lythenGold.R / 255f, lythenGold.G / 255f, lythenGold.B / 255f, 1f));

            EEMod.HydrosEmerge.Parameters["lerpVal"].SetValue(lerpVal);
            EEMod.HydrosEmerge.Parameters["thresh"].SetValue(lerpVal);

            EEMod.HydrosEmerge.Parameters["time"].SetValue(new Vector2((((int)(cutsceneTicks / 2) * 2f) / 480f) * timeMultiplier.X, (((int)(cutsceneTicks / 2) * 2f) / 480f) * timeMultiplier.Y));

            EEMod.HydrosEmerge.Parameters["invert"].SetValue(invert);

            EEMod.HydrosEmerge.Parameters["alpha"].SetValue(alpha);

            EEMod.HydrosEmerge.Parameters["offset"].SetValue(((NPC.Center / 600f) / 2) * 2f);

            EEMod.HydrosEmerge.Parameters["frames"].SetValue(1);

            EEMod.HydrosEmerge.Parameters["noiseBounds"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelatedBloom").Value.Bounds.Size());
            EEMod.HydrosEmerge.Parameters["imgBounds"].SetValue(scale);

            EEMod.HydrosEmerge.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelatedBloom").Value);

            EEMod.HydrosEmerge.CurrentTechnique.Passes[0].Apply();
        }

        /*public override bool CheckDead()
        {
            int goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), default(Vector2), mod.GetGoreSlot("Gores/HydrosGore"), 1f);
            Main.gore[goreIndex].scale = 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

            return true;
        }*/
    }
}