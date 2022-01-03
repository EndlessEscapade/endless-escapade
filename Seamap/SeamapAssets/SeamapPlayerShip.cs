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

namespace EEMod.Seamap.SeamapContent
{
    public class EEPlayerShip : SeamapObject
    {
        public float ShipHelthMax = 7;
        public float shipHelth = 7;
        public int cannonDelay = 60;

        public int abilityDelay = 120;

        public Player myPlayer;

        public EEPlayerShip(Vector2 pos, Vector2 vel, Player player) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            myPlayer = player;

            width = 44;
            height = 52;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/SeamapPlayerShip", AssetRequestMode.ImmediateLoad).Value;
        }

        public override void Update()
        {
            float boatSpeed = 0.5f;

            position += velocity;
            if (myPlayer.controlUp)
            {
                velocity.Y -= 0.1f * boatSpeed;
            }
            if (myPlayer.controlDown)
            {
                velocity.Y += 0.1f * boatSpeed;
            }
            if (myPlayer.controlRight)
            {
                velocity.X += 0.1f * boatSpeed;
            }
            if (myPlayer.controlLeft)
            {
                velocity.X -= 0.1f * boatSpeed;
            }
            if (myPlayer.controlUseItem && cannonDelay <= 0)
            {
                SeamapObjects.NewSeamapObject(new FriendlyCannonball(Center, Vector2.Normalize(Main.MouseWorld - Center) * 4));

                SoundEngine.PlaySound(SoundID.Item61);
                cannonDelay = 60;
            }
            if (myPlayer.controlUseTile && abilityDelay <= 0)
            {
                //velocity = Vector2.Zero;
                SoundEngine.PlaySound(SoundID.Item37);
                abilityDelay = 120;
            }

            if (abilityDelay > 110)
            {
                velocity *= 0.2f;
            }

            cannonDelay--;
            abilityDelay--;

            Vector2 v = new Vector2(boatSpeed * 4);

            velocity = Vector2.Clamp(velocity, -v, v);

            //velocity *= 0.98f;

            base.Update();

            if (position.X < 0) position.X = 0;
            if (position.X > Seamap.seamapWidth - width) position.X = Seamap.seamapWidth - width;

            if (position.Y < 0) position.Y = 0;
            if (position.Y > Seamap.seamapHeight - height) position.Y = Seamap.seamapHeight - height;
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            int frameNum = 0;
            EEPlayer eePlayer = myPlayer.GetModPlayer<EEPlayer>();

            if (Main.netMode == NetmodeID.SinglePlayer || (myPlayer.team == 0))
            {
                if (eePlayer.boatSpeed == 3)
                {
                    frameNum = 1;
                }

                if (eePlayer.boatSpeed == 1)
                {
                    frameNum = 0;
                }
            }

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                switch (myPlayer.team)
                {
                    case 1:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 3;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 2;
                        break;
                    case 2:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 9;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 8;
                        break;
                    case 3:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 5;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 4;
                        break;
                    case 4:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 7;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 6;
                        break;
                    case 5:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 11;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 10;
                        break;
                }
            }

            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/SeamapPlayerShip").Value;

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                new Rectangle(0, frameNum * 52, 44, 52),
                Color.White * (1 - (eePlayer.cutSceneTriggerTimer / 180f)),
                velocity.X / 10, new Rectangle(0, frameNum * 52, 44, 52).Size() / 2,
                1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            return false;
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
