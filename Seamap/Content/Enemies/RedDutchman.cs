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
    public class RedDutchman : SeamapObject
    {
        public RedDutchman(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            position = pos;
            velocity = vel;

            width = 74;
            height = 68;

            alpha = 1f;

            texture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/Enemies/RedDutchman", AssetRequestMode.ImmediateLoad).Value;
        }

        public int ticker;
        public override void Update()
        {
            ticker++;

            velocity = Vector2.Normalize(SeamapObjects.localship.Center - Center) * 1.5f;

            if (ticker % 120 == 0)
            {
                SeamapObjects.NewSeamapObject(new EnemyCannonball(Center, Vector2.Normalize(SeamapObjects.localship.Center - Center) * 2.5f));
            }

            base.Update();
        }

        public override bool PreDraw(SpriteBatch spriteBatch)
        {
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

            Texture2D playerShipTexture = ModContent.Request<Texture2D>("EEMod/Seamap/Content/RedDutchman", AssetRequestMode.ImmediateLoad).Value;

            spriteBatch.Draw(playerShipTexture, Center - Main.screenPosition,
                null, Color.White * (1 - (eePlayer.cutSceneTriggerTimer / 180f)),
                velocity.X / 10, new Rectangle(0, 0, width, height).Size() / 2,
                1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            return false;
        }
    }
}
