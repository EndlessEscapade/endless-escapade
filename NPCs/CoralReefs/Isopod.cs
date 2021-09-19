using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Isopod : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Isopod");
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //spriteBatch.End();
            //spriteBatch.Begin();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 20;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 34;
            NPC.height = 134;

            NPC.noGravity = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }


        public override void AI()
        {
            NPC.ai[1]++;

            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            if (NPC.ai[1] % 90 == 0)
            {
                NPC.velocity += Vector2.Normalize(target.Center - NPC.Center) * 12;
            }
            NPC.velocity *= 0.98f;

            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}