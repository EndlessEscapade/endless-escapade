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
    public class SeamapPlayerShip : SeamapObject
    {
        public float ShipHelthMax = 5;
        public float shipHelth = 5;
        public int cannonDelay = 60;

        public int abilityDelay = 120;

        public Player myPlayer;

        public int invFrames = 20;


        public SeamapPlayerShip(Vector2 pos, Vector2 vel, Player player) : base(pos, vel)
        {
            position = pos;

            velocity = vel;

            myPlayer = player;

            width = 124;
            height = 98;

            rot = MathHelper.TwoPi * 3f / 4f;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/SeamapPlayerShip", AssetRequestMode.ImmediateLoad).Value;
        }

        public float boatSpeed = 0.075f;

        public float rot;
        public float forwardSpeed;

        public Vector2 movementVel;

        public override void Update()
        {
            CollisionChecks();

            if(invFrames > 0) invFrames--;

            if (invFrames <= 0)
            {
                if (myPlayer.controlUp && (forwardSpeed < (boatSpeed * 25)))
                {
                    //velocity += Vector2.UnitX.RotatedBy(rot) * boatSpeed;

                    forwardSpeed += boatSpeed;
                    forwardSpeed = MathHelper.Clamp(forwardSpeed, -boatSpeed * 5, boatSpeed * 25);
                }
                if (myPlayer.controlDown && forwardSpeed > (-boatSpeed * 5))
                {
                    //velocity -= Vector2.UnitX.RotatedBy(rot) * boatSpeed * 0.5f;

                    forwardSpeed -= boatSpeed * 0.5f;
                    forwardSpeed = MathHelper.Clamp(forwardSpeed, -boatSpeed * 5, boatSpeed * 25);
                }
                if (myPlayer.controlRight)
                {
                    rot += 0.05f;
                }
                if (myPlayer.controlLeft)
                {
                    rot -= 0.05f;
                }

                if (myPlayer.controlUseItem && cannonDelay <= 0 && myPlayer == Main.LocalPlayer)
                {
                    LeftClickAbility();

                    cannonDelay = 60;
                }

                if (myPlayer.controlUseTile && abilityDelay <= 0)
                {
                    RightClickAbility();

                    abilityDelay = 120;
                }

                cannonDelay--;
                abilityDelay--;
            }

            if (shipHelth <= 0) Die();

            movementVel = Vector2.UnitX.RotatedBy(rot) * forwardSpeed;

            position += movementVel;

            base.Update();

            forwardSpeed *= 0.998f;
            velocity *= 0.96f;

            #region Position constraints
            if (position.X < 0) position.X = 0;
            if (position.X > Core.Seamap.seamapWidth - width) position.X = Core.Seamap.seamapWidth - width;

            if (position.Y < 0) position.Y = 0;
            if (position.Y > Core.Seamap.seamapHeight - height - 200) position.Y = Core.Seamap.seamapHeight - height - 200;
            #endregion
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/SeamapPlayerShip").Value;

            int yVal;
            float spriteRot;

            rot = TwoPiRestrict(rot);

            if (rot > MathHelper.TwoPi - (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 4;
            }
            else if (rot > MathHelper.TwoPi - 2f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 3;
            }
            else if (rot > MathHelper.TwoPi - 3f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 2;
            }
            else if (rot > MathHelper.TwoPi - 4f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 4f)) % (float)((2f * Math.PI) / 4f)) - (float)(Math.PI / 4f);
                yVal = 114 * 1;
            }
            else if (rot > MathHelper.TwoPi - 5f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 4f)) % (float)((2f * Math.PI) / 4f)) - (float)(Math.PI / 4f);
                yVal = 114 * 0;
            }
            else if (rot > MathHelper.TwoPi - 6f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 4f)) % (float)((2f * Math.PI) / 4f)) - (float)(Math.PI / 4f);
                yVal = 114 * 1;
            }
            else if (rot > MathHelper.TwoPi - 7f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 2;
            }
            else if (rot > MathHelper.TwoPi - 8f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 3;
            }
            else if (rot > MathHelper.TwoPi - 9f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 4;
            }
            else if (rot > MathHelper.TwoPi - 10f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 5;
            }
            else if (rot > MathHelper.TwoPi - 11f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 6;
            }
            else if (rot > MathHelper.TwoPi - 12f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 7;
            }
            else if (rot > MathHelper.TwoPi - 13f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 8;
            }
            else if (rot > MathHelper.TwoPi - 14f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 7;
            }
            else if (rot > MathHelper.TwoPi - 15f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 6;
            }
            else if (rot > MathHelper.TwoPi - 15f * (Math.PI / 8f))
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 5;
            }
            else
            {
                spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);
                yVal = 114 * 4;
            }

            spriteRot += (float)Math.Sin(Main.GameUpdateCount / 5f) * (invFrames / 80f);

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                new Rectangle(0, yVal, 124, 114),
                Color.White.LightSeamap(), spriteRot / 2f, 
                new Rectangle(0, 0, 124, 114).Size() / 2,
                1, (rot < Math.PI * 2 * 3 / 4 - (Math.PI / 4f) && rot > Math.PI * 2 / 4 - (Math.PI / 4f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            return false;
        }

        public void LeftClickAbility()
        {
            myPlayer.GetModPlayer<ShipyardPlayer>().LeftClickAbility(this);
        }

        public void RightClickAbility()
        {
            myPlayer.GetModPlayer<ShipyardPlayer>().RightClickAbility(this);
        }

        public void Die()
        {
            SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/ShipDeath"));

            myPlayer.GetModPlayer<SeamapPlayer>().ReturnHome();

            shipHelth = ShipHelthMax;
        }

        public float CannonRestrictRange()
        {
            float mouseRot = Vector2.Normalize(Main.MouseWorld - Center).ToRotation() + MathHelper.Pi;

            float angleOfFreedom = 0.4f;

            float realRot = rot;

            /*if (mouseRot - realRot > 0)
                return TwoPiRestrict(MathHelper.Clamp(mouseRot, realRot + 1.57f - angleOfFreedom, realRot + 1.57f + angleOfFreedom));
            else
                return TwoPiRestrict(MathHelper.Clamp(mouseRot, realRot - 1.57f - angleOfFreedom, realRot - 1.57f + angleOfFreedom));*/

            float toTheLeft = TwoPiRestrict(realRot - MathHelper.PiOver2);
            float toTheRight = TwoPiRestrict(realRot + MathHelper.PiOver2);

            if (Math.Acos(Vector2.Dot(Vector2.UnitX.RotatedBy(mouseRot), Vector2.UnitX.RotatedBy(toTheLeft))) < Math.Acos(Vector2.Dot(Vector2.UnitX.RotatedBy(mouseRot), Vector2.UnitX.RotatedBy(toTheRight))))
            {
                if(Math.Acos(Vector2.Dot(Vector2.UnitX.RotatedBy(mouseRot), Vector2.UnitX.RotatedBy(toTheLeft))) > angleOfFreedom)
                {
                    if ((mouseRot - toTheLeft) > MathHelper.Pi) return (toTheLeft - angleOfFreedom);
                    if ((mouseRot - toTheLeft) < -MathHelper.Pi) return (toTheLeft + angleOfFreedom);

                    return (((mouseRot - toTheLeft) < 0)) ? (toTheLeft - angleOfFreedom) : (toTheLeft + angleOfFreedom);
                }
                else
                {
                    return mouseRot;
                }
            }
            else 
            {
                if (Math.Acos(Vector2.Dot(Vector2.UnitX.RotatedBy(mouseRot), Vector2.UnitX.RotatedBy(toTheRight))) > angleOfFreedom)
                {
                    if ((mouseRot - toTheRight) > MathHelper.Pi) return (toTheRight - angleOfFreedom);
                    if ((mouseRot - toTheRight) < -MathHelper.Pi) return (toTheRight + angleOfFreedom);

                    return (((mouseRot - toTheRight) < 0)) ? (toTheRight - angleOfFreedom) : (toTheRight + angleOfFreedom);
                }
                else
                {
                    return mouseRot;
                }
            }
        }

        public float TwoPiRestrict(float val)
        {
            while (val > MathHelper.TwoPi)
                val -= MathHelper.TwoPi;

            while (val < 0)
                val += MathHelper.TwoPi;

            return val;
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

                if (obj.collides && invFrames <= 0)
                {
                    if (new Rectangle((int)position.X, (int)position.Y, 124, 98).Intersects(obj.Hitbox))
                    {
                        if (obj is Cannonball)
                            if ((int)(obj as Cannonball).team == myPlayer.team) continue;

                        shipHelth--;
                        invFrames = 20;

                        velocity += Vector2.Normalize(obj.Center - Center) * boatSpeed * -15;
                        forwardSpeed = 0;
                    }
                }
            }
        }
        #endregion
    }
}
