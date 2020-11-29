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
            EEMod.White.CurrentTechnique.Passes[0].Apply();
            EEMod.White.Parameters["alpha"].SetValue(((float)Math.Sin(alpha) + 1) * 0.5f);
           // Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center.ForDraw(), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale * 1.01f, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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
            if(Main.GameUpdateCount % 60 < 20)
            {
                HeartBeat = (float)Math.Sin((Main.GameUpdateCount % 60) * (6.14f/20f));
            }
            else
            {
                HeartBeat = 0;
            }
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/SpireEye"), npc.Center.ForDraw() + new Vector2(0,5), npc.frame, Color.White * (float)Math.Sin(Main.GameUpdateCount / 60f), npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
        public override void AI()
        {
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.18f));
            Vector2 one = new Vector2(-10, Main.rand.Next(-10, 10)).RotatedBy(1.57f / 2f);
            Vector2 two = new Vector2(10, Main.rand.Next(-10, 10)).RotatedBy(1.57f / 2f);
            Vector2 three = new Vector2(Main.rand.Next(-10, 10), 10).RotatedBy(1.57f / 2f);
            Vector2 four = new Vector2(Main.rand.Next(-10, 10), -10).RotatedBy(1.57f / 2f);
            Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f));
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + one*2 + offset, -Vector2.Normalize(one)/2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1,Color.White, new SlowDown(0.95f));
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + two * 2 + offset, -Vector2.Normalize(two) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f));
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + three * 2 + offset, -Vector2.Normalize(three) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f)); 
            EEMod.Particles.Get("Main").SpawnParticles(npc.Center + four * 2 + offset, -Vector2.Normalize(four) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f));
        }
    }
}