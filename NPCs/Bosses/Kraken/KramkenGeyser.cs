using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken
{
    public class KramkenGeyser : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Geyser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.damage = 20;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 180;
            // Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 70)
            {
                for (int i = 0; i < 1; i++)
                {
                    int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt, 0, 0, 6, default, Projectile.scale * 1.3f);
                    // Main.dust[num].noGravity = false;
                    Main.dust[num].velocity *= 1.5f;
                    // Main.dust[num].noLight = false;
                }
            }
            else
            {
                if (Projectile.ai[1] == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, 0, Main.rand.NextFloat(-5, -2), 6, Color.Blue, Projectile.scale * 1.5f);
                        // Main.dust[num].noGravity = false;
                        Main.dust[num].velocity *= 7f;
                        Main.dust[num].velocity.X = Main.rand.NextFloat(-2, 2);
                        // Main.dust[num].noLight = false;
                    }
                }
                if (Projectile.ai[1] == 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, 0, Main.rand.NextFloat(2, 5), 6, Color.Blue, Projectile.scale * 1.5f);
                        // Main.dust[num].noGravity = false;
                        Main.dust[num].velocity *= 7f;
                        Main.dust[num].velocity.X = Main.rand.NextFloat(-2, 2);
                        // Main.dust[num].noLight = false;
                    }
                }
                if (Projectile.ai[1] == 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, Main.rand.NextFloat(2, 5), 0, 6, Color.Blue, Projectile.scale * 1.5f);
                        // Main.dust[num].noGravity = false;
                        Main.dust[num].velocity *= 7f;
                        Main.dust[num].velocity.Y = Main.rand.NextFloat(-2, 2);
                        // Main.dust[num].noLight = false;
                    }
                }
                if (Projectile.ai[1] == 3)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, Main.rand.NextFloat(-5, -2), 0, 6, Color.Blue, Projectile.scale * 1.5f);
                        // Main.dust[num].noGravity = false;
                        Main.dust[num].velocity *= 7f;
                        Main.dust[num].velocity.Y = Main.rand.NextFloat(-2, 2);
                        // Main.dust[num].noLight = false;
                    }
                }
                Projectile.netUpdate = true;
            }
        }
    }
}