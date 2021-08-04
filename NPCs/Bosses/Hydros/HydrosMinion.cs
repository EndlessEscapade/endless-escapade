using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Hydros
{
    internal class HydrosMinion : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
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
            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
        }

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            if (target.WithinRange(NPC.Center, 6400))
            {
                NPC.velocity = Vector2.Normalize(target.Center - NPC.Center) * 4;
            }
        }

        /*public override bool CheckDead()
        {
            int goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), default(Vector2), mod.GetGoreSlot("Gores/HydrosMinionGore"), 1f);
            Main.gore[goreIndex].scale = 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

            return true;
        }*/
    }
}