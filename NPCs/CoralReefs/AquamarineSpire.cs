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
using EEMod.Tiles.Foliage.Coral;
using EEMod.Projectiles.Enemy;
using EEMod.Prim;

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

        public bool awake = true;
        float HeartBeat;
        int blinkTime = 0;
        bool blinking = false;

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player target = Main.LocalPlayer;

            float timeBetween;
            float bigTimeBetween;
            if (awake)
            {
                Vector2 eyePos = (Vector2.Normalize(target.Center - npc.Center) * 3) + npc.Center + new Vector2(-2, 2 + blinkTime);

                if (!blinking && Main.rand.NextBool(180) && blinkTime <= 0)
                    blinking = true;
                if (!blinking && blinkTime > 0)
                    blinkTime--;
                if (blinkTime == 8)
                    blinking = false;
                if (blinking && blinkTime < 8)
                    blinkTime++;

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), Color.White, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 0);

                timeBetween = 70;
                bigTimeBetween = 200;
            }
            else
            {
                timeBetween = 35;
                bigTimeBetween = 100;
            }

            if (Main.GameUpdateCount % 200 < timeBetween)
            {
                HeartBeat = Math.Abs((float)Math.Sin((Main.GameUpdateCount % bigTimeBetween) * (6.28f / timeBetween))) * (1 - (Main.GameUpdateCount % bigTimeBetween) / (timeBetween * 1.5f));
            }
            else
            {
                HeartBeat = 0;
            }
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireGlow"), npc.Center.ForDraw() + new Vector2(0, 4), npc.frame, Color.White * HeartBeat, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public override void AI()
        {
            if (!awake)
            {
                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.18f));
                Vector2 one = new Vector2(-8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 two = new Vector2(8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 three = new Vector2(Main.rand.Next(-8, 8), 8).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 four = new Vector2(Main.rand.Next(-8, 8), -8).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f) + 2 + HeartBeat / 60f);
                int scale = 4;
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + one * scale + offset, -Vector2.Normalize(one) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + two * scale + offset, -Vector2.Normalize(two) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + three * scale + offset, -Vector2.Normalize(three) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + four * scale + offset, -Vector2.Normalize(four) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
            }
            else
            {
                Player target = Main.LocalPlayer;

                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.25f));
                Vector2 one = new Vector2(-8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 two = new Vector2(8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 three = new Vector2(Main.rand.Next(-8, 8), 8).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 four = new Vector2(Main.rand.Next(-8, 8), -8).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f) + 2 + HeartBeat / 60f);
                int scale = 4;
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + one * scale + offset, -Vector2.Normalize(one) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + two * scale + offset, -Vector2.Normalize(two) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + three * scale + offset, -Vector2.Normalize(three) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + four * scale + offset, -Vector2.Normalize(four) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));

                npc.ai[0]++;

                if(npc.ai[0] % 60 == 0)
                {
                    Projectile projectile = Projectile.NewProjectileDirect(npc.Center, Vector2.Normalize(target.Center - npc.Center) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 2, 0f);
                    EEMod.primitives.CreateTrail(new SceptorPrimTrail(projectile));
                }
            }
        }
    }
}