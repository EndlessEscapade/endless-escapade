using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;

namespace EEMod.NPCs.CoralReefs
{
    public class Grebyser : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grebyser");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 32;
            npc.height = 32;

            npc.noGravity = true;
            npc.aiStyle = 67;

            npc.lavaImmune = true;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        int attackTimer = 0;
        public override void FindFrame(int frameHeight)
        {
                npc.frameCounter++;
                if (npc.frameCounter == 5)
                {
                    npc.frame.Y = npc.frame.Y + frameHeight;
                    npc.frameCounter = 0;
                }
                if (npc.frame.Y >= frameHeight * 3)
                {
                    npc.frame.Y = 0;
                    return;
                }
        }
        public override bool PreAI()
        {
            Player player = Main.player[npc.target];
            attackTimer++;
            attackTimer %= 300;

            if (attackTimer > 250 && attackTimer < 275 && attackTimer % 5 == 0)
            {
                Vector2 direction = Vector2.Zero - Vector2.UnitY; 
                direction = direction.RotatedBy(npc.rotation);
                direction = direction.RotatedBy(Main.rand.NextFloat(-0.9f, 0.9f));
                direction *= Main.rand.NextFloat(3,5);
                int proj = Projectile.NewProjectile(npc.Center, direction, ModContent.ProjectileType<GrebyserFlare>(), 60, 0, npc.target);
                if (Main.netMode != NetmodeID.Server)
                {
                    EEMod.primitives.CreateTrail(new GrebyserPrimTrail(Main.projectile[proj]));
                }
            }
            return base.PreAI();
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.Draw(EEMod.instance.GetTexture("NPCs/CoralReefs/GrebyserGlow"), npc.Center - Main.screenPosition + new Vector2(0, 4), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
    public class GrebyserFlare : ModProjectile
	{
        public override string Texture => Helpers.EmptyTexture;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grebyser Flare");
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 11;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 600;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.CrystalShard;
            projectile.ignoreWater = true;
        }

	}
}