using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Hydros
{
    public class Geyser : EEProjectile
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
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 90)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt, 0, 0, 6, default, Projectile.scale);
                Main.dust[num].noGravity = false;
                Main.dust[num].velocity *= 1.5f;
                Main.dust[num].noLight = false;
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, 0, Main.rand.NextFloat(-2, -5), 6, Color.Blue, Projectile.scale * 1.1f);
                    Main.dust[num].noGravity = false;
                    Main.dust[num].velocity *= 1f;
                    Main.dust[num].noLight = false;
                }
            }
        }
    }
}