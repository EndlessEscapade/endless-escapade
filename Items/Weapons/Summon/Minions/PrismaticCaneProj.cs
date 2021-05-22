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
using EEMod.NPCs.CoralReefs;
using EEMod.Projectiles.Enemy;
using EEMod.Extensions;
using EEMod.Prim;
using System.Linq;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class PrismaticCaneProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prismatic Cane");
        }

        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.timeLeft = 999999999;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.extraUpdates = 12;
        }

        public bool awake = false;

        private Vector2 desiredTarget;

        public override void AI()
        {
            projectile.timeLeft = 999999999;

            Player owner = Main.player[projectile.owner];

            Vector2 desiredVector;

            projectile.ai[0] = owner.ownedProjectileCounts[projectile.type];

            if (awake)
            {
                desiredTarget = owner.Center;

                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / projectile.ai[0] * projectile.ai[1]) + Main.GameUpdateCount * 2)) * 192);
            }
            else
            {
                desiredTarget = Main.MouseWorld;

                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / projectile.ai[0] * projectile.ai[1]) + Main.GameUpdateCount * 2)) * 192);
            }

            if (Vector2.Distance(projectile.Center, desiredVector) > 10)
                projectile.Center += Vector2.Normalize(desiredVector - projectile.Center) * 3;
            else
                projectile.Center = desiredVector;
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
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

            projectile.scale = 1f + (heartBeat / 5f);

            Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Projectiles/Summons/PrismaticCaneProj");
            Texture2D mask = ModContent.GetInstance<EEMod>().GetTexture("Masks/SmoothFadeOut");

            float sineAdd = (float)Math.Sin(Main.GameUpdateCount / 20f) + 2.5f;

            Vector2 orig = new Vector2(11, 11);

            Vector2 desiredVector;

            if (awake)
            {
                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / projectile.ai[0] * (projectile.ai[1] + 1)) + Main.GameUpdateCount * 2)) * 192);
            }
            else
            {
                desiredVector = desiredTarget + (Vector2.UnitX.RotatedBy(MathHelper.ToRadians((360f / projectile.ai[0] * (projectile.ai[1] + 1)) + Main.GameUpdateCount * 2)) * 192);
            }

            if (Vector2.Distance(projectile.Center, desiredTarget) <= 194)
            {
                float n = 1 / (desiredVector - projectile.Center).Length();

                for (float k = 0; k < 1; k += n)
                {
                    Main.spriteBatch.Draw(mod.GetTexture("Particles/Square"), projectile.Center + (desiredVector - projectile.Center) * k - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.Lerp(Color.Cyan, Color.Magenta, (float)Math.Sin(Main.GameUpdateCount / 30f)), (desiredVector - projectile.Center).ToRotation(), Vector2.One, 2f, SpriteEffects.None, 0);
                }
            }

            Helpers.DrawAdditive(mask, projectile.Center.ForDraw(), Color.White * (0.5f + (heartBeat / 2f)), projectile.scale, projectile.rotation);

            Main.spriteBatch.Draw(tex, projectile.Center.ForDraw(), new Rectangle(0, 0, 22, 22), lightColor, 0f, orig, projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, projectile.Center.ForDraw(), new Rectangle(0, 0, 22, 22), Color.White * heartBeat, 0f, orig, projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}