using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.GlisteningReefs
{
    internal class MantaRay : EENPC
    {
        const int ChainLength = 20;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            DisplayName.SetDefault("Manta Ray");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 112;
            NPC.height = 66;

            NPC.noGravity = true;

            NPC.buffImmune[BuffID.Confused] = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
        }

        Vector2[] ChainPositions = new Vector2[ChainLength];

        Texture2D tex1 => Mod.Assets.Request<Texture2D>("NPCs/CoralReefs/GlisteningReefs/MantaRayC1").Value;
        Texture2D tex2 => Mod.Assets.Request<Texture2D>("NPCs/CoralReefs/GlisteningReefs/MantaRayC2").Value;
        Texture2D tex3 => Mod.Assets.Request<Texture2D>("NPCs/CoralReefs/GlisteningReefs/MantaRayC3").Value;

        float FolowFactor = 0.02f;
        public override void AI()
        {
            float speed = 3;
            int Length = 108;
            int Interval = 500;

            int startingFrame = Interval - Length;
            float timeSince = (NPC.ai[0] % Interval) - startingFrame;

            if (NPC.ai[0] == 0)
            {
                for (int i = 0; i < ChainLength; i++)
                {
                    ChainPositions[i] = NPC.Center;
                }
            }

            NPC.ai[0]++;

            if (timeSince > 0)
            {
                NPC.ai[1] = 1;
            }
            else NPC.ai[1] = 0;

            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            Vector2 dist = player.Center - NPC.Center;
            float velLength = NPC.velocity.Length();

            bool spriterot = NPC.Center.X < player.Center.X;
            int dir = spriterot ? 1 : -1;

            if (NPC.ai[1] == 0)
                NPC.velocity += (Vector2.Normalize(dist) * speed) / 40f - NPC.velocity / 50f;
            else
            {
                NPC.velocity *= 0.995f;
            }

            NPC.rotation = NPC.velocity.ToRotation() + (float)Math.PI;

            FolowFactor += (0.02f - FolowFactor) / 16f;

            if (NPC.ai[1] == 1) FolowFactor += (0.01f - FolowFactor) / 16f;

            Vector2 basePos = NPC.Center + new Vector2(45, (float)Math.Sin(NPC.ai[0] / 30f) * 2).RotatedBy(NPC.rotation);

            Vector2 v = Vector2.Zero;

            for (int i = 0; i < ChainLength; i++)
            {
                if (i == 0)
                {
                    Vector2 diff = (basePos - ChainPositions[i]);
                    Vector2 add = diff * FolowFactor * (diff.Length() + velLength);
                    if (add.Length() > 10)
                    {
                        add = Vector2.Normalize(add) * 10;
                    }
                    ChainPositions[i] += add;
                }
                else
                {
                    Vector2 to = ChainPositions[i - 1] + new Vector2(0, (float)Math.Sin((NPC.ai[0] / 30f) + i / 3f) * (i / 2f)).RotatedBy(NPC.rotation);
                    Vector2 diff = (to - ChainPositions[i]);

                    if (NPC.ai[1] == 1)
                    {
                        float hold = 50;
                        if (timeSince > hold)
                        {
                            float timeIn = timeSince - hold;
                            float lerp = timeIn / 100f;
                            float smoothStep = MathHelper.SmoothStep(-1.8f, 1.8f, lerp * 2.5f);
                            ChainPositions[i] += ((NPC.Center + v) - ChainPositions[i]) / 11f;
                            v += new Vector2(11 * dir, 0).RotatedBy(i / 7f * smoothStep * dir);
                        }
                        else
                        {
                            float lerp = timeSince / hold;
                            float smoothStep = MathHelper.SmoothStep(0, -1f, lerp);
                            ChainPositions[i] += ((NPC.Center + v) - ChainPositions[i]) / 17f;
                            v += new Vector2(7 * dir, 0).RotatedBy((ChainLength - i) / 8f * smoothStep * dir);
                        }
                    }
                    else
                    {
                        Vector2 add = diff * FolowFactor * (diff.Length() + velLength);
                        if (add.Length() > 10)
                        {
                            add = Vector2.Normalize(add) * 10;
                        }
                        ChainPositions[i] += add;
                    }
                }
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            for (int i = 0; i < ChainLength; i++)
            {
                Texture2D chosen = tex1;
                float rotation = 0;
                if (i > 0)
                {
                    rotation = (ChainPositions[i] - ChainPositions[i - 1]).ToRotation();
                    chosen = tex2;
                }

                if (i == ChainLength - 1) chosen = tex3;

                spriteBatch.Draw(chosen, ChainPositions[i].ForDraw(), chosen.Bounds, drawColor, rotation, chosen.TextureCenter(), 1f, SpriteEffects.None, 0f);
            }

            bool spriterot = NPC.rotation > Math.PI / 2 && NPC.rotation < 3 * (Math.PI / 2);

            Texture2D tex = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;

            spriteBatch.Draw(tex, NPC.Center.ForDraw(), tex.Bounds, drawColor, NPC.rotation, tex.TextureCenter(), NPC.scale, spriterot ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            return false;
        }
    }
}