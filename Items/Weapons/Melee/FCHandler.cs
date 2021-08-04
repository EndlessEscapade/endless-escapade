using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class FCHandler : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Chakram");
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.melee = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.alpha = 255;
            Projectile.timeLeft = 1000000000;
        }

        public int[] projectileIndex = { -1, -1, -1, -1 };

        public override void AI()
        {
            /*npc = HomeOnTarget();
            Vector2[] PositionScope = { new Vector2(dist, -dist), new Vector2(-dist, -dist), new Vector2(-dist, dist), new Vector2(dist, dist) };
            projectile.ai[0]++;
            for (int i = 0; i < Main.projectile.Length - 1; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<FeatheredChakramProjectileAlt>())
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (projectileIndex[j] == -1)
                        {
                            if (Main.projectile[i].ai[1] == 0)
                            {
                                projectileIndex[j] = i;
                                Main.projectile[i].ai[1] = 1;
                            }
                        }
                    }
                }
            }
            if (npc != null)
            {
                if (projectile.ai[0] % 300 == 0)
                {
                    WTRPosition = Helpers.FillPseudoRandomUniform(4);
                }
                for (int i = 0; i < 4; i++)
                {
                    if (projectileIndex[i] != 1)
                    {
                        Main.projectile[projectileIndex[i]].velocity *= 0.99f;
                        Main.projectile[projectileIndex[i]].velocity += (npc.Center + PositionScope[WTRPosition[i]] - Main.projectile[projectileIndex[i]].Center) / 200f - Main.projectile[projectileIndex[i]].velocity * 0.1f;
                    }
                }
            }*/
        }
    }
}