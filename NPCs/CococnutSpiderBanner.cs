using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.NPCs
{
    public class CoconutCrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut Crab");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.alpha = 0;
            npc.aiStyle = 3;
            npc.lifeMax = Main.expertMode ? 90 : 90;    //this is the npc health
            npc.damage = Main.expertMode ? 15 : 25;  //this is the npc damage
            npc.defense = 6;         //this is the npc defense
            npc.knockBackResist = 1.5f;
            npc.width = 28; //this is where you put the npc sprite width.     important
            npc.height = 47; //this is where you put the npc sprite height.   important
            npc.boss = false;
            npc.lavaImmune = true;       //this make the npc immune to lava
            npc.noGravity = false;           //this make the npc float
            npc.noTileCollide = false;        //this make the npc go thru walls
            npc.HitSound = SoundID.NPCHit23;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.behindTiles = false;

            npc.value = Item.buyPrice(0, 0, 5, 0);
            npc.npcSlots = 1f;
            npc.netAlways = true;
        }
    }
}
