using System;
using System.Linq;
using EndlessEscapade.Content.Seamap.Islands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace EndlessEscapade.Common.Seamap;

public partial class Seamap
{
    public static int seamapWidth = 5000;
    public static int seamapHeight = 5000;

    public static float brightness;

    public static float weatherDensity;

    public static float islandTextValue;
    public static Island lastIsland;

    public static Vector2 permaWindVector;

    public static void Render() {
        var spriteBatch = Main.spriteBatch;

        CalculateBrightness();

        if (Main.LocalPlayer.GetModPlayer<SeamapPlayer>().seamapUpdateCount <= 0 ||
            (Main.LocalPlayer == null && !Main.gameInactive)) {
            var blackout = ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/Pure").Value;

            spriteBatch.Begin();

            spriteBatch.Draw(blackout, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);

            spriteBatch.End();
        }

        spriteBatch.Begin();

        RenderWater(spriteBatch); //Layer 0

        //Custom rendering primitives for Seamap

        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        RenderEntities(spriteBatch); //Layer 1, postdraw layer 2

        if (!Main.dedServ) {
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        RenderClouds(spriteBatch); //Layer 3


        if (!Main.hideUI) {
            RenderSeamapUI(spriteBatch); //Layer 4
        }

        spriteBatch.End();
    }

    public static void RenderSeamapUI(SpriteBatch spriteBatch) {
        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

        #region Rendering Disembark

        var anyIslands = false;

        foreach (var obj in SeamapObjects.SeamapEntities) {
            if (obj is Island && (obj as Island).cancollide) {
                var island = obj as Island;
                if ( /*island.Hitbox.Intersects(SeamapObjects.localship.Hitbox) && */
                    Vector2.DistanceSquared(SeamapObjects.localship.Hitbox.Center.ToVector2(), obj.Center) <
                    (obj as Island).interactDistance * (obj as Island).interactDistance) {
                    //Main.spriteBatch.DrawString
                    var textSize = FontAssets.MouseText.Value.MeasureString("Disembark?");
                    var textPositionLeft = textSize.X / 2;

                    anyIslands = true;
                    lastIsland = island;

                    ChatManager.DrawColorCodedString(Main.spriteBatch,
                        FontAssets.MouseText.Value,
                        "Disembark?",
                        island.Center -
                        new Vector2(textPositionLeft, island.height / 2 + 40) -
                        Main.screenPosition +
                        new Vector2(0, (float)Math.Sin(Main.GameUpdateCount / 15f) * 5f),
                        Color.White * islandTextValue,
                        0f,
                        Vector2.Zero,
                        Vector2.One);
                }
            }
        }

        if (anyIslands) {
            islandTextValue += 0.04f;
            islandTextValue = MathHelper.Clamp(islandTextValue, 0f, 1f);
        }
        else {
            if (islandTextValue > 0f) {
                var textSize = FontAssets.MouseText.Value.MeasureString("Disembark?");
                var textPositionLeft = textSize.X / 2;

                ChatManager.DrawColorCodedString(Main.spriteBatch,
                    FontAssets.MouseText.Value,
                    "Disembark?",
                    lastIsland.Center -
                    new Vector2(textPositionLeft, lastIsland.height / 2 + 40) -
                    Main.screenPosition +
                    new Vector2(0, (float)Math.Sin(Main.GameUpdateCount / 15f) * 5f),
                    Color.White * islandTextValue,
                    0f,
                    Vector2.Zero,
                    Vector2.One);
            }

            islandTextValue -= 0.08f;
            islandTextValue = MathHelper.Clamp(islandTextValue, 0f, 1f);
        }

        #endregion

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);

        //TODO: move this to an actual UI

        #region Rendering OCEAN screen thingy

        if (Main.LocalPlayer.GetModPlayer<SeamapPlayer>().seamapUpdateCount > 10 && Main.LocalPlayer.GetModPlayer<SeamapPlayer>().seamapUpdateCount <= 190) {
            var updateCount = Main.LocalPlayer.GetModPlayer<SeamapPlayer>().seamapUpdateCount;

            var oceanLogo = ModContent.Request<Texture2D>("EndlessEscapade/Content/Seamap/UI/OceanScreen").Value;

            var yOffset = updateCount <= 70 ? (float)Math.Sin((updateCount - 10) * 1.57f / 60f) * 240f - 120 :
                updateCount <= 130 ? 120 : (float)Math.Sin((updateCount - 70) * 1.57f / 60f) * 240f - 120;

            spriteBatch.Draw(oceanLogo, new Vector2(Main.screenWidth / 2, yOffset), null, Color.White, 0, new Vector2(186, 92), 1, SpriteEffects.None, 0);
        }

        #endregion
    }

    public static void RenderEntities(SpriteBatch spriteBatch) {
        static int CompareSeamapEntities(SeamapObject a, SeamapObject b) {
            return a?.Bottom.Y.CompareTo(b?.Bottom.Y ?? 0f) ?? 0;
        }
        //static bool InactiveOrUnused(SeamapObject obj) => obj?.active != true;

        var source = SeamapObjects.SeamapEntities;

        var toDraw = source.Where(p => p?.active == true).ToArray();
        Array.Sort(toDraw, CompareSeamapEntities);

        foreach (var entity in toDraw) {
            entity.Draw(spriteBatch);
        }

        foreach (var entity in toDraw) {
            entity.PostDraw(spriteBatch);
        }
    }

    private static void RenderWater(SpriteBatch spriteBatch) {
        //EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        var waterTexture = ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/Pure").Value;

        var pos = Vector2.Zero;
        var toScreen = pos - Main.screenPosition;

        //Neutral water palette
        var WaterShaderBase = EndlessEscapade.Instance.Assets.Request<Effect>("Assets/Effects/WaterShaderBase", AssetRequestMode.ImmediateLoad).Value;

        WaterShaderBase.Parameters["icyWaterColor"]
            .SetValue(Vector4.Lerp(new Color(52, 75, 136).LightSeamap().ToVector4(), new Color(34, 30, 45).LightSeamap().ToVector4(), weatherDensity));
        WaterShaderBase.Parameters["neutralWaterColor"]
            .SetValue(Vector4.Lerp(new Color(36, 119, 182).LightSeamap().ToVector4(), new Color(44, 44, 68).LightSeamap().ToVector4(), weatherDensity));
        WaterShaderBase.Parameters["tropicalWaterColor"]
            .SetValue(Vector4.Lerp(new Color(96, 178, 220).LightSeamap().ToVector4(), new Color(53, 65, 77).LightSeamap().ToVector4(), weatherDensity));

        WaterShaderBase.Parameters["densityNoisemap"].SetValue(ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/Noise/SeamapNoise").Value);

        WaterShaderBase.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Draw(waterTexture,
            new Rectangle((int)toScreen.X, (int)toScreen.Y, seamapWidth, seamapHeight),
            null,
            Color.White,
            0f,
            Vector2.Zero,
            SpriteEffects.None,
            0f);

        spriteBatch.End();
        //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        /*foreach (var entity in SeamapObjects.ActiveEntities) {
            if (entity is not Island)
                continue;

            Helpers.DrawAdditive(ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/RadialGradientSquish").Value, entity.Center - Main.screenPosition, Color.Lerp(new Color(96, 178, 220).LightSeamap(), new Color(53, 65, 77).LightSeamap(), weatherDensity) * 0.4f, entity.texture.Width * 2f / 150f);
        }*/

        //spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        var WaterShader = EndlessEscapade.Instance.Assets.Request<Effect>("Assets/Effects/WaterShader", AssetRequestMode.ImmediateLoad).Value;

        WaterShader.Parameters["noiseTex"].SetValue(ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/Noise/DotNoise2SquishIndex").Value);

        WaterShader.Parameters["baseWaterColor"].SetValue(new Color(0, 0, 0).LightSeamap().ToVector4());
        WaterShader.Parameters["highlightColor"].SetValue(new Color(5, 5, 5).LightSeamap().ToVector4()); //8,8,8 for storms

        WaterShader.Parameters["sinVal"].SetValue(Main.GameUpdateCount / 1500f); // divided by 1000 for storms

        WaterShader.Parameters["width"].SetValue(1000);
        WaterShader.Parameters["height"].SetValue(600);

        WaterShader.CurrentTechnique.Passes[0].Apply();

        for (var i = 0; i < seamapWidth / 1000; i++) {
            for (float j = 0; j < seamapHeight / 1000; j += 0.6f) {
                var arrayOffset = new Vector2(i, j);

                spriteBatch.Draw(waterTexture,
                    new Rectangle((int)toScreen.X + i * 1000, (int)toScreen.Y + (int)(j * 1000), 1000, 600),
                    new Color(0.1f, 0.1f, 0.1f, 0.1f).LightSeamap());
            }
        }

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
    }

    private static void RenderClouds(SpriteBatch spriteBatch) {
        spriteBatch.End();

        var waterTexture = ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/Pure").Value;

        var SeaColour = new Color(28 / 255f, 118 / 255f, 186 / 255f);

        var pos = Vector2.Zero;
        var toScreen = pos - Main.screenPosition;

        if (Main.raining && weatherDensity < 1f) {
            weatherDensity += 0.001f;
        }

        if (!Main.raining && weatherDensity > 0f) {
            weatherDensity -= 0.001f;
        }

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        var SeamapCloudShader = EndlessEscapade.Instance.Assets.Request<Effect>("Assets/Effects/SeamapCloudShader", AssetRequestMode.ImmediateLoad).Value;

        SeamapCloudShader.Parameters["cloudNoisemap"].SetValue(ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/Noise/CloudNoise").Value);
        SeamapCloudShader.Parameters["densityNoisemap"].SetValue(ModContent.Request<Texture2D>("EndlessEscapade/Assets/Textures/Noise/SeamapNoise").Value);

        SeamapCloudShader.Parameters["cloudsColor4"].SetValue((Color.Black * (0.1f + weatherDensity * 0.1f)).ToVector4());
        SeamapCloudShader.Parameters["cloudsColor3"].SetValue((Color.Black * (0.1f + weatherDensity * 0.1f)).ToVector4());
        SeamapCloudShader.Parameters["cloudsColor2"].SetValue((Color.Black * (0.1f + weatherDensity * 0.1f)).ToVector4());
        SeamapCloudShader.Parameters["cloudsColor1"].SetValue((Color.Black * (0.1f + weatherDensity * 0.1f)).ToVector4());

        var tempWindVector = permaWindVector / 4800f;

        if (tempWindVector.Y < 0) {
            tempWindVector.Y = 1 + tempWindVector.Y;
        }

        if (tempWindVector.X < 0) {
            tempWindVector.X = 1 + tempWindVector.X;
        }

        SeamapCloudShader.Parameters["wind"].SetValue(tempWindVector);

        SeamapCloudShader.Parameters["weatherDensity"].SetValue(weatherDensity);
        SeamapCloudShader.Parameters["stepsX"].SetValue(5f);
        SeamapCloudShader.Parameters["stepsY"].SetValue(4.8f);

        SeamapCloudShader.Parameters["vec"].SetValue(new Vector2(1000, 600));

        for (var i = 0; i < seamapWidth / 1000; i++) {
            for (var j = -0.6f; j < seamapHeight / 1000; j += 0.6f) {
                var arrayOffset = new Vector2(i, j);

                SeamapCloudShader.Parameters["arrayOffset"].SetValue(arrayOffset);
                SeamapCloudShader.CurrentTechnique.Passes[0].Apply();

                spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X + i * 1000, (int)toScreen.Y + (int)(j * 1000) + 100, 1000, 600), Color.White);
            }
        }


        SeamapCloudShader.Parameters["cloudsColor4"]
            .SetValue(Vector4.Lerp((new Color(218, 221, 237).LightSeamap() * 1f).ToVector4(),
                (new Color(142, 143, 156).LightSeamap() * 1f).ToVector4(),
                weatherDensity));
        SeamapCloudShader.Parameters["cloudsColor3"]
            .SetValue(Vector4.Lerp((new Color(153, 195, 245).LightSeamap() * 0.9f).ToVector4(),
                (new Color(92, 117, 162).LightSeamap() * 0.9f).ToVector4(),
                weatherDensity));
        SeamapCloudShader.Parameters["cloudsColor2"]
            .SetValue(Vector4.Lerp((new Color(153, 195, 245).LightSeamap() * 0.75f).ToVector4(),
                (new Color(85, 104, 133).LightSeamap() * 0.9f).ToVector4(),
                weatherDensity));
        SeamapCloudShader.Parameters["cloudsColor1"]
            .SetValue(Vector4.Lerp((new Color(138, 169, 201).LightSeamap() * 0.7f).ToVector4(),
                (new Color(49, 76, 119).LightSeamap() * 0.9f).ToVector4(),
                weatherDensity));

        for (var i = 0; i < seamapWidth / 1000; i++) {
            for (var j = -0.6f; j < seamapHeight / 1000; j += 0.6f) {
                var arrayOffset = new Vector2(i, j);

                SeamapCloudShader.Parameters["arrayOffset"].SetValue(arrayOffset);
                SeamapCloudShader.CurrentTechnique.Passes[0].Apply();

                spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X + i * 1000, (int)toScreen.Y + (int)(j * 1000), 1000, 600), Color.White);
            }
        }

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        var SeamapBorderVignette = EndlessEscapade.Instance.Assets.Request<Effect>("Assets/Effects/SeamapBorderVignette", AssetRequestMode.ImmediateLoad).Value;

        SeamapBorderVignette.Parameters["color"]
            .SetValue(Vector4.Lerp(Color.White.LightSeamap().ToVector4(), Color.Gray.LightSeamap().ToVector4(), weatherDensity));

        SeamapBorderVignette.CurrentTechnique.Passes[0].Apply();

        spriteBatch.Draw(waterTexture, new Rectangle((int)toScreen.X, (int)toScreen.Y, 5000, 4800), Color.White);

        spriteBatch.End();
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
    }

    private static void CalculateBrightness() {
        if (Main.LocalPlayer.GetModPlayer<SeamapPlayer>().seamapUpdateCount == 1) {
            brightness = Main.dayTime ? Main.raining ? 0.5f : 1f : Main.raining ? 0.5f : 0.2f;
        }

        if (!Main.raining) {
            if (Main.dayTime) {
                if (brightness < 1f) {
                    brightness += 0.0025f;
                }
                else if (brightness > 0.5f) {
                    brightness -= 0.0025f;
                }
            }
        }
        else {
            if (Main.dayTime) {
                if (brightness < 0.5f) {
                    brightness += 0.0025f;
                }
                else if (brightness > 0.2f) {
                    brightness -= 0.0025f;
                }
            }
        }
    }
}

public static class SeamapExtensions
{
    public static Color LightSeamap(this Color color) {
        return Color.Lerp(color, Color.Black, 1f - Seamap.brightness);
    }
}
