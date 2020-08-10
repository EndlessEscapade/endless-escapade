using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Enemy;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.NPCs.CoralReefs
{
    public class MechanicalShark : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Shark");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 6)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 174;
            npc.height = 98;

            npc.noGravity = true;

            npc.spriteDirection = -1;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        Vector2 oldPlayerPos = new Vector2();
        public override void AI()
        {
            npc.TargetClosest();
            if (npc.ai[0] == 0)
            {
                npc.velocity = Vector2.Normalize(Main.player[npc.target].position - npc.Center) * 6;
            }
            if (npc.ai[0] == 1)
            {
                npc.ai[2]++;
                if (npc.ai[2] >= 60)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<MechanicalMissile>(), 120, 5f);
                    npc.ai[2] = 0;
                }
                npc.velocity = Vector2.Normalize(Main.player[npc.target].position - npc.Center) * 2;
            }
            if (npc.ai[0] == 2)
            {
                if (npc.ai[1] % 120 == 0)
                {
                    oldPlayerPos = Main.player[npc.target].position;
                    oldPlayerPos = Vector2.Normalize(oldPlayerPos - npc.Center) * 12f;
                }
                npc.velocity = oldPlayerPos;
            }

            npc.rotation = npc.velocity.ToRotation();
            npc.spriteDirection = npc.direction;

            npc.ai[1]++;
            if (npc.ai[1] >= 360)
            {
                npc.ai[0] = Main.rand.Next(0, 3);
                npc.ai[1] = 0;
                npc.ai[2] = 0;
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/CoralReefs/MechanicalSharkGlow"), npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.ai[0] == 2)
            {
                AfterImage.DrawAfterimage(spriteBatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            return true;
        }
    }
}