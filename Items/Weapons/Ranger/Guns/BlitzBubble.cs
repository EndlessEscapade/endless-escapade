using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria.ID;
using EEMod.Prim;
using Terraria.Audio;

namespace EEMod.Items.Weapons.Ranger.Guns
{
    public class BlitzBubble : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 32;
            Projectile.height = 32;
            // Projectile.friendly = false;
            // Projectile.tileCollide = false;
            // Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.alpha = 110;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 200)
            {
                Projectile.scale = Main.rand.NextFloat(0.95f, 1.35f);
            }
            Projectile.velocity.X *= 0.99f;
            Projectile.velocity.Y -= 0.015f;
            var list = Helpers.ProjectileForeach.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
            foreach (var proj in list)
            {
                if (proj.DamageType == DamageClass.Ranged && proj.active && proj.friendly && !proj.hostile && (proj.width <= 6 || proj.height <= 6))
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake = 6;
                    Projectile.timeLeft = 1;
                    CombatText.NewText(new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height), new Color(255, 155, 0, 100),
                    "Blitz!");
                    for (int i = 0; i < 20; i++)
                    {
                        int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .4f;
                        if (Main.dust[num].position != Projectile.Center)
                            Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 8f;
                    }
                    if (Main.netMode != NetmodeID.Server)
                    {
                        EEMod.primitives.CreateTrail(new BubbleBlitzerPrimTrail(proj));
                    }
                    proj.damage = (int)(proj.damage * 1.71f);
                }
            }

            Projectile.rotation = Projectile.velocity.X / 32;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 54);
            for (int i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .4f;
                if (Main.dust[num].position != Projectile.Center)
                    Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 3f;
            }
        }
    }
}