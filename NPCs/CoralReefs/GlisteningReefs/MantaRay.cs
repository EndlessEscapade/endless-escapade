using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.GlisteningReefs
{
    internal class MantaRay : ModNPC
    {
        const int ChainLength = 20;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 1;
            DisplayName.SetDefault("Manta Ray");
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 112;
            npc.height = 66;

            npc.noGravity = true;

            npc.buffImmune[BuffID.Confused] = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }

        Vector2[] ChainPositions = new Vector2[ChainLength];

        Texture2D tex1 => mod.GetTexture("NPCs/CoralReefs/GlisteningReefs/MantaRayC1");
        Texture2D tex2 => mod.GetTexture("NPCs/CoralReefs/GlisteningReefs/MantaRayC2");
        Texture2D tex3 => mod.GetTexture("NPCs/CoralReefs/GlisteningReefs/MantaRayC3");

        float FolowFactor = 0.02f;
        public override void AI()
        {
            float speed = 3;
            int Length = 108;
            int Interval = 500;

            int startingFrame = Interval - Length;
            float timeSince = (npc.ai[0] % Interval) - startingFrame;

            if (npc.ai[0] == 0)
            {
                for (int i = 0; i < ChainLength; i++)
                {
                    ChainPositions[i] = npc.Center;
                }
            }

            npc.ai[0]++;

            if (timeSince > 0)
            {
                npc.ai[1] = 1;
            }
            else npc.ai[1] = 0;

            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Vector2 dist = player.Center - npc.Center;
            float velLength = npc.velocity.Length();

            bool spriterot = npc.Center.X < player.Center.X;
            int dir = spriterot ? 1 : -1;

            if (npc.ai[1] == 0)
                npc.velocity += (Vector2.Normalize(dist) * speed) / 40f - npc.velocity / 50f;
            else
            {
                npc.velocity *= 0.995f;
            }

            npc.rotation = npc.velocity.ToRotation() + (float)Math.PI;

            FolowFactor += (0.02f - FolowFactor) / 16f;

            if (npc.ai[1] == 1) FolowFactor += (0.01f - FolowFactor) / 16f;

            Vector2 basePos = npc.Center + new Vector2(45, (float)Math.Sin(npc.ai[0] / 30f) * 2).RotatedBy(npc.rotation);

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
                    Vector2 to = ChainPositions[i - 1] + new Vector2(0, (float)Math.Sin((npc.ai[0] / 30f) + i / 3f) * (i / 2f)).RotatedBy(npc.rotation);
                    Vector2 diff = (to - ChainPositions[i]);

                    if (npc.ai[1] == 1)
                    {
                        float hold = 50;
                        if (timeSince > hold)
                        {
                            float timeIn = timeSince - hold;
                            float lerp = timeIn / 100f;
                            float smoothStep = MathHelper.SmoothStep(-1.8f, 1.8f, lerp * 2.5f);
                            ChainPositions[i] += ((npc.Center + v) - ChainPositions[i]) / 11f;
                            v += new Vector2(11 * dir, 0).RotatedBy(i / 7f * smoothStep * dir);
                        }
                        else
                        {
                            float lerp = timeSince / hold;
                            float smoothStep = MathHelper.SmoothStep(0, -1f, lerp);
                            ChainPositions[i] += ((npc.Center + v) - ChainPositions[i]) / 17f;
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

            bool spriterot = npc.rotation > Math.PI / 2 && npc.rotation < 3 * (Math.PI / 2);

            Texture2D tex = Main.npcTexture[npc.type];

            spriteBatch.Draw(tex, npc.Center.ForDraw(), tex.Bounds, drawColor, npc.rotation, tex.TextureCenter(), npc.scale, spriterot ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);

            return false;
        }
    }
}