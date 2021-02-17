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

namespace EEMod
{
    public class LightingMasks : Mechanic
    {
        internal readonly List<Vector2> _lightPoints = new List<Vector2>();
        internal readonly List<Color> _colorPoints = new List<Color>();

        internal static LightingMasks Instance;
        public void UpdateLight()
        {/*
            for (int i = 0; i < EEMod.maxNumberOfLights; i++)
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[$"EEMod:LightSource{i}"].IsActive())
                {
                    Filters.Scene.Deactivate($"EEMod:LightSource{i}");
                }
            }

            for (int i = 0; i < EEMod.maxNumberOfLights; i++)
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[$"EEMod:LightSource{i}"].IsActive())
                {
                    Filters.Scene.Activate($"EEMod:LightSource{i}", Vector2.Zero).GetShader().UseIntensity(0f);
                }
            }

            List<Vector2> listTransformable = new List<Vector2>();

            for (int i = 0; i < _lightPoints.Count; i++)
            {
                listTransformable.Add((_lightPoints[i] * 16 - Main.screenPosition) / new Vector2(Main.screenWidth, Main.screenHeight));

                if (i < EEMod.maxNumberOfLights)
                {
                    Helpers.DrawAdditive(ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradient"), (_lightPoints[i] * 16).ForDraw(), _colorPoints[i] * 0.2f, 0.5f);
                    //Filters.Scene[$"EEMod:LightSource{i}"].GetShader().UseImageOffset(listTransformable[i]).UseIntensity(0.0045f).UseColor(_colorPoints[i]);
                }
            }

            _lightPoints.Clear();
            _colorPoints.Clear();
            listTransformable.Clear();*/
        }
        public override void OnDraw()
        {
           // ModContent.GetInstance<EEMod>().TVH.Draw(Main.spriteBatch);
        }

        public override void OnUpdate()
        {
           // UpdateLight();
        }

        public override void OnLoad()
        {
            Instance = this;
        }
        protected override Layer DrawLayering => Layer.BehindTiles;
    }
}