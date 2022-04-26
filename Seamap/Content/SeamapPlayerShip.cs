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
using EEMod.Seamap.Content.Islands;

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

        public float boatSpeed = 0.025f;

        public float rot;
        public float forwardSpeed;

        public Vector2 movementVel;

        public override void Update()
        {
            CollisionChecks();

            if(invFrames > 0) invFrames--;

            if (invFrames <= 0)
            {
                if (myPlayer.controlUp && (forwardSpeed < 3))
                {
                    //velocity += Vector2.UnitX.RotatedBy(rot) * boatSpeed;

                    forwardSpeed += boatSpeed;
                    forwardSpeed = MathHelper.Clamp(forwardSpeed, -boatSpeed * 5, 2);
                }
                if (myPlayer.controlDown && forwardSpeed > (-boatSpeed * 5))
                {
                    //velocity -= Vector2.UnitX.RotatedBy(rot) * boatSpeed * 0.5f;

                    forwardSpeed -= boatSpeed;
                    forwardSpeed = MathHelper.Clamp(forwardSpeed, -boatSpeed * 5, 2);
                }
                if (myPlayer.controlRight)
                {
                    rot += 0.03f;
                }
                if (myPlayer.controlLeft)
                {
                    rot -= 0.03f;
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

            position += movementVel - (Seamap.Core.Seamap.windVector * 0.2f);

            forwardSpeed = movementVel.Length();

            base.Update();

            forwardSpeed *= 0.999f;
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

            int frame = 0;
            float spriteRot = 0f;
            bool flipped = false;

            rot = TwoPiRestrict(rot);

            float rotForSprite = TwoPiRestrict(rot + MathHelper.PiOver2);
            float rotAbsed = Math.Abs(rotForSprite - MathHelper.Pi);

            if (rotForSprite > MathHelper.Pi && rotAbsed > (MathHelper.Pi / 9f) && rotAbsed < (8f * MathHelper.Pi / 9f)) flipped = true;

            if(rotAbsed < MathHelper.Pi / 9f)
            {
                frame = 8;
                spriteRot = (DynamicClamp(rotForSprite, MathHelper.Pi / 4.5f) - (MathHelper.Pi / 9f));
            }
            else if(rotAbsed < 2 * MathHelper.Pi / 9f)
            {
                frame = 7;
                spriteRot = DynamicClamp(rotForSprite, MathHelper.Pi / 9f) - (MathHelper.Pi / 18f);
            }
            else if(rotAbsed < 3 * MathHelper.Pi / 9f)
            {
                frame = 6;
                spriteRot = DynamicClamp(rotForSprite, MathHelper.Pi / 9f) - (MathHelper.Pi / 18f);
            }
            else if(rotAbsed < 4 * MathHelper.Pi / 9f)
            {
                frame = 5;
                spriteRot = DynamicClamp(rotForSprite, MathHelper.Pi / 9f) - (MathHelper.Pi / 18f);
            }
            else if(rotAbsed < 5 * MathHelper.Pi / 9f)
            {
                frame = 4;
                spriteRot = DynamicClamp(rotForSprite, MathHelper.Pi / 9f) - (MathHelper.Pi / 18f);
            }
            else if(rotAbsed < 6 * MathHelper.Pi / 9f)
            {
                frame = 3;
                spriteRot = DynamicClamp(rotForSprite, MathHelper.Pi / 9f) - (MathHelper.Pi / 18f);
            }
            else if(rotAbsed < 7 * MathHelper.Pi / 9f)
            {
                frame = 2;
                spriteRot = DynamicClamp(rotForSprite, MathHelper.Pi / 9f) - (MathHelper.Pi / 18f);
            }
            else if (rotAbsed < 8 * MathHelper.Pi / 9f)
            {
                frame = 1;
                spriteRot = DynamicClamp(rotForSprite, MathHelper.Pi / 9f) - (MathHelper.Pi / 18f);
            }
            else
            {
                frame = 0;
                spriteRot = (DynamicClamp(rotAbsed, MathHelper.Pi / 4.5f) - (MathHelper.Pi / 9f)) * (rotForSprite > MathHelper.Pi ? 1f : -1f);
            }


            int yVal = 114 * frame;

            spriteRot += (float)Math.Sin(Main.GameUpdateCount / 5f) * (invFrames / 80f);

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                new Rectangle(0, yVal, 124, 114),
                Color.White.LightSeamap(), spriteRot / 2f, 
                new Rectangle(0, 0, 124, 114).Size() / 2,
                1, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

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

        public float DynamicClamp(float val, float clamper)
        {
            while (val > clamper)
                val -= clamper;

            while (val < 0)
                val += clamper;

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

                        SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/ShipHurt"));

                        velocity += Vector2.Normalize(obj.Center - Center) * boatSpeed * -15;
                        forwardSpeed = 0;
                    }
                }
            }
        }
        #endregion
    }
}
