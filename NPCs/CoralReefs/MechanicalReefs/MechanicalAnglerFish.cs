using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using EEMod.Projectiles.Enemy;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class MechanicalAnglerFish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Angler Fish");
            Main.npcFrameCount[npc.type] = 3;
        }

        private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameNumber * 48;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

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

        public override void AI()
        {
            if (npc.ai[3] == 0)
            {
                npc.ai[3] = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<MechanicalLure>(), npc.damage, 0f);
            }
            else
            {
                npc.velocity = Vector2.Zero;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color DrawColor)
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            Texture2D LureChain = mod.GetTexture("Projectiles/Enemy/MechanicalLureChain");
            float distance = Vector2.Distance(npc.Center, Main.projectile[(int)npc.ai[3]].position) / LureChain.Height;
            for (int i = 0; i < distance; i++)
            {
                Main.spriteBatch.Draw(LureChain, npc.Center - Main.screenPosition + new Vector2((npc.width / 2) - (LureChain.Width / 2), (npc.height / 2) + (i * LureChain.Height)), Color.White);
            }
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, 8), null, DrawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}