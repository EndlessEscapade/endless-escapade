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
using EEMod.Seamap.Content;
using System.Diagnostics;
using ReLogic.Content;
using EEMod.Prim;
using EEMod.Extensions;
using EEMod.Seamap.Core;
using EEMod.Seamap.Content.Cannonballs;

namespace EEMod.Seamap.Content
{
    public class EEPlayerShip : SeamapObject
    {
        public float ShipHelthMax = 5;
        public float shipHelth = 5;
        public int cannonDelay = 60;

        public int abilityDelay = 120;

        public Player myPlayer;

        public int invFrames = 20;


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

            position += velocity - (Core.Seamap.windVector * 0.3f * ((120 - (abilityDelay < 0 ? 0 : abilityDelay)) / 120f));

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

                if (myPlayer.controlUseItem && cannonDelay <= 0 && myPlayer == Main.LocalPlayer)
                {
                    LeftClickAbility();

                    ShenCannonball cannonball = new ShenCannonball(Center, velocity + Vector2.Normalize(Main.MouseWorld - Center) * 6, Color.Lerp(Color.OrangeRed, Color.Goldenrod, (float)Math.Sin(Main.GameUpdateCount / 180f).PositiveSin()));

                    velocity -= Vector2.Normalize(Main.MouseWorld - Center) * 0.5f;

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
            if (position.X > Core.Seamap.seamapWidth - width) position.X = Core.Seamap.seamapWidth - width;

            if (position.Y < 0) position.Y = 0;
            if (position.Y > Core.Seamap.seamapHeight - height - 200) position.Y = Core.Seamap.seamapHeight - height - 200;

            if (shipHelth <= 0)
            {
                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/ShipDeath"));

                myPlayer.GetModPlayer<EEPlayer>().ReturnHome();

                shipHelth = ShipHelthMax;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            EEPlayer eePlayer = myPlayer.GetModPlayer<EEPlayer>();


            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/SeamapPlayerShipAlt").Value;

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                new Rectangle(0, 0, 90, 90),
                Color.White.LightSeamap(), (velocity.X / 10) + ((float)Math.Sin(Main.GameUpdateCount / (invFrames > 0 ? 40f : 120f)) * (invFrames > 0 ? 0.1f : 0.075f)), 
                new Rectangle(0, 0, 90, 90).Size() / 2,
                1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            /*spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition, null,
                Color.White.LightSeamap(), (velocity.X / 10) + ((float)Math.Sin(Main.GameUpdateCount / (invFrames > 0 ? 40f : 120f)) * (invFrames > 0 ? 0.1f : 0.075f)),
                new Rectangle(0, 0, 90, 90).Size() / 2,
                1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);*/

            return false;
        }

        public void LeftClickAbility()
        {

        }

        public void RightClickAbility()
        {

        }

        #region Collision nonsense
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

        public void CollisionChecks()
        {
            foreach (SeamapObject obj in SeamapObjects.SeamapEntities)
            {
                if (obj == null) continue;

                if (obj.collides && invFrames < 0)
                {
                    if (Hitbox.Intersects(obj.Hitbox))
                    {
                        shipHelth--;
                        invFrames = 20;

                        velocity = Vector2.Normalize(obj.Center - Center) * boatSpeed * -4;
                    }
                }
            }
        }
        #endregion
    }
}
