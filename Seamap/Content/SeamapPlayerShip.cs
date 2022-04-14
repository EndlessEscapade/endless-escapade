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

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/SeamapPlayerShip", AssetRequestMode.ImmediateLoad).Value;
        }

        public float boatSpeed = 0.15f;

        public float rot;
        public float forwardSpeed;

        public override void Update()
        {
            CollisionChecks();

            invFrames--;

            if (invFrames < 0)
            {
                if (myPlayer.controlUp)
                {
                    forwardSpeed += boatSpeed;
                    forwardSpeed = MathHelper.Clamp(forwardSpeed, -boatSpeed * 10, boatSpeed * 15);
                }
                if (myPlayer.controlDown)
                {
                    forwardSpeed -= boatSpeed;
                    forwardSpeed = MathHelper.Clamp(forwardSpeed, -boatSpeed * 10, boatSpeed * 15);
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

                if (abilityDelay > 110)
                {
                    velocity *= 0.2f;
                }
            }

            if (shipHelth <= 0) Die();

            velocity = Vector2.UnitX.RotatedBy(rot) * forwardSpeed;

            base.Update();

            velocity *= 0.998f;

            #region Position constraints
            if (position.X < 0) position.X = 0;
            if (position.X > Core.Seamap.seamapWidth - width) position.X = Core.Seamap.seamapWidth - width;

            if (position.Y < 0) position.Y = 0;
            if (position.Y > Core.Seamap.seamapHeight - height - 200) position.Y = Core.Seamap.seamapHeight - height - 200;
            #endregion
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            EEPlayer eePlayer = myPlayer.GetModPlayer<EEPlayer>();


            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/SeamapPlayerShip").Value;

            int yVal;

            while (rot > MathHelper.TwoPi)
                rot -= MathHelper.TwoPi;

            while (rot < 0)
                rot += MathHelper.TwoPi;

            if (rot > (2f * Math.PI) - (Math.PI / 8f))
                yVal = 114 * 2;
            else if (rot > (2f * Math.PI) - 3f * (Math.PI / 8f))
                yVal = 114 * 1;
            else if (rot > (2f * Math.PI) - 5f * (Math.PI / 8f))
                yVal = 114 * 0;
            else if (rot > (2f * Math.PI) - 7f * (Math.PI / 8f))
                yVal = 114 * 1;
            else if (rot > (2f * Math.PI) - 9f * (Math.PI / 8f))
                yVal = 114 * 2;
            else if (rot > (2f * Math.PI) - 11f * (Math.PI / 8f))
                yVal = 114 * 3;
            else if (rot > (2f * Math.PI) - 13f * (Math.PI / 8f))
                yVal = 114 * 4;
            else if (rot > (2f * Math.PI) - 15f * (Math.PI / 8f))
                yVal = 114 * 3;
            else
                yVal = 114 * 2;

            float spriteRot = ((float)(rot + (Math.PI / 8f)) % (float)((2f * Math.PI) / 8f)) - (float)(Math.PI / 8f);

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                new Rectangle(0, yVal, 124, 114),
                Color.White.LightSeamap(), spriteRot / 2f, 
                new Rectangle(0, 0, 124, 114).Size() / 2,
                1, (rot < Math.PI * 2 * 3 / 4 - (Math.PI / 8f) && rot > Math.PI * 2 / 4 - (Math.PI / 8f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

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
            float mouseRot = Vector2.Normalize(Main.MouseWorld - Center).ToRotation();

            if(mouseRot - rot < 0)
                return MathHelper.Clamp(mouseRot, rot - 1.57f - 0.4f, rot - 1.57f + 0.4f) - (float)Math.PI;
            else
                return MathHelper.Clamp(mouseRot, rot + 1.57f - 0.4f, rot + 1.57f + 0.4f) - (float)Math.PI;
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
                    if (new Rectangle((int)position.X, (int)position.Y, 124, 98).Intersects(obj.Hitbox))
                    {
                        if (obj is Cannonball)
                            if ((int)(obj as Cannonball).team == myPlayer.team) continue;

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
