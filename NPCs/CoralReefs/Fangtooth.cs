using EEMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    internal class Fangtooth : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 8)
                {
                    frameNumber = 0;
                }
                NPC.frame.Y = frameNumber * 118;
            }
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 50;
            NPC.defense = 6;
            NPC.damage = 20;
            NPC.width = 94;
            NPC.height = 40;
            NPC.aiStyle = 0;
            NPC.knockBackResist = 10;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }
    }
}