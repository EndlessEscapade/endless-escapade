using EEMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Banners;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.NPCs.Bosses.Hydros
{
    public class LightningSeahorse : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 5)
                {
                    frameNumber = 0;
                }
                NPC.frame.Y = frameNumber * (504 / 7);
            }
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 50;
            NPC.defense = 6;
            NPC.damage = 20;
            NPC.width = 126;
            NPC.height = 76;
            NPC.aiStyle = 0;
            NPC.knockBackResist = 10;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if (NPC.wet)
            {
                if (target.WithinRange(NPC.Center, 6400))
                {
                    NPC.velocity = Vector2.Normalize(target.Center - NPC.Center) * 4;
                }
            }

            NPC.rotation = NPC.velocity.X / 6;
        }

        public Color lythenGold = new Color(231, 197, 60);
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //Outline layer


            //130 x 80
            //126 x 76

            ticker++;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            //Hydros layer 1

            ApplyIntroShader(1f, new Vector2(126, 76), Vector2.Zero, new Vector2(1f, 1f));

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorse").Value, NPC.Center - Main.screenPosition, new Rectangle(0, frameNumber * 76, 126, 76), Color.White, NPC.rotation, new Vector2(63, 38), 1f, SpriteEffects.None, 0f);

            //Hydros layer 2

            ApplyIntroShader(1f, new Vector2(126, 76), new Vector2(0.8f, 0.5f), new Vector2(-1f, -1f));

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorse").Value, NPC.Center - Main.screenPosition, new Rectangle(0, frameNumber * 76, 126, 76), Color.White, NPC.rotation, new Vector2(63, 38), 1f, SpriteEffects.None, 0f);

            //Hydros layer 3

            ApplyIntroShader(1f, new Vector2(126, 76), new Vector2(0.3f, 0.8f), new Vector2(1f, -1f));

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorse").Value, NPC.Center - Main.screenPosition, new Rectangle(0, frameNumber * 76, 126, 76), Color.White, NPC.rotation, new Vector2(63, 38), 1f, SpriteEffects.None, 0f);

            //Hydros layer 4

            ApplyIntroShader(1f, new Vector2(126, 76), new Vector2(0.7f, 0.6f), new Vector2(-1f, 1f));

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorse").Value, NPC.Center - Main.screenPosition, new Rectangle(0, frameNumber * 76, 126, 76), Color.White, NPC.rotation, new Vector2(63, 38), 1f, SpriteEffects.None, 0f);

            //Outline

            ApplyIntroShader(1f, new Vector2(130, 80), Vector2.Zero, Vector2.One, true, 0.5f);

            for (int k = 0; k < 4; k++)
            {
                Vector2 initRot = Vector2.UnitY * 2f;

                spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorseOutlineNoEyes").Value, NPC.Center + initRot.RotatedBy((ticker / 30f) + (k * 1.57f)) - Main.screenPosition, new Rectangle(0, frameNumber * 80, 130, 80), Color.White, NPC.rotation, new Vector2(65, 40), 1f, SpriteEffects.None, 0f);
            }

            for (int k = 0; k < 4; k++)
            {
                Vector2 initRot = Vector2.UnitY * 2f;

                spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorseOutlineEyes").Value, NPC.Center + initRot.RotatedBy((ticker / 30f) + (k * 1.57f)) - Main.screenPosition, new Rectangle(0, frameNumber * 76, 130, 76), Color.White, NPC.rotation, new Vector2(65, 38), 1f, SpriteEffects.None, 0f);
            }

            ApplyIntroShader(1f, new Vector2(130, 80), Vector2.Zero, Vector2.One, true);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorseOutlineNoEyes").Value, NPC.Center - Main.screenPosition, new Rectangle(0, frameNumber * 80, 130, 80), Color.White, NPC.rotation, new Vector2(65, 40), 1f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            
            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Bosses/Hydros/LightningSeahorseOutlineEyes").Value, NPC.Center - Main.screenPosition, new Rectangle(0, frameNumber * 76, 130, 76), Color.White, NPC.rotation, new Vector2(65, 38), 1f, SpriteEffects.None, 0f);

            return false;
        }

        public int ticker;
        public void ApplyIntroShader(float lerpVal, Vector2 scale, Vector2 offset, Vector2 timeMultiplier, bool invert = false, float alpha = 1f)
        {
            EEMod.HydrosEmerge.Parameters["newColor"].SetValue(new Vector4(lythenGold.R / 255f, lythenGold.G / 255f, lythenGold.B / 255f, 1f));

            EEMod.HydrosEmerge.Parameters["lerpVal"].SetValue(lerpVal);
            EEMod.HydrosEmerge.Parameters["thresh"].SetValue(lerpVal);

            EEMod.HydrosEmerge.Parameters["time"].SetValue(new Vector2((((int)(ticker / 2) * 2f) / 480f) * timeMultiplier.X, (((int)(ticker / 2) * 2f) / 480f) * timeMultiplier.Y));
            
            EEMod.HydrosEmerge.Parameters["invert"].SetValue(invert);

            EEMod.HydrosEmerge.Parameters["alpha"].SetValue(alpha);

            EEMod.HydrosEmerge.Parameters["offset"].SetValue(((NPC.Center / 600f) / 2) * 2f);

            EEMod.HydrosEmerge.Parameters["frames"].SetValue(7);

            EEMod.HydrosEmerge.Parameters["noiseBounds"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelatedBloom").Value.Bounds.Size());
            EEMod.HydrosEmerge.Parameters["imgBounds"].SetValue(scale);

            EEMod.HydrosEmerge.Parameters["noiseTexture"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("Textures/Noise/LightningNoisePixelatedBloom").Value);

            EEMod.HydrosEmerge.CurrentTechnique.Passes[0].Apply();
        }

        public override void OnKill()
        {
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                EEWorld.EEWorld.instance.minionsKilled++;
            }
        }
    }
}