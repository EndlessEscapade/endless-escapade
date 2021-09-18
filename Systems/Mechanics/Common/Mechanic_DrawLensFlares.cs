using EEMod.Extensions;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EEMod.ID;
using EEMod.Config;
using EEMod.Systems;

namespace EEMod
{
    public class DrawLensFlares : Mechanic
    {
        internal readonly Vector2 _sunPos;
        internal readonly float _globalAlpha;
        internal readonly float _intensityFunction;

        internal static DrawLensFlares Instance;

        public void UpdateLensFlares(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            if (EEModConfigClient.Instance.BetterLighting && Main.worldName != KeyID.CoralReefs)
            {
                spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare2").Value, _sunPos - Main.screenPosition + new Vector2(-400, 400), new Rectangle(0, 0, 174, 174), Color.White * .7f * _globalAlpha * (_intensityFunction * 0.36f), 0f, new Vector2(87), 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare2").Value, _sunPos - Main.screenPosition + new Vector2(-800, 800), new Rectangle(0, 0, 174, 174), Color.White * .8f * _globalAlpha * (_intensityFunction * 0.36f), 0f, new Vector2(87), .5f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void OnDraw(SpriteBatch spriteBatch)
        {
            UpdateLensFlares(spriteBatch);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnLoad()
        {
            Instance = this;
        }

        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}