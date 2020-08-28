using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace EEMod
{
    public class NoiseSurfacingTest : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glow");
        }

        public override void AI()
        {

        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 20;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 144;
            npc.height = 160;

            npc.noGravity = true;

            npc.buffImmune[BuffID.Confused] = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }
    }
}
