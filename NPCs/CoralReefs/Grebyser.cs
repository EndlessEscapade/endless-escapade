using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.CoralReefs
{
    public class Grebyser : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grebyser");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 3;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 0;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 32;
            npc.height = 32;

            npc.noGravity = false;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            npc.ai[0]++;
            if (npc.ai[0] >= 600)
            {
                Projectile.NewProjectile(npc.Center + new Vector2(0, -16), Vector2.One, ProjectileID.GeyserTrap, npc.damage, 2f);
                npc.ai[0] = 0;
            }
        }
    }
}