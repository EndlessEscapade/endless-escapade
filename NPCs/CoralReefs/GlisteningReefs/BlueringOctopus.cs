using EEMod.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.GlisteningReefs
{
    public class BlueringOctopus : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bluering Octopus");
            //.npcFrameCount[npc.type] = 6;
        }

        /*public override void FindFrame(int frameHeight)
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
        }*/

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 174;
            NPC.height = 98;

            NPC.noGravity = true;

            NPC.spriteDirection = -1;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];
            NPC.ai[1]++;
            if (NPC.ai[1] >= 60)
            {
                NPC.ai[2]++;
                if (NPC.ai[2] >= 2)
                {
                    Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), NPC.Center, Vector2.Normalize(target.Center - NPC.Center) * 6, ModContent.ProjectileType<BlueRing>(), NPC.damage, 0f);
                    NPC.ai[2] = 0;
                }

                if (target.position.Y <= NPC.position.Y)
                {
                    NPC.velocity.Y -= 16;
                    NPC.velocity.X += Helpers.Clamp((target.Center.X - NPC.Center.X) / 10, -8, 8);
                }

                NPC.ai[1] = 0;
            }
            NPC.velocity *= 0.98f;
            NPC.velocity.Y += 0.1f;

            NPC.rotation = NPC.velocity.X / 18;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/CoralReefs/GlisteningReefs/BlueringOctopusGlow").Value, NPC.Center - Main.screenPosition + new Vector2(0, -6), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.ai[0] == 2)
            {
                AfterImage.DrawAfterimage(spriteBatch, Main.npcTexture[npc.type], 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            return true;
        }*/
    }
}