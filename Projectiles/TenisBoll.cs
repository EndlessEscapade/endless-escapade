using EEMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.TennisRackets;

namespace EEMod.Projectiles
{
    public class TenisBoll : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tennis Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Bounce(Projectile.ModProjectile, oldVelocity, .7f);
            return false;
        }

        private static Vector2 SavedVel;

        public void Bounce(ModProjectile modProj, Vector2 oldVelocity, float bouncyness = 1f)
        {
            Projectile projectile = modProj.Projectile;
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
            Player chosenPlayer = Main.player[GetPlayer(Projectile.Center)];
            Texture2D volleyArrow = Mod.Assets.Request<Texture2D>("Projectiles/VolleyballArrow").Value;
            Main.spriteBatch.Draw(volleyArrow, Projectile.Center - Main.screenPosition, new Rectangle(0, volleyArrow.Height / frames * (11 - frame), volleyArrow.Width, volleyArrow.Height / frames), Color.White * ree, new Vector2(mouseHitBoxVec.X - chosenPlayer.Center.X, mouseHitBoxVec.Y - chosenPlayer.Center.Y).ToRotation() + MathHelper.PiOver2, new Rectangle(0, 0, volleyArrow.Width, volleyArrow.Height).Size() / 2, 1, SpriteEffects.None, 0);
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            float velocitylength = Projectile.velocity.Length();
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (k != 0)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length / 2);
                    spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(0, 0, Projectile.width, Projectile.height), new Color(255, 255, 255, 10), Projectile.rotation, drawOrigin, Projectile.scale * (1 - (k / (float)Projectile.oldPos.Length)) * (velocitylength * 0.06f), SpriteEffects.None, 0f);
                }
            }
            return true;
        }

        private static Vector2 mouseHitBoxVec;
        public static Projectile chosenRacket;

        private bool isRacket(int i)
        {
            return Main.projectile[i].type == ModContent.ProjectileType<TennisRacketProj>();
        }

        public override void AI()
        {
            Player Yoda = Main.player[GetPlayer(Projectile.Center)];
            EEPlayer modPlayer = Yoda.GetModPlayer<EEPlayer>();
            if (Main.myPlayer == GetPlayer(Projectile.Center))
            {
                mouseHitBoxVec = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                frame = (int)(Yoda.GetModPlayer<EEPlayer>().powerLevel * (11f / modPlayer.maxPowerLevel));
                Projectile.netUpdate = true;
            }
            Vector2 pos = Vector2.Zero;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (isRacket(i) && Main.projectile[i].active)
                {
                    if ((Main.projectile[i].Center - Projectile.Center).LengthSquared() < (pos - Projectile.Center).LengthSquared())
                    {
                        pos = Main.projectile[i].Center;
                        indexOfProjectile = i;
                        Projectile.netUpdate = true;
                    }
                }
            }
            chosenRacket = Main.projectile[indexOfProjectile];
            Projectile.timeLeft = 100;
            Rectangle mouseHitBox = new Rectangle((int)mouseHitBoxVec.X - 6, (int)mouseHitBoxVec.Y - 6, 12, 12);
            Rectangle projectileHitBox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            Rectangle playerHitBox = new Rectangle((int)Yoda.position.X - 30, (int)Yoda.position.Y - 30, Yoda.width + 30, Yoda.height + 30);
            Rectangle racketHitBox = new Rectangle((int)chosenRacket.position.X + 14, (int)chosenRacket.position.Y + 2, chosenRacket.width - 14, chosenRacket.height - 2);
            if (chosenRacket != null)
            {
                if (racketHitBox.Intersects(projectileHitBox) && chosenRacket.ai[1] == 0)
                {
                    if (Projectile.ai[0] != 2)
                    {
                        chosenRacket.ai[1] = 40;
                        Projectile.velocity = new Vector2(chosenRacket.velocity.X * 1.5f, chosenRacket.velocity.Y);
                        for (int i = 0; i < 360; i += 10)
                        {
                            float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                            float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                            Vector2 offset = new Vector2(xdist, ydist);
                            Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.Smoke, offset * 0.5f);
                            dust.noGravity = true;
                            dust.velocity *= 0.96f;
                            // dust.noLight = false;
                        }
                    }
                    Projectile.netUpdate = true;
                    chosenRacket.netUpdate = true;
                }
                if (Projectile.ai[0] == 2)
                {
                    chosenRacket.ai[0] = 1;
                }
                else
                {
                    chosenRacket.ai[0] = 0;
                }
            }
            if (Projectile.ai[0] == 0)
            {
                ree = 0;
                Projectile.velocity = SavedVel;
                Projectile.ai[0] = 1;
                Projectile.ai[1] = 0;
                Projectile.netUpdate = true;
            }
            if (Yoda.controlUp && mouseHitBox.Intersects(projectileHitBox) && Main.myPlayer == GetPlayer(Projectile.Center))
            {
                Projectile.ai[0] = 2;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 2)
            {
                if (Yoda.controlUseItem)
                {
                    ree += 0.01f;
                    if (ree > 1)
                    {
                        ree = 1;
                    }
                    SavedVel = Vector2.Normalize(new Vector2(mouseHitBoxVec.X - Yoda.Center.X, mouseHitBoxVec.Y - Yoda.Center.Y)) * modPlayer.powerLevel;
                    Projectile.ai[1] = 1;
                    Projectile.netUpdate = true;
                }
                if (Projectile.ai[1] == 1 && !Yoda.controlUseItem)
                {
                    Projectile.ai[0] = 0;
                }
                Projectile.Center = Yoda.Center + new Vector2((Yoda.direction * 10) - 10, -20);
            }
            Projectile.velocity.Y += 0.2f;
            if (Projectile.velocity.Y > 10)
            {
                Projectile.velocity.Y = 10;
            }
            Projectile.velocity.X *= 0.98f;
            Projectile.rotation += Projectile.velocity.X / 16f;
        }
    }
}