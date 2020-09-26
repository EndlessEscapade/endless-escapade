using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace EEMod.NPCs.CoralReefs
{
    public class JellyfishPlatform1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jellyfish");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.friendly = false;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            //npc.alpha = 127;

            npc.lifeMax = 5;

            npc.width = 48;
            npc.height = 32;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;

            npc.damage = 5;
        }

        public override void AI()
        {
            Lighting.AddLight(npc.Center, 0.2f, 0.4f, 1.4f);
            npc.TargetClosest();
            Player target = Main.player[npc.target];

            npc.ai[0]++;
            if (target.Center.Y > npc.Center.Y)
            {
                if (npc.velocity.Y < 2)
                {
                    npc.velocity.Y *= 1.01f;
                }

                if (npc.velocity.Y <= 0)
                {
                    npc.velocity.Y += 0.5f;
                }
            }
            else
            {
                if (npc.ai[0] >= 120)
                {
                    npc.velocity.Y -= 4;
                    npc.velocity.X += Helpers.Clamp((target.Center.X - npc.Center.X) / 10, -2, 2);
                    npc.ai[0] = 0;
                }
                npc.velocity *= 0.97f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Main.NewText("a");
            target.velocity += Vector2.Normalize(target.Center - npc.Center) * 6;
        }
    }
}