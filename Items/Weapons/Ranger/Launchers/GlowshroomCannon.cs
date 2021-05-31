using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Gores;

namespace EEMod.Items.Weapons.Ranger.Launchers
{
    public class GlowshroomCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Mortar");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 15;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 90;
            item.height = 52;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.shootSpeed = 0f;
            item.UseSound = SoundID.Item11;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.shoot = ModContent.ProjectileType<GlowshroomCannonItemProj>();
            item.ranged = true;
            item.crit = 3;
            item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.ownedProjectileCounts[item.shoot] <= 0;
        }
    }

    public class GlowshroomCannonItemProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Mortar");
        }

        public override void SetDefaults()
        {
            projectile.width = 90;
            projectile.height = 52;
            projectile.friendly = false;
            projectile.ranged = true;
            projectile.timeLeft = 26;
            projectile.hostile = false;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];

            projectile.rotation = -MathHelper.PiOver2 + (owner.direction == 1 ? (MathHelper.Pi / 8f) : (-MathHelper.Pi * 9 / 8f));

            projectile.Center = owner.Center + new Vector2(owner.direction * 12f, 0f);

            projectile.spriteDirection = owner.direction;

            if (projectile.ai[0] == 0)
            {
                Projectile.NewProjectile(owner.Center + new Vector2(owner.direction * 24, -16), new Vector2(projectile.spriteDirection * 3, -12f) + owner.velocity, ModContent.ProjectileType<GlowshroomCannonProj>(), projectile.damage, projectile.knockBack);
                projectile.ai[0]++;
            }
        }
    }

    public class GlowshroomCannonProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroomball");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.ranged = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();

            projectile.velocity.Y += 0.25f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 4; i++)
            {
                Gore gore = Gore.NewGorePerfect(projectile.Center, Vector2.Zero, mod.GetGoreSlot("Gores/GlowshroomCannonGore" + (i + 1)), 1);
                gore.velocity = new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
            }
            return true;
        }
    }
}