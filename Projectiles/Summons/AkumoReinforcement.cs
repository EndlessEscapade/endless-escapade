using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Summons
{
    public class AkumoReinforcement : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reinforcemini");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 58;
            projectile.penetrate = -1;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.friendly = true;
            projectile.damage = 50;
            projectile.knockBack = 4f;
            projectile.hostile = false;
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            NPC target = Main.npc[(int)projectile.ai[1]];

            projectile.ai[0] += 0.01f;
            projectile.Center = target.Center + Vector2.UnitY.RotatedBy(projectile.ai[0]) * target.width;
        }

        /*public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float funnySin = (float)Math.Sin(projectileAiCont[0]);
            Texture2D texture = mod.GetTexture("Projectiles/Summons/AkumoMinionGlow");
            Vector2 funny = projectile.Center.ForDraw();
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 10, 0)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width/2, projectile.height/2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 0, 10)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * -10, 0)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 0, -10)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)funny.X, (int)funny.Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.75f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
        }*/

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] == 3)
                AfterImage.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B), overrideFrameCount: 4, overrideFrame: new Rectangle(0, projectile.frame * 56, 56, 58));
            return true;
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(funnySin * 10, 0), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(0, funnySin * 10), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(funnySin * -10, 0), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(0, funnySin * -10), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition, texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.75f);
        }*/

        public override void Kill(int timeleft)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<AkumoBuff>()))
            {
                Main.LocalPlayer.ClearBuff(ModContent.BuffType<AkumoBuff>());
            }
        }
    }
}