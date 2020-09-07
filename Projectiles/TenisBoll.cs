using EEMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class TenisBoll : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tennis Ball");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale *= 1f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Bounce(projectile.modProjectile, oldVelocity, .7f);
            return false;
        }

        private static Vector2 SavedVel;

        public void Bounce(ModProjectile modProj, Vector2 oldVelocity, float bouncyness = 1f)
        {
            Projectile projectile = modProj.projectile;
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X * bouncyness;
            }

            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y * bouncyness;
            }
        }

        private readonly int frames = 11;
        private int frame;
        private float ree = 0;

        public static int GetPlayer(Vector2 center, int[] playersToExclude = default, bool activeOnly = true, float distance = -1, Func<Player, bool> CanAdd = null)
        {
            int currentPlayer = -1;
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if ((!activeOnly || (player.active && !player.dead)) && (distance == -1f || player.Distance(center) < distance))
                {
                    bool add = true;
                    if (playersToExclude != default)
                    {
                        foreach (int m in playersToExclude)
                        {
                            if (m == player.whoAmI) { add = false; break; }
                        }
                    }
                    if (add)
                    {
                        distance = player.Distance(center);
                        currentPlayer = i;
                    }
                }
            }
            return currentPlayer;
        }

        private int indexOfProjectile;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(SavedVel);
            writer.WriteVector2(mouseHitBoxVec);
            writer.Write(frame);
            writer.Write(ree);
            writer.Write(indexOfProjectile);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            SavedVel = reader.ReadVector2();
            mouseHitBoxVec = reader.ReadVector2();
            frame = reader.ReadInt32();
            ree = reader.ReadInt32();
            indexOfProjectile = reader.ReadInt32();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player chosenPlayer = Main.player[GetPlayer(projectile.Center)];
            Texture2D volleyArrow = TextureCache.VArrow;
            Main.spriteBatch.Draw(volleyArrow, projectile.Center - Main.screenPosition, new Rectangle(0, volleyArrow.Height / frames * (11 - frame), volleyArrow.Width, volleyArrow.Height / frames), Color.White * ree, new Vector2(mouseHitBoxVec.X - chosenPlayer.Center.X, mouseHitBoxVec.Y - chosenPlayer.Center.Y).ToRotation() + MathHelper.Pi / 2, new Rectangle(0, 0, volleyArrow.Width, volleyArrow.Height).Size() / 2, 1, SpriteEffects.None, 0);
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            float velocitylength = projectile.velocity.Length();
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                if (k != 0)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    //Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length / 2);
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, 0, projectile.width, projectile.height), new Color(255, 255, 255, 10), projectile.rotation, drawOrigin, projectile.scale * (1 - (k / (float)projectile.oldPos.Length)) * (velocitylength * 0.06f), SpriteEffects.None, 0f);
                }
            }
            return true;
        }

        private static Vector2 mouseHitBoxVec;
        public static Projectile chosenRacket;

        public override void AI()
        {
            Player Yoda = Main.player[GetPlayer(projectile.Center)];
            EEPlayer modPlayer = Yoda.GetModPlayer<EEPlayer>();
            if (Main.myPlayer == GetPlayer(projectile.Center))
            {
                mouseHitBoxVec = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                frame = (int)(Yoda.GetModPlayer<EEPlayer>().powerLevel * (11f / modPlayer.maxPowerLevel));
                projectile.netUpdate = true;
            }
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<TennisRachetProj>() && Main.projectile[i].active)
                {
                    if ((Main.projectile[i].Center - projectile.Center).LengthSquared() < (pos - projectile.Center).LengthSquared())
                    {
                        pos = Main.projectile[i].Center;
                        indexOfProjectile = i;
                        projectile.netUpdate = true;
                    }
                }
            }
            chosenRacket = Main.projectile[indexOfProjectile];
            projectile.timeLeft = 100;
            Rectangle mouseHitBox = new Rectangle((int)mouseHitBoxVec.X - 6, (int)mouseHitBoxVec.Y - 6, 12, 12);
            Rectangle projectileHitBox = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            Rectangle playerHitBox = new Rectangle((int)Yoda.position.X - 30, (int)Yoda.position.Y - 30, Yoda.width + 30, Yoda.height + 30);
            Rectangle racketHitBox = new Rectangle((int)chosenRacket.position.X + 14, (int)chosenRacket.position.Y + 2, chosenRacket.width - 14, chosenRacket.height - 2);
            if (chosenRacket != null)
            {
                if (racketHitBox.Intersects(projectileHitBox) && chosenRacket.ai[1] == 0)
                {
                    if (projectile.ai[0] != 2)
                    {
                        chosenRacket.ai[1] = 40;
                        projectile.velocity = new Vector2(chosenRacket.velocity.X * 1.5f, chosenRacket.velocity.Y);
                        for (int i = 0; i < 360; i += 10)
                        {
                            float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                            float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                            Vector2 offset = new Vector2(xdist, ydist);
                            Dust dust = Dust.NewDustPerfect(projectile.Center + offset, DustID.Smoke, offset * 0.5f);
                            dust.noGravity = true;
                            dust.velocity *= 0.96f;
                            dust.noLight = false;
                        }
                    }
                    projectile.netUpdate = true;
                    chosenRacket.netUpdate = true;
                }
                if (projectile.ai[0] == 2)
                {
                    chosenRacket.ai[0] = 1;
                }
                else
                {
                    chosenRacket.ai[0] = 0;
                }
            }
            if (projectile.ai[0] == 0)
            {
                ree = 0;
                projectile.velocity = SavedVel;
                projectile.ai[0] = 1;
                projectile.ai[1] = 0;
                projectile.netUpdate = true;
            }
            if (Yoda.controlUp && mouseHitBox.Intersects(projectileHitBox) && Main.myPlayer == GetPlayer(projectile.Center))
            {
                projectile.ai[0] = 2;
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] == 2)
            {
                if (Yoda.controlUseItem)
                {
                    ree += 0.01f;
                    if (ree > 1)
                    {
                        ree = 1;
                    }
                    SavedVel = Vector2.Normalize(new Vector2(mouseHitBoxVec.X - Yoda.Center.X, mouseHitBoxVec.Y - Yoda.Center.Y)) * modPlayer.powerLevel;
                    projectile.ai[1] = 1;
                    projectile.netUpdate = true;
                }
                if (projectile.ai[1] == 1 && !Yoda.controlUseItem)
                {
                    projectile.ai[0] = 0;
                }
                projectile.Center = Yoda.Center + new Vector2((Yoda.direction * 10) - 10, -20);
            }
            projectile.velocity.Y += 0.2f;
            if (projectile.velocity.Y > 10)
            {
                projectile.velocity.Y = 10;
            }
            projectile.velocity.X *= 0.98f;
            projectile.rotation += projectile.velocity.X / 16f;
        }
    }
}