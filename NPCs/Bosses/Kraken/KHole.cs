using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class KHole : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hole");
        }

        public override void SetDefaults()
        {
            NPC.boss = true;
            NPC.lavaImmune = true;
            // NPC.friendly = false;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.lifeMax = 50000;
            NPC.defense = 40;
            NPC.damage = 0;
            NPC.value = Item.buyPrice(0, 8, 0, 0);
            NPC.noTileCollide = true;
            NPC.width = 186;
            NPC.height = 444;
            NPC.dontTakeDamage = true;
            NPC.npcSlots = 24f;
            NPC.knockBackResist = 0f;
            NPC.behindTiles = true;

            //NPC.Priority = MusicPriority.BossMedium;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void AI()
        {
            NPC.timeLeft = 1000;
            NPC.active = true;
            NPC.position = new Vector2(NPC.ai[0], NPC.ai[1]);
            if (NPC.ai[2] == 1)
            {
                NPC.spriteDirection = 1;
            }
        }
    }
}