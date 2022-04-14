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
using EEMod.Extensions;
using ReLogic.Content;
using EEMod.Seamap.Core;
using EEMod.Seamap.Content.Cannonballs;

namespace EEMod.Seamap.Content.Enemies
{
    public class PirateShip : SeamapObject
    {
        public PirateShip(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 74;
            height = 68;

            alpha = 1f;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/Enemies/PirateShip", AssetRequestMode.ImmediateLoad).Value;
        }

        public override bool collides => true;

        public int ticker;
        public override void Update()
        {
            ticker++;

            position += velocity - (Seamap.Core.Seamap.windVector * 0.2f);
            velocity += (Vector2.Normalize(SeamapObjects.localship.Center - Center) * 0.1f);

            if (velocity.LengthSquared() >= 3f * 3f) velocity = Vector2.Normalize(velocity) * 3f;

            if (ticker % 120 == 0)
            {
                //SeamapObjects.NewSeamapObject(new EnemyCannonball(Center, Vector2.Normalize(SeamapObjects.localship.Center - Center) * 2.5f));
            }

            //base.Update();

            oldPosition = position;
            oldVelocity = velocity;
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            ShipyardPlayer eePlayer = Main.LocalPlayer.GetModPlayer<ShipyardPlayer>();

            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/PirateShip", AssetRequestMode.ImmediateLoad).Value;

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                null, Color.White * (1 - (eePlayer.cutSceneTriggerTimer / 180f)),
                velocity.X / 10, new Rectangle(0, 0, width, height).Size() / 2,
                1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            return false;
        }
    }
}
