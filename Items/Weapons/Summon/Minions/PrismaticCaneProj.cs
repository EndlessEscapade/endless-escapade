using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

using EEMod.Projectiles.Enemy;
using EEMod.Extensions;
using EEMod.Prim;
using System.Linq;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class PrismaticCaneProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prismatic Cane");
        }

        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.timeLeft = 999999999;
            Projectile.ignoreWater = true;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            // Projectile.tileCollide = false;
            Projectile.extraUpdates = 12;
        }

        public bool awake = false;

        private Vector2 desiredTarget;

        public override void AI()
        {
            Projectile.timeLeft = 999999999;

            Player owner = Main.player[Projectile.owner];

            Vector2 desiredVector;

            Projectile.ai[0] = owner.ownedProjectileCounts[Projectile.type];

            if (awake)
            {
                desiredTarget = owner.Center;

                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / Projectile.ai[0] * Projectile.ai[1]) + Main.GameUpdateCount * 2)) * 192);
            }
            else
            {
                desiredTarget = Main.MouseWorld;

                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / Projectile.ai[0] * Projectile.ai[1]) + Main.GameUpdateCount * 2)) * 192);
            }

            if (Vector2.Distance(Projectile.Center, desiredVector) > 10)
                Projectile.Center += Vector2.Normalize(desiredVector - Projectile.Center) * 3;
            else
                Projectile.Center = desiredVector;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            float heartBeat;
            if (Main.GameUpdateCount % 150 < 60)
            {
                heartBeat = Math.Abs((float)Math.Sin((Main.GameUpdateCount % 150) * (6.28f / 60))) * (1 - (Main.GameUpdateCount % 150) / (60 * 1.5f));
            }
            else
            {
                heartBeat = 0;
            }

            Projectile.scale = 1f + (heartBeat / 5f);

            Texture2D tex = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Projectiles/Summons/PrismaticCaneProj").Value;
            Texture2D mask = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/SmoothFadeOut").Value;

            float sineAdd = (float)Math.Sin(Main.GameUpdateCount / 20f) + 2.5f;

            Vector2 orig = new Vector2(11, 11);

            Vector2 desiredVector;

            if (awake)
            {
                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / Projectile.ai[0] * (Projectile.ai[1] + 1)) + Main.GameUpdateCount * 2)) * 192);
            }
            else
            {
                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / Projectile.ai[0] * (Projectile.ai[1] + 1)) + Main.GameUpdateCount * 2)) * 192);
            }

            if (Vector2.Distance(Projectile.Center, desiredTarget) <= 194)
            {
                float n = 1 / (desiredVector - Projectile.Center).Length();

                for (float k = 0; k < 1; k += n)
                {
                    Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Particles/Square").Value, Projectile.Center + (desiredVector - Projectile.Center) * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.Lerp(Color.Cyan, Color.Magenta, (float)Math.Sin(Main.GameUpdateCount / 30f)), (desiredVector - Projectile.Center).ToRotation(), Vector2.One, 2f, SpriteEffects.None, 0);
                }
            }

            Helpers.DrawAdditive(mask, Projectile.Center.ForDraw(), Color.White * (0.5f + (heartBeat / 2f)), Projectile.scale, Projectile.rotation);

            Main.spriteBatch.Draw(tex, Projectile.Center.ForDraw(), new Rectangle(0, 0, 22, 22), lightColor, 0f, orig, Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, Projectile.Center.ForDraw(), new Rectangle(0, 0, 22, 22), Color.White * heartBeat, 0f, orig, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}