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
using EEMod.Items.Placeables.Ores;

namespace EEMod.NPCs.Bosses.SailorsOmen
{
    public class SailorsOmen : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor's Omen");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.alpha = 0;
            npc.lifeMax = 1000000;
            npc.width = 40;
            npc.height = 40;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.damage = 0;
            npc.knockBackResist = 0f;
        }

        public RenderTarget2D Target;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if(Target == default) Target = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);

            RenderTarget2D EffervescenceRT = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            Main.instance.GraphicsDevice.SetRenderTarget(EffervescenceRT);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center.ForDraw(), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale * 1.05f, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center.ForDraw() + new Vector2(0, -30), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale * 1.05f, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Main.instance.GraphicsDevice.SetRenderTarget(Target);
            Main.instance.GraphicsDevice.Clear(Color.Transparent);

            EEMod.Effervescence.CurrentTechnique.Passes[0].Apply();
            EEMod.Effervescence.Parameters["width"].SetValue(Main.npcTexture[npc.type].Width);
            EEMod.Effervescence.Parameters["height"].SetValue(Main.npcTexture[npc.type].Height);

            Vector3 color = Color.Red.ToVector3() / 255f;
            EEMod.Effervescence.Parameters["border"].SetValue(new Vector4(color.X, color.Y, color.Z, 1f));

            Main.spriteBatch.Draw(EffervescenceRT, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}