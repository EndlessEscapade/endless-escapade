using EEMod.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs.MechanicalReefs
{
    public class MechanicalShark : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Shark");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter == 6)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 6)
            {
                NPC.frame.Y = 0;
                return;
            }
        }

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

            NPC.lavaImmune = false;
            NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        private Vector2 oldPlayerPos = new Vector2();

        public override void AI()
        {
            NPC.TargetClosest();
            if (NPC.ai[0] == 0)
            {
                NPC.velocity = Vector2.Normalize(Main.player[NPC.target].position - NPC.Center) * 6;
            }
            if (NPC.ai[0] == 1)
            {
                NPC.ai[2]++;
                if (NPC.ai[2] >= 60)
                {
                    Projectile.NewProjectile(NPC.Center, Vector2.Zero, ModContent.ProjectileType<MechanicalMissile>(), 120, 5f);
                    NPC.ai[2] = 0;
                }
                NPC.velocity = Vector2.Normalize(Main.player[NPC.target].position - NPC.Center) * 2;
            }
            if (NPC.ai[0] == 2)
            {
                if (NPC.ai[1] % 120 == 0)
                {
                    oldPlayerPos = Main.player[NPC.target].position;
                    oldPlayerPos = Vector2.Normalize(oldPlayerPos - NPC.Center) * 12f;
                }
                NPC.velocity = oldPlayerPos;
            }

            float k = 0;
            if (Main.player[NPC.target].position.X <= NPC.position.X)
            {
                k = MathHelper.Pi;
            }
            NPC.rotation = NPC.velocity.ToRotation() + k;
            NPC.spriteDirection = NPC.direction;

            NPC.ai[1]++;
            if (NPC.ai[1] >= 360)
            {
                NPC.ai[0] = Main.rand.Next(0, 3);
                NPC.ai[1] = 0;
                NPC.ai[2] = 0;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/MechanicalReefs/MechanicalSharkGlow"), NPC.Center - Main.screenPosition + new Vector2(0, 4), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (NPC.ai[0] == 2)
            {
                AfterImage.DrawAfterimage(spriteBatch, Main.npcTexture[NPC.type], 0, NPC, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            return true;
        }
    }
}