using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class SpiritPistolProjectileMain : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 12;       //projectile width
            Projectile.height = 12;  //projectile height
            Projectile.friendly = true;      //make that the projectile will not damage you
            Projectile.DamageType = DamageClass.Magic;     //
            Projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            Projectile.penetrate = -1;      //how many npc will penetrate
                                            //how many time this projectile has before disepire
            Projectile.light = 0.3f;    // projectile light
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
        }

        private bool firstFrame = true;
        private readonly int[] linkedProj = new int[6];

        public override void AI()
        {
            if (firstFrame)
            {
                for (int i = 0; i < 6; i++)
                {
                    int proj = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(Projectile), Projectile.position, Vector2.Zero, ModContent.ProjectileType<SpiritPistolProjectileSecondary>(), Projectile.damage, Projectile.knockBack, Owner: Projectile.owner, ai0: i * (MathHelper.TwoPi / 6), ai1: Projectile.whoAmI);
                    linkedProj[i] = proj;
                }
                firstFrame = false;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            foreach (int proj in linkedProj)
            {
                Main.projectile[proj].Kill();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.velocity = Vector2.Zero;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}