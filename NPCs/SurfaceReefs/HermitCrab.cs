using EEMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EEMod.NPCs.SurfaceReefs
{
    public class HermitCrab : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 4)
            {
                NPC.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 4)
                {
                    frameNumber = 0;
                }
                NPC.frame.Y = frameNumber * 32;
            }
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 50;
            NPC.defense = 6;
            NPC.damage = 20;
            NPC.width = 42;
            NPC.height = 32;
            NPC.aiStyle = 3;
            NPC.knockBackResist = 10;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }
    }
}