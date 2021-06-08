using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EEMod.Items.Weapons.Ranger.Longbows.HuntressBow
{
    public class HuntressBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Huntress' Recurve Bow");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.useAnimation = 18;
            item.useTime = 6;
            item.reuseDelay = 35;

            item.shootSpeed = 16f;
            item.knockBack = 6.5f;
            item.width = 30;
            item.height = 58;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.noMelee = false;
            item.autoReuse = true;
            item.useAmmo = AmmoID.Arrow;

            item.ranged = true;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<ShimmerShotProj>();
        }

        public override bool AltFunctionUse(Player player)
        {
            if (arrowShots >= 15) return false;
            else return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.noUseGraphic = true;
                item.useStyle = ItemUseStyleID.SwingThrow;

                item.useTime = 10;
                item.useAnimation = 10;
                item.reuseDelay = 60;

                item.damage = 50;

                item.autoReuse = false;
            }
            else
            {
                if (arrowShots < 15)
                {
                    item.noUseGraphic = false;
                    item.useStyle = ItemUseStyleID.HoldingOut;

                    item.useAnimation = 18;
                    item.useTime = 6;
                    item.reuseDelay = 35;

                    item.damage = 25;

                    item.autoReuse = true;
                }
                else
                {
                    item.noUseGraphic = false;
                    item.useStyle = ItemUseStyleID.HoldingOut;

                    item.useAnimation = 30;
                    item.useTime = 30;
                    item.reuseDelay = 30;

                    item.damage = 60;

                    item.autoReuse = false;
                }
            }

            return base.CanUseItem(player);
        }

        public int arrowShots;
        public int ballistaShots;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 0)
            {
                if (arrowShots < 15)
                {
                    Projectile cloudSprite = Projectile.NewProjectileDirect(player.Center, Vector2.Normalize(player.Center - Main.MouseWorld) * -4, ModContent.ProjectileType<HuntressArrow>(), item.damage, item.knockBack, default);
                    cloudSprite.ai[1] = player.GetModPlayer<HuntressBowPlayer>().targetNPC.whoAmI;

                    arrowShots++;
                }
                else
                {
                    Projectile cloudSprite = Projectile.NewProjectileDirect(player.Center, Vector2.Normalize(player.Center - Main.MouseWorld) * -40, ModContent.ProjectileType<HuntressBallista>(), item.damage, item.knockBack, default);

                    ballistaShots++;

                    if(ballistaShots >= 3)
                    {
                        ballistaShots = 0;
                        arrowShots = 0;
                    }
                }
            }
            if (player.altFunctionUse == 2)
            {
                Projectile cloudSprite = Projectile.NewProjectileDirect(player.Center, Vector2.Normalize(player.Center - Main.MouseWorld) * -4, ModContent.ProjectileType<HuntressGlaive>(), item.damage, item.knockBack, default);
                cloudSprite.ai[1] = player.GetModPlayer<HuntressBowPlayer>().targetNPC.whoAmI;
            }

            return false;
        }
    }

    public class HuntressBowPlayer : ModPlayer
    {
        public NPC targetNPC;

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if(player.HeldItem.type == ModContent.ItemType<HuntressBow>())
            {
                float dist = float.MaxValue;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    float chung = Vector2.Distance(Main.MouseWorld, Main.npc[i].Center);
                    if (chung < dist && Main.npc[i].active)
                    {
                        dist = chung;
                        targetNPC = Main.npc[i];
                    }
                }

                Point to = (Vector2.Normalize(Main.MouseWorld - player.Center) * Main.screenWidth).ToPoint();
                Point from = player.Center.ToPoint();

                Main.spriteBatch.End();
                Main.spriteBatch.Begin();

                Texture2D targetTexture = ModContent.GetTexture("EEMod/Items/Weapons/Ranger/Longbows/HuntressBow/HuntressBowTarget");
                Texture2D ballistaTarget = ModContent.GetTexture("EEMod/Items/Weapons/Ranger/Longbows/HuntressBow/HuntressBallistaTarget");

                if ((player.HeldItem.modItem as HuntressBow).arrowShots >= 15)
                    Main.spriteBatch.Draw(ballistaTarget, Raycast(player.Center, Vector2.Normalize(Main.MouseWorld - player.Center), false, false, 1, 1500) - Main.screenPosition, targetTexture.Bounds, Color.White, Main.GameUpdateCount / 40f, targetTexture.Bounds.Size() / 2f, 1 + ((float)Math.Sin(Main.GameUpdateCount / 60f) / 10f), SpriteEffects.None, 0f);

                else
                    Main.spriteBatch.Draw(targetTexture, targetNPC.Center - Main.screenPosition, targetTexture.Bounds, Color.White, Main.GameUpdateCount / 60f, targetTexture.Bounds.Size() / 2f, 1f, SpriteEffects.None, 0f);


                Main.spriteBatch.End();
                Main.spriteBatch.Begin();
            }
        }

        private static Vector2 Raycast(Vector2 point1, Vector2 direction, bool ignoreNPCs, bool ignoreTiles, int accuracy, int maxIterations)
        {
            int iterations = 0;
            Vector2 currVec = point1;

            while(iterations < maxIterations)
            {
                Vector2 newVec = currVec + (direction * accuracy);
                Point newPoint = newVec.ToPoint();

                if(!ignoreNPCs)
                {
                    for(int i = 0; i < Main.maxNPCs; i++)
                    {
                        if(Main.npc[i].getRect().Contains(newPoint) && Main.npc[i].active)
                        {
                            return newVec;
                        }
                    }
                }

                if(!ignoreTiles)
                {
                    if(!Collision.CanHit(currVec, accuracy, accuracy, newVec, accuracy, accuracy))
                    {
                        return newVec;
                    }
                }

                currVec = newVec;

                iterations++;
            }

            return Vector2.Zero;
        }
    }

    public class HuntressBowFlair : ModProjectile
    {

    }

    public class HuntressArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 30;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private NPC targetNPC;
        private float initialRot;
        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                targetNPC = Main.npc[(int)projectile.ai[1]];

                projectile.rotation = projectile.velocity.ToRotation();
                initialRot = projectile.rotation;
            }
            
            projectile.ai[0]++;

            projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(targetNPC.Center) * 16f, (projectile.ai[0] / 500f));

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }

    public class HuntressCritArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            projectile.width = 6;
            projectile.height = 30;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        private NPC targetNPC;
        private float initialRot;
        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                targetNPC = Main.npc[(int)projectile.ai[1]];

                projectile.rotation = projectile.velocity.ToRotation();
                initialRot = projectile.rotation;
            }

            projectile.ai[0]++;

            projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(targetNPC.Center) * 16f, (projectile.ai[0] / 500f));

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }

    public class HuntressGlaive : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        private NPC targetNPC;
        private int kills;

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                targetNPC = Main.npc[(int)projectile.ai[1]];

                projectile.ai[0]++;
            }

            projectile.velocity = projectile.DirectionTo(targetNPC.Center) * 12f;

            projectile.rotation += 0.15f * (kills + 1);

            if(targetNPC == null)
            {
                float dist = float.MaxValue;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    float chung = Vector2.Distance(projectile.Center, Main.npc[i].Center);
                    if (chung < dist && Main.npc[i].active)
                    {
                        dist = chung;
                        targetNPC = Main.npc[i];
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            kills++;

            float dist = float.MaxValue;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (i == target.whoAmI) continue;

                float chung = Vector2.Distance(projectile.Center, Main.npc[i].Center);
                if (chung < dist && Main.npc[i].active)
                {
                    dist = chung;
                    targetNPC = Main.npc[i];
                }
            }

            if (targetNPC == target) projectile.Kill();

            if (kills > 7) projectile.Kill();
        }
    }

    public class HuntressBallista : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 14;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;

            projectile.hostile = false;
            projectile.friendly = true;

            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}
