using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Compatibility;
using EEMod.NPCs.Bosses.Hydros;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class KHole : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hole");
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.lavaImmune = true;
            npc.friendly = false;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.lifeMax = 50000;
            npc.defense = 40;
            npc.damage = 0;
            npc.value = Item.buyPrice(0, 8, 0, 0);
            npc.noTileCollide = true;
            npc.width = 186;
            npc.height = 444;
            npc.dontTakeDamage = true;
            npc.npcSlots = 24f;
            npc.knockBackResist = 0f;
            npc.behindTiles = true;
            musicPriority = MusicPriority.BossMedium;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void AI()
        {
            npc.timeLeft = 1000;
            npc.active = true;
            npc.position = new Vector2(npc.ai[0], npc.ai[1]);
            if (npc.ai[2] == 1)
            {
                npc.spriteDirection = 1;
            }
        }

    }
}
