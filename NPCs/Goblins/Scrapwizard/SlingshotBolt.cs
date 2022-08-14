using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class SlingshotBolt : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slingshot Bolt");
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;

            NPC.alpha = 0;

            NPC.friendly = false;
            NPC.scale = 1f;

            NPC.aiStyle = -1;
            NPC.lifeMax = 1;

            NPC.noGravity = true;

            NPC.defense = 0;

            NPC.noTileCollide = false;

            NPC.DeathSound = SoundID.Item10;

            NPC.damage = 20;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 1)
            {
                NPC.velocity = Vector2.Normalize(Main.player[(int)NPC.ai[1]].Center - NPC.Center) * 21f;

                NPC.ai[0] = 2;
            }
            else if (NPC.ai[0] == 0)
            {
                NPC.velocity = Vector2.Zero;
            }
            else
            {
                if ((NPC.position.X == NPC.oldPosition.X) || (NPC.position.Y == NPC.oldPosition.Y) || (NPC.velocity.X != NPC.oldVelocity.X) || (NPC.velocity.Y != NPC.oldVelocity.Y))
                {
                    NPC.StrikeNPC(100000, 0, 0);
                }
            }

            NPC.rotation = (Main.player[(int)NPC.ai[1]].Center - NPC.Center).ToRotation();

            //NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(Main.LocalPlayer.Center - NPC.Center) * 8f, 0.02f);
        }

        public override bool? CanFallThroughPlatforms()
        {
            return true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void OnKill()
        {
            NPC.velocity = Vector2.Zero;

            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(NPC.position, 0, 0, DustID.CrystalSerpent_Pink);
                Main.dust[dust].velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-1, 2), Main.rand.NextFloat(-1, 2));
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(Main.graphics.GraphicsDevice.Viewport.Width / 2, Main.graphics.GraphicsDevice.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            EEMod.LightningShader.Parameters["maskTexture"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Goblins/Scrapwizard/ScrapwizardHexBolt").Value);

            EEMod.LightningShader.Parameters["newColor"].SetValue(new Vector4(Color.Violet.R, Color.Violet.G, Color.Violet.B, Color.Violet.A) / 255f);

            EEMod.LightningShader.Parameters["transformMatrix"].SetValue(view * projection);

            spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardHexBolt").Value, NPC.Center - Main.screenPosition, null, Color.Violet, NPC.rotation, new Vector2(12, 12), new Vector2(0.5f, 0.3f), SpriteEffects.None, 0f);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }
    }
}