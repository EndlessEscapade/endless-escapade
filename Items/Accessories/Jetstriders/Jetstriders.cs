using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;

namespace EEMod.Items.Accessories.Jetstriders
{
    public class Jetstriders : EEItem
    {
        int charges = 3;
        bool holdingSpace = false;

        private Vector2 direction = Vector2.Zero;
        private Vector2 oldDirection = Vector2.Zero;

        private int runTimer = 0;

        private float flipDegrees = 0;

        private int leanTimer = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Jetstriders");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.defense = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlJump)
            {
                if (!holdingSpace && charges > 0 && player.velocity.Y != 0)
                {
                    charges--;
                    float jumpPower = 13;

                    oldDirection = direction;
                    direction = -Vector2.UnitY;
                    if (player.controlLeft)
                    {
                        jumpPower = 12;
                        direction.X = -1;
                        flipDegrees = -0.1f;
                    }
                    if (player.controlRight)
                    {
                        jumpPower = 12;
                        direction.X = 1;
                        flipDegrees = 0.1f;
                    }
                    direction = Vector2.Normalize(direction) * jumpPower;
                    if (runTimer > 0)
                        oldDirection += direction;
                    player.velocity = direction;
                    runTimer = 40;
                    leanTimer = 4;
                }
                holdingSpace = true;
            }
            else
                holdingSpace = false;
            if (player.velocity.Y == 0)
            {
                charges = 3;
            }

            player.fullRotationOrigin = player.Size / 2;

            if (leanTimer-- > 0)
            {
                flipDegrees += 0.2f * Math.Sign(flipDegrees);
                player.fullRotation = flipDegrees;
            }
            else if (Math.Abs(flipDegrees) > 0.1f && leanTimer <= 0)
            {
                player.fullRotation = flipDegrees;
                flipDegrees -= 0.1f * Math.Sign(flipDegrees);
            }
            else
            {
                player.fullRotation = 0;
            }

            if (runTimer-- > 0)
            {
                player.GetModPlayer<JetstriderPlayer>().Running = true;
                player.velocity = direction;
                if (direction.X != 0)
                    direction.Y *= 0.95f;
            }
            else
                player.GetModPlayer<JetstriderPlayer>().Running = false;
        }
    }
    public class JetstriderPlayer : ModPlayer
    {

        public bool Running;
        public int runTicker = 0;

        private int legFrameCounter = 7;
        public override void PreUpdate()
        {
            
        }

        public override void PostUpdate()
        {
            if (Running)
            {
                runTicker++;
                if (runTicker >= (10 - Player.velocity.Length()) * 2)
                {
                    legFrameCounter++;
                    if (legFrameCounter > 17)
                        legFrameCounter = 7;
                    runTicker = 0;
                }
                Player.legFrame = new Rectangle(0, 56 * legFrameCounter, 40, 56);
            }
            base.PostUpdate();
        }
    }
}