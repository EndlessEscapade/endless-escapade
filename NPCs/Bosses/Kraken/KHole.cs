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
            npc.width = 562;
            npc.height = 472;
            npc.dontTakeDamage = true;
            npc.npcSlots = 24f;
            npc.knockBackResist = 0f;

            musicPriority = MusicPriority.BossMedium;
        }
        Vector2 topLeft = new Vector2(6000, 18300);
        Vector2 topRight = new Vector2(10000, 18300);

        public override void AI()
        {
          npc.position = new Vector2(npc.ai[0],npc.ai[1]);
        }

    }
}
