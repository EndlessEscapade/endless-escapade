using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Compatibility;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class Kraken : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kraken");
            //Main.npcFrameCount[npc.type] = 9;
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
            npc.damage = 95;
            npc.value = Item.buyPrice(0, 8, 0, 0);
            npc.noTileCollide = true;
            npc.width = 250;
            npc.height = 230;
            drawOffsetY = 40;

            npc.npcSlots = 24f;
            npc.knockBackResist = 0f;

            musicPriority = MusicPriority.BossMedium;
        }
    }
}
