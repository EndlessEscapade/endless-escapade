using EEMod.ID;
using EEMod.Subworlds.CoralReefs;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EEMod.NPCs.KelpForest
{
    internal class SeaDragon : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        private int frameNumber = 0;

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5)
            {
                NPC.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 5)
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
            NPC.width = 52;
            NPC.height = 118;
            NPC.aiStyle = 0;
            NPC.knockBackResist = 10;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            //NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            //NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if (NPC.wet)
            {
                if (target.WithinRange(NPC.Center, 6400))
                {
                    NPC.velocity = Vector2.Normalize(target.Center - NPC.Center) * 4;
                }
            }

            NPC.rotation = NPC.velocity.X / 6;
        }

        public override void OnKill()
        {
            if (SubworldSystem.IsActive<CoralReefs>())
            {
                EEWorld.EEWorld.instance.minionsKilled++;
            }
        }
    }
}