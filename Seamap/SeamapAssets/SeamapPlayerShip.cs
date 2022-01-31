using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EEMod.ID;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.ID;
using EEMod.Seamap.SeamapAssets;
using System.Diagnostics;
using ReLogic.Content;
using EEMod.Prim;

namespace EEMod.Seamap.SeamapContent
{
    public class EEPlayerShip : SeamapObject
    {
        public float ShipHelthMax = 20;
        public float shipHelth = 20;
        public int cannonDelay = 60;

        public int abilityDelay = 120;

        public Player myPlayer;

        public EEPlayerShip(Vector2 pos, Vector2 vel, Player player) : base(pos, vel)
        {
            position = pos;

            velocity = vel;

            myPlayer = player;

            width = 44;
            height = 48;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/SeamapPlayerShip", AssetRequestMode.ImmediateLoad).Value;
        }

        public float boatSpeed = 0.3f;

        public override void Update()
        {
            invFrames--;

            position += velocity;

            CollisionChecks();

            if (invFrames < 0)
            {
                if (myPlayer.controlUp)
                {
                    velocity.Y -= ((velocity.Y > 0) ? 0.2f : 0.1f) * boatSpeed;
                }
                if (myPlayer.controlDown)
                {
                    velocity.Y += ((velocity.Y < 0) ? 0.2f : 0.1f) * boatSpeed;
                }
                if (myPlayer.controlRight)
                {
                    velocity.X += ((velocity.X < 0) ? 0.2f : 0.1f) * boatSpeed;
                }
                if (myPlayer.controlLeft)
                {
                    velocity.X -= ((velocity.X > 0) ? 0.2f : 0.1f) * boatSpeed;
                }

                if (myPlayer.controlUseItem && cannonDelay <= 0)
                {
                    FriendlyCannonball cannonball = new FriendlyCannonball(Center, velocity + Vector2.Normalize(Main.MouseWorld - Center) * 4);

                    PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(cannonball, Color.Purple, 40));

                    SeamapObjects.NewSeamapObject(cannonball);

                    SoundEngine.PlaySound(SoundID.Item38);
                    cannonDelay = 60;
                }

                if (myPlayer.controlUseTile && abilityDelay <= 0)
                {
                    //velocity = Vector2.Zero;
                    SoundEngine.PlaySound(SoundID.Item37);
                    abilityDelay = 120;
                }

                cannonDelay--;
                abilityDelay--;

                if (abilityDelay > 110)
                {
                    velocity *= 0.2f;
                }
            }

            Vector2 v = new Vector2(boatSpeed * 4);

            velocity = Vector2.Clamp(velocity, -v, v);

            velocity *= 0.998f;

            base.Update();

            if (position.X < 0) position.X = 0;
            if (position.X > Seamap.seamapWidth - width) position.X = Seamap.seamapWidth - width;

            if (position.Y < 0) position.Y = 0;
            if (position.Y > Seamap.seamapHeight - height - 200) position.Y = Seamap.seamapHeight - height - 200;

            if (shipHelth <= 0) Main.LocalPlayer.GetModPlayer<EEPlayer>().ReturnHome();
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            int frameNum = 0;
            EEPlayer eePlayer = myPlayer.GetModPlayer<EEPlayer>();


            if (eePlayer.boatSpeed == 3)
                frameNum = 1;
            if (eePlayer.boatSpeed == 1)
                frameNum = 0;

            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/SeamapPlayerShip").Value;

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                new Rectangle(0, frameNum * 48, 44, 48),
                Color.White.LightSeamap(), (velocity.X / 10) + ((float)Math.Sin(Main.GameUpdateCount / (invFrames > 0 ? 40f : 120f)) * (invFrames > 0 ? 0.1f : 0.075f)), 
                new Rectangle(0, frameNum * 48, 44, 48).Size() / 2,
                1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            return false;
        }

        public int invFrames = 20;

        public void CollisionChecks()
        {
            foreach (SeamapObject obj in SeamapObjects.SeamapEntities)
            {
                if (obj == null) continue;

                if (obj.collides && invFrames < 0)
                {
                    if(IsTouchingLeft(Hitbox, obj.Hitbox, velocity) ||
                       IsTouchingTop(Hitbox, obj.Hitbox, velocity) ||
                       IsTouchingRight(Hitbox, obj.Hitbox, velocity) ||
                       IsTouchingBottom(Hitbox, obj.Hitbox, velocity))
                    {
                        shipHelth--;
                        invFrames = 20;

                        velocity = Vector2.Normalize(obj.Center - Center) * boatSpeed * -4;
                    }
                }
            }
        }

        public static bool IsTouchingLeft(Rectangle rect1, Rectangle rect2, Vector2 vel)
        {
            return rect1.Right + vel.X > rect2.Left &&
              rect1.Left < rect2.Left &&
              rect1.Bottom > rect2.Top &&
              rect1.Top < rect2.Bottom;
        }

        public static bool IsTouchingRight(Rectangle rect1, Rectangle rect2, Vector2 vel)
        {
            return rect1.Left + vel.X < rect2.Right &&
              rect1.Right > rect2.Right &&
              rect1.Bottom > rect2.Top &&
              rect1.Top < rect2.Bottom;
        }

        public static bool IsTouchingTop(Rectangle rect1, Rectangle rect2, Vector2 vel)
        {
            return rect1.Bottom + vel.Y > rect2.Top &&
              rect1.Top < rect2.Top &&
              rect1.Right > rect2.Left &&
              rect1.Left < rect2.Right;
        }

        public static bool IsTouchingBottom(Rectangle rect1, Rectangle rect2, Vector2 vel)
        {
            return rect1.Top + vel.Y < rect2.Bottom &&
              rect1.Bottom > rect2.Bottom &&
              rect1.Right > rect2.Left &&
              rect1.Left < rect2.Right;
        }
    }
}
