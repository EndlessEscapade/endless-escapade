using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Volleyball : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Volleyball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 52;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1f;
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

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(SavedVel);
            writer.WriteVector2(mouseHitBoxVec);
            writer.Write(frame);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            SavedVel = reader.ReadVector2();
            mouseHitBoxVec = reader.ReadVector2();
            frame = reader.ReadInt32();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player chosenPlayer = Main.player[GetPlayer(Projectile.Center)];
            Texture2D outline = Mod.Assets.Request<Texture2D>("Projectiles/VolleyballArrowOutline").Value;
            Texture2D fill = Mod.Assets.Request<Texture2D>("Projectiles/VolleyballArrowFill").Value;

            Main.spriteBatch.Draw(outline, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, outline.Width, outline.Height), Color.White * ree, new Vector2(mouseHitBoxVec.X - chosenPlayer.Center.X, mouseHitBoxVec.Y - chosenPlayer.Center.Y).ToRotation() + MathHelper.PiOver2, new Rectangle(0, 0, outline.Width, outline.Height).Size() / 2, 1, SpriteEffects.None, 0);

            Main.spriteBatch.Draw(fill, Projectile.Center - Main.screenPosition, new Rectangle(0, fill.Height / frames * (11 - frame), fill.Width, fill.Height / frames), Color.White * ree, new Vector2(mouseHitBoxVec.X - chosenPlayer.Center.X, mouseHitBoxVec.Y - chosenPlayer.Center.Y).ToRotation() + MathHelper.PiOver2, new Rectangle(0, 0, fill.Width, fill.Height).Size() / 2, 1, SpriteEffects.None, 0);
            return true;
        }

        private static Vector2 mouseHitBoxVec;

        public override void AI()
        {
            Player Yoda = Main.player[GetPlayer(Projectile.Center)];
            EEPlayer modPlayer = Yoda.GetModPlayer<EEPlayer>();
            if (Main.myPlayer == GetPlayer(Projectile.Center))
            {
                mouseHitBoxVec = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                frame = (int)(Yoda.GetModPlayer<EEPlayer>().powerLevel * (11f / modPlayer.maxPowerLevel));
            }
            Projectile.owner = GetPlayer(Projectile.Center);
            Projectile.timeLeft = 100;
            Rectangle mouseHitBox = new Rectangle((int)mouseHitBoxVec.X - 6, (int)mouseHitBoxVec.Y - 6, 12, 12);
            Rectangle projectileHitBox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
            Rectangle playerHitBox = new Rectangle((int)Yoda.position.X - 30, (int)Yoda.position.Y - 30, Yoda.width + 30, Yoda.height + 30);
            if (playerHitBox.Intersects(projectileHitBox))
            {
                if (Projectile.ai[0] == 1 && Yoda.controlUseItem)
                {
                    SavedVel = Vector2.Normalize(new Vector2(mouseHitBoxVec.X - Yoda.Center.X, mouseHitBoxVec.Y - Yoda.Center.Y)) * 10;
                    Projectile.ai[0] = 0;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.ai[0] == 0)
            {
                ree = 0;
                Projectile.velocity = SavedVel;
                Projectile.ai[0] = 1;
                Projectile.ai[1] = 0;
            }
            if (Yoda.controlUp && mouseHitBox.Intersects(projectileHitBox))
            {
                Projectile.ai[0] = 2;
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
                Projectile.Center = Yoda.Center + new Vector2((Yoda.direction * 10) - 10, -30);
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