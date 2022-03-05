using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;

namespace EEMod.NPCs.ThermalVents
{
    public class Grebyser : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grebyser");
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 32;
            NPC.height = 32;

            NPC.noGravity = true;
            NPC.aiStyle = 67;

            NPC.lavaImmune = true;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        int attackTimer = 0;
        public override void FindFrame(int frameHeight)
        {
                NPC.frameCounter++;
                if (NPC.frameCounter == 5)
                {
                    NPC.frame.Y = NPC.frame.Y + frameHeight;
                    NPC.frameCounter = 0;
                }
                if (NPC.frame.Y >= frameHeight * 3)
                {
                    NPC.frame.Y = 0;
                    return;
                }
        }
        public override bool PreAI()
        {
            Player player = Main.player[NPC.target];
            attackTimer++;
            attackTimer %= 300;

            if (attackTimer > 250 && attackTimer < 275 && attackTimer % 5 == 0)
            {
                Vector2 direction = Vector2.Zero - Vector2.UnitY; 
                direction = direction.RotatedBy(NPC.rotation);
                direction = direction.RotatedBy(Main.rand.NextFloat(-0.9f, 0.9f));
                direction *= Main.rand.NextFloat(3,5);
                int proj = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), NPC.Center, direction, ModContent.ProjectileType<GrebyserFlare>(), 60, 0, NPC.target);
                if (Main.netMode != NetmodeID.Server)
                {
                    PrimitiveSystem.primitives.CreateTrail(new GrebyserPrimTrail(Main.projectile[proj]));
                }
            }
            return base.PreAI();
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.Draw(EEMod.Instance.Assets.Request<Texture2D>("NPCs/CoralReefs/GrebyserGlow").Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
    public class GrebyserFlare : EEProjectile
	{
        public override string Texture => Helpers.EmptyTexture;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grebyser Flare");
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 11;
			Projectile.aiStyle = 1;
			// Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 600;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.CrystalShard;
            Projectile.ignoreWater = true;
        }

	}
}