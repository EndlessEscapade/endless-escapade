using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class AkumoReinforcement : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reinforcemini");
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 58;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.friendly = true;
            Projectile.damage = 50;
            Projectile.knockBack = 4f;
            // Projectile.hostile = false;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            NPC target = Main.npc[(int)Projectile.ai[1]];

            Projectile.ai[0] += 0.01f;
            Projectile.Center = target.Center + Vector2.UnitY.RotatedBy(Projectile.ai[0]) * target.width;
        }

        /*public override void PostDraw(Color lightColor)
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

        /*public override bool PreDraw(ref Color lightColor)
        {
            if (projectile.ai[0] == 3)
                AfterImage.DrawAfterimage(spriteBatch, TextureAssets.Projectile[projectile.type].Value, 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B), overrideFrameCount: 4, overrideFrame: new Rectangle(0, projectile.frame * 56, 56, 58));
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