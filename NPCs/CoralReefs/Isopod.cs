using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Isopod : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Isopod");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            EEMod.NoiseSurfacing.Parameters["yCoord"].SetValue(npc.ai[1]);
            EEMod.NoiseSurfacing.Parameters["t"].SetValue((0.25f - Math.Abs(0.25f - (npc.ai[0] % 0.5f))) * 4);
            EEMod.NoiseSurfacing.Parameters["xDis"].SetValue(npc.ai[0] % 0.5f);
            EEMod.NoiseSurfacing.Parameters["noiseTexture"].SetValue(TextureCache.Noise);
            EEMod.NoiseSurfacing.CurrentTechnique.Passes[0].Apply();
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //spriteBatch.End();
            //spriteBatch.Begin();
        }

        public override void AI()
        {
            npc.ai[0] += 0.002f;
            if (npc.ai[0] % 0.5f < 0.002f)
            {
                npc.ai[1] = Main.rand.NextFloat(0, 1);
            }
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 20;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 34;
            npc.height = 134;

            npc.noGravity = true;

            npc.buffImmune[BuffID.Confused] = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }
    }
}