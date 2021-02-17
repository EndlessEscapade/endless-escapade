using EEMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    internal class HermitCrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 4)
            {
                npc.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 4)
                {
                    frameNumber = 0;
                }
                npc.frame.Y = frameNumber * 32;
            }
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 50;
            npc.defense = 6;
            npc.damage = 20;
            npc.width = 52;
            npc.height = 118;
            npc.aiStyle = 0;
            npc.knockBackResist = 10;
            npc.value = Item.buyPrice(0, 0, 5, 0);
            npc.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }

        public override void AI()
        {
            Player target = Main.player[npc.target];
            if (npc.wet)
            {
                if (target.WithinRange(npc.Center, 6400))
                {
                    npc.velocity = Vector2.Normalize(target.Center - npc.Center) * 4;
                }
            }

            npc.rotation = npc.velocity.X / 6;
        }

        public override void NPCLoot()
        {
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                EEWorld.EEWorld.instance.minionsKilled++;
            }
        }
    }
}