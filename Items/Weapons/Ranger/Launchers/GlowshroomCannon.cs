using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Gores;

namespace EEMod.Items.Weapons.Ranger.Launchers
{
    public class GlowshroomCannon : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Mortar");
        }

        public override void SetDefaults()
        {
            Item.melee = false;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 15;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.width = 90;
            Item.height = 52;
            Item.rare = ItemRarityID.Green;
            Item.knockBack = 5f;
            Item.shootSpeed = 0f;
            Item.UseSound = SoundID.Item11;
            Item.useStyle = ItemUseStyleID.Stabbing;
            Item.shoot = ModContent.ProjectileType<GlowshroomCannonItemProj>();
            Item.ranged = true;
            Item.crit = 3;
            Item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.ownedProjectileCounts[Item.shoot] <= 0;
        }
    }

    public class GlowshroomCannonItemProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Mortar");
        }

        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 52;
            Projectile.friendly = false;
            Projectile.ranged = true;
            Projectile.timeLeft = 26;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            Projectile.rotation = -MathHelper.PiOver2 + (owner.direction == 1 ? (MathHelper.Pi / 8f) : (-MathHelper.Pi * 9 / 8f));

            Projectile.Center = owner.Center + new Vector2(owner.direction * 12f, 0f);

            Projectile.spriteDirection = owner.direction;

            if (Projectile.ai[0] == 0)
            {
                Projectile.NewProjectile(owner.Center + new Vector2(owner.direction * 24, -16), new Vector2(Projectile.spriteDirection * 3, -12f) + owner.velocity, ModContent.ProjectileType<GlowshroomCannonProj>(), Projectile.damage, Projectile.knockBack);
                Projectile.ai[0]++;
            }
        }
    }

    public class GlowshroomCannonProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.ranged = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.velocity.Y += 0.25f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 4; i++)
            {
                Gore gore = Gore.NewGorePerfect(Projectile.Center, Vector2.Zero, mod.GetGoreSlot("Gores/GlowshroomCannonGore" + (i + 1)), 1);
                gore.velocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
            }
            return true;
        }
    }
}