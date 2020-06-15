using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.Bosses.Hydros
{
    internal class HydrosMinion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }

        private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 5)
            {
                npc.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 5)
                {
                    frameNumber = 0;
                }
                npc.frame.Y = frameNumber * 118;
            }
        }


        public override void SetDefaults()
        {
            npc.lifeMax = 200;
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
            if (target.WithinRange(npc.Center, 6400))
            {
                npc.velocity = Vector2.Normalize(target.Center - npc.Center) * 4;
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