using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.EEWorld;

namespace EEMod.NPCs.CoralReefs
{
    internal class Nautilus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 5)
            {
                npc.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 4)
                {
                    frameNumber = 0;
                }
                npc.frame.Y = frameNumber * 84;
            }
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 50;
            npc.defense = 6;
            npc.damage = 20;
            npc.width = 108;
            npc.height = 84;
            npc.aiStyle = 0;
            npc.knockBackResist = 10;
            npc.value = Item.buyPrice(0, 0, 5, 0);
            npc.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }

        public override void AI()
        {
            Player target = Main.player[npc.target];
            if (target.WithinRange(npc.Center, 6400))
            {
                if (npc.ai[1] < 4) { npc.ai[1] *= 1.05f; }
            }
            else
            {
                npc.ai[1] *= 0.95f;
            }
            npc.velocity = Vector2.Normalize(target.Center - npc.Center) * npc.ai[1];
            npc.rotation = npc.velocity.ToRotation();
        }
    }
}