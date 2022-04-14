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
    public class DrawLensFlares : ModSystem
    {
        internal readonly Vector2 _sunPos;
        internal readonly float _globalAlpha;
        internal readonly float _intensityFunction;

        internal static DrawLensFlares Instance;

        public void UpdateLensFlares()
        {
            if (EEModConfigClient.Instance.BetterLighting && Main.worldName != KeyID.CoralReefs)
            {
                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare2").Value, _sunPos - Main.screenPosition + new Vector2(-400, 400), new Rectangle(0, 0, 174, 174), Color.White * .7f * _globalAlpha * (_intensityFunction * 0.36f), 0f, new Vector2(87), 1f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare2").Value, _sunPos - Main.screenPosition + new Vector2(-800, 800), new Rectangle(0, 0, 174, 174), Color.White * .8f * _globalAlpha * (_intensityFunction * 0.36f), 0f, new Vector2(87), .5f, SpriteEffects.None, 0);
            }
        }

        public override void PostDrawTiles()
        {
            UpdateLensFlares();
        }

        public override void Load()
        {
            Instance = this;
        }
    }
}