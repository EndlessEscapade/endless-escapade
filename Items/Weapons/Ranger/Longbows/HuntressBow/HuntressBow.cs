using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EEMod.Items.Weapons.Ranger.Longbows.HuntressBow
{
    public class HuntressBow : EEItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Huntress' Recurve Bow");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.HoldingOut;

            Item.useAnimation = 18;
            Item.useTime = 6;
            Item.reuseDelay = 35;

            Item.shootSpeed = 16f;
            Item.knockBack = 6.5f;
            Item.width = 30;
            Item.height = 58;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.noMelee = false;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Arrow;

            Item.ranged = true;

            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<ShimmerShotProj>();
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
                Item.noUseGraphic = true;
                Item.useStyle = ItemUseStyleID.SwingThrow;

                Item.useTime = 10;
                Item.useAnimation = 10;
                Item.reuseDelay = 60;

                Item.damage = 50;

                Item.autoReuse = false;
            }
            else
            {
                if (arrowShots < 15)
                {
                    Item.noUseGraphic = false;
                    Item.useStyle = ItemUseStyleID.HoldingOut;

                    Item.useAnimation = 18;
                    Item.useTime = 6;
                    Item.reuseDelay = 35;

                    Item.damage = 25;

                    Item.autoReuse = true;
                }
                else
                {
                    Item.noUseGraphic = false;
                    Item.useStyle = ItemUseStyleID.HoldingOut;

                    Item.useAnimation = 30;
                    Item.useTime = 30;
                    Item.reuseDelay = 30;

                    Item.damage = 60;

                    Item.autoReuse = false;
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
                    Projectile cloudSprite = Projectile.NewProjectileDirect(player.Center, Vector2.Normalize(player.Center - Main.MouseWorld) * -4, ModContent.ProjectileType<HuntressArrow>(), Item.damage, Item.knockBack, default);
                    cloudSprite.ai[1] = player.GetModPlayer<HuntressBowPlayer>().targetNPC.whoAmI;

                    arrowShots++;
                }
                else
                {
                    Projectile cloudSprite = Projectile.NewProjectileDirect(player.Center, Vector2.Normalize(player.Center - Main.MouseWorld) * -40, ModContent.ProjectileType<HuntressBallista>(), Item.damage, Item.knockBack, default);

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
                Projectile cloudSprite = Projectile.NewProjectileDirect(player.Center, Vector2.Normalize(player.Center - Main.MouseWorld) * -4, ModContent.ProjectileType<HuntressGlaive>(), Item.damage, Item.knockBack, default);
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

    public class HuntressBowFlair : EEProjectile
    {

    }

    public class HuntressArrow : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 30;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;

            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private NPC targetNPC;
        private float initialRot;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                targetNPC = Main.npc[(int)Projectile.ai[1]];

                Projectile.rotation = Projectile.velocity.ToRotation();
                initialRot = Projectile.rotation;
            }
            
            Projectile.ai[0]++;

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(targetNPC.Center) * 16f, (Projectile.ai[0] / 500f));

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }

    public class HuntressCritArrow : EEProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 30;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;

            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private NPC targetNPC;
        private float initialRot;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                targetNPC = Main.npc[(int)Projectile.ai[1]];

                Projectile.rotation = Projectile.velocity.ToRotation();
                initialRot = Projectile.rotation;
            }

            Projectile.ai[0]++;

            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(targetNPC.Center) * 16f, (Projectile.ai[0] / 500f));

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }

    public class HuntressGlaive : EEProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;

            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private NPC targetNPC;
        private int kills;

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                targetNPC = Main.npc[(int)Projectile.ai[1]];

                Projectile.ai[0]++;
            }

            Projectile.velocity = Projectile.DirectionTo(targetNPC.Center) * 12f;

            Projectile.rotation += 0.15f * (kills + 1);

            if(targetNPC == null)
            {
                float dist = float.MaxValue;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    float chung = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
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

                float chung = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
                if (chung < dist && Main.npc[i].active)
                {
                    dist = chung;
                    targetNPC = Main.npc[i];
                }
            }

            if (targetNPC == target) Projectile.Kill();

            if (kills > 7) Projectile.Kill();
        }
    }

    public class HuntressBallista : EEProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 14;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;

            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
