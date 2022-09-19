using EEMod.Autoloading;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Prim;
using EEMod.Projectiles;
using EEMod.Items.Weapons.Mage;
using EEMod.Tiles;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;
using EEMod.Seamap.Core;
using EEMod.Seamap.Content;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.UI.Chat;
using EEMod.Systems;
using Terraria.GameContent;
using EEMod.NPCs.Glowshroom;
using EEMod.Players;
using System.Diagnostics;
using EEMod.Subworlds;
using EEMod.NPCs.Goblins.Scrapwizard;
using EEMod.Subworlds.CoralReefs;
using SubworldLibrary;
using System.Reflection;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria.GameContent.Liquid;
using Terraria.Utilities;
using Terraria.Graphics.Light;
using Terraria.GameContent.Shaders;

namespace EEMod
{
    internal delegate void MechanicDrawDelegate(SpriteBatch spriteBatch);

    public partial class EEMod
    {
        internal event MechanicDrawDelegate BeforeTiles;
        internal event MechanicDrawDelegate AfterTiles;

        internal event MechanicDrawDelegate BeforeNPCCache;

        public List<IComponent> Updatables = new List<IComponent>();

        float bgAlpha;

        public bool wasDoingWorldGen;

        private void LoadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float += RegisterLightPoint;

            On.Terraria.Main.Draw += ManageWorldLoadUI;
            On.Terraria.Main.DrawProjectiles += RenderSeamap;
            On.Terraria.Main.DrawWoF += RenderBehindTiles;
            On.Terraria.Main.CacheNPCDraws += PreRenderNPCs;

            On.Terraria.Main.DrawWater += WaterAlphaMod;

            On.Terraria.GameContent.Drawing.TileDrawing.DrawPartialLiquid += TileDrawing_DrawPartialLiquid;
            //On.Terraria.GameContent.Liquid.LiquidRenderer.InternalPrepareDraw += LiquidRenderer_InternalPrepareDraw;

            On.Terraria.Main.DrawBlack += Main_DrawBlack;
            //On.Terraria.Main.DoDraw_WallsAndBlacks += Main_DoDraw_WallsAndBlacks1;

            //On.Terraria.Main.DrawWoF += Main_DrawWoF;
            On.Terraria.Main.DoDraw_UpdateCameraPosition += RenderPrimitives;

            //On.Terraria.Main.DoDraw_WallsAndBlacks += Main_DoDraw_WallsAndBlacks;

            //On.Terraria.Main.DoDraw_WallsAndBlacks += DrawGoblinFortBg;

            //On.Terraria.Main.DoDraw_Tiles_NonSolid += DrawGoblinFortBg;

            On.Terraria.UI.IngameFancyUI.Draw += DisableFancyUIOnSeamap;

            On.Terraria.Main.Draw += DrawLoadingScreen;

            On.Terraria.Player.Update_NPCCollision += GoblinTableCollision;

            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += PrepareSubworldList;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += RenderSubworldList;

            On.Terraria.WorldGen.SaveAndQuitCallBack += ManageSaving;

            Main.OnPreDraw += PreparePrimitives;
            Main.OnPreDraw += PrepLoadingScreen;

            if (Main.dedServ)
                return;
        }

        private void UnloadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float -= RegisterLightPoint;

            On.Terraria.Main.Draw -= ManageWorldLoadUI;
            On.Terraria.Main.DrawProjectiles -= RenderSeamap;
            On.Terraria.Main.DrawWoF -= RenderBehindTiles;
            On.Terraria.Main.CacheNPCDraws -= PreRenderNPCs;
            //On.Terraria.Main.DoDraw_Tiles_Solid -= DrawCoralReefsBg;
            On.Terraria.Main.DoDraw_UpdateCameraPosition -= RenderPrimitives;

            On.Terraria.Main.DrawBlack -= Main_DrawBlack;

            //On.Terraria.Main.DoDraw_WallsAndBlacks -= Main_DoDraw_WallsAndBlacks;

            //On.Terraria.GameContent.Liquid.LiquidRenderer.InternalPrepareDraw -= LiquidRenderer_InternalPrepareDraw;

            On.Terraria.GameContent.Drawing.TileDrawing.DrawPartialLiquid -= TileDrawing_DrawPartialLiquid;

            On.Terraria.Main.DoDraw_WallsAndBlacks -= DrawGoblinFortBg;

            On.Terraria.UI.IngameFancyUI.Draw -= DisableFancyUIOnSeamap;

            On.Terraria.Main.DoDraw_WallsAndBlacks += DrawGoblinFortBg;

            On.Terraria.Main.Draw -= DrawLoadingScreen;

            On.Terraria.Player.Update_NPCCollision -= GoblinTableCollision;

            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor -= PrepareSubworldList;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf -= RenderSubworldList;

            On.Terraria.WorldGen.SaveAndQuitCallBack -= ManageSaving;

            Main.OnPreDraw -= PreparePrimitives;
            Main.OnPreDraw -= PrepLoadingScreen;
        }


        private void Main_DrawBlack(On.Terraria.Main.orig_DrawBlack orig, Main self, bool force)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Vector2 value = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            int num = (Main.tileColor.R + Main.tileColor.G + Main.tileColor.B) / 3;
            float num2 = (float)((double)num * 0.4) / 255f;
            if (Lighting.Mode == LightMode.Retro)
            {
                num2 = (float)(Main.tileColor.R - 55) / 255f;
                if (num2 < 0f)
                {
                    num2 = 0f;
                }
            }
            else if (Lighting.Mode == LightMode.Trippy)
            {
                num2 = (float)(num - 55) / 255f;
                if (num2 < 0f)
                {
                    num2 = 0f;
                }
            }
            Microsoft.Xna.Framework.Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();
            Microsoft.Xna.Framework.Point point = new Microsoft.Xna.Framework.Point(-Main.offScreenRange / 16 + screenOverdrawOffset.X, -Main.offScreenRange / 16 + screenOverdrawOffset.Y);
            int num3 = (int)((Main.screenPosition.X - value.X) / 16f - 1f) + point.X;
            int num4 = (int)((Main.screenPosition.X + (float)Main.screenWidth + value.X) / 16f) + 2 - point.X;
            int num5 = (int)((Main.screenPosition.Y - value.Y) / 16f - 1f) + point.Y;
            int num6 = (int)((Main.screenPosition.Y + (float)Main.screenHeight + value.Y) / 16f) + 5 - point.Y;
            if (num3 < 0)
            {
                num3 = point.X;
            }
            if (num4 > Main.maxTilesX)
            {
                num4 = Main.maxTilesX - point.X;
            }
            if (num5 < 0)
            {
                num5 = point.Y;
            }
            if (num6 > Main.maxTilesY)
            {
                num6 = Main.maxTilesY - point.Y;
            }
            if (!force)
            {
                if (num5 < Main.maxTilesY / 2)
                {
                    num6 = Math.Min(num6, (int)Main.worldSurface + 1);
                    num5 = Math.Min(num5, (int)Main.worldSurface + 1);
                }
                else
                {
                    num6 = Math.Max(num6, Main.UnderworldLayer);
                    num5 = Math.Max(num5, Main.UnderworldLayer);
                }
            }
            for (int i = num5; i < num6; i++)
            {
                bool flag = i >= Main.UnderworldLayer;
                if (flag)
                {
                    num2 = 0.2f;
                }
                for (int j = num3; j < num4; j++)
                {
                    int num7 = j;
                    for (; j < num4; j++)
                    {
                        if (!WorldGen.InWorld(j, i))
                        {
                            return;
                        }
                        if (Main.tile[j, i] == null)
                        {
                            Main.tile[j, i].ClearEverything();
                        }
                        Tile tile = Main.tile[j, i];
                        float num8 = Lighting.Brightness(j, i);
                        num8 = (float)Math.Floor(num8 * 255f) / 255f;
                        byte b = tile.LiquidAmount;
                        if (!(num8 <= num2) || ((flag || b >= 250) && !WorldGen.SolidTile(tile) && (b < 200 || num8 != 0f)) || (WallID.Sets.Transparent[tile.WallType] && (!Main.tile[j, i].HasTile || !Main.tileBlockLight[(int)tile.BlockType])) || Framing.GetTileSafely(j, i - 1).LiquidAmount > 0 || Framing.GetTileSafely(j, i + 1).LiquidAmount > 0 || Framing.GetTileSafely(j + 1, i).LiquidAmount > 0 || Framing.GetTileSafely(j - 1, i).LiquidAmount > 0 || (tile.IsHalfBlock && Framing.GetTileSafely(j - 1, i).LiquidAmount > 0 && Lighting.Brightness(j, i) > 0f) || (!Main.drawToScreen && LiquidRenderer.Instance.HasFullWater(j, i) && tile.WallType == 0 && !tile.IsHalfBlock && !((double)i <= Main.worldSurface)))
                        {
                            break;
                        }
                    }
                    if (j - num7 > 0)
                    {
                        Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(num7 << 4, i << 4) - Main.screenPosition + value, new Microsoft.Xna.Framework.Rectangle(0, 0, j - num7 << 4, 16), Microsoft.Xna.Framework.Color.Black);
                    }
                }
            }
            TimeLogger.DrawTime(5, stopwatch.Elapsed.TotalMilliseconds);
        }

        private unsafe void TileDrawing_DrawPartialLiquid(On.Terraria.GameContent.Drawing.TileDrawing.orig_DrawPartialLiquid orig, Terraria.GameContent.Drawing.TileDrawing self, Tile tileCache, Vector2 position, Rectangle liquidSize, int liquidType, Color aColor)
        {
            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

            Vector2 rectangle = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                rectangle = Vector2.Zero;
            }

            Vector2 archivePos = position;

            position += Main.screenPosition;
            position -= rectangle;

            Vector2 drawOffset = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) - Main.screenPosition;

            int i = (int)((position.X) / 16f);
            int j = (int)((position.Y) / 16f);

            int slope = (int)tileCache.Slope;
            if (!TileID.Sets.BlocksWaterDrawingBehindSelf[(int)tileCache.BlockType] || slope == 0)
            {
                Main.tileBatch.Begin();

                Main.DrawTileInWater(drawOffset, (int)((position.X) / 16f), (int)((position.Y) / 16f));

                Lighting.GetCornerColors(i, j, out VertexColors vertices);

                float opacity = Main.tile[i, j].LiquidType == 0 ? 0.65f : 0.95f;

                Color color1 = vertices.BottomLeftColor;
                Color color2 = vertices.BottomRightColor;
                Color color3 = vertices.TopLeftColor;
                Color color4 = vertices.TopRightColor;

                if (SubworldSystem.IsActive<CoralReefs>())
                {
                    vertices.BottomLeftColor = aColor * opacity * Main.liquidAlpha[Main.waterStyle] * (color1.A / 255f);
                    vertices.BottomRightColor = aColor * opacity * Main.liquidAlpha[Main.waterStyle] * (color2.A / 255f);
                    vertices.TopLeftColor = aColor * opacity * Main.liquidAlpha[Main.waterStyle] * (color3.A / 255f);
                    vertices.TopRightColor = aColor * opacity * Main.liquidAlpha[Main.waterStyle] * (color4.A / 255f);
                }
                else
                {
                    vertices.BottomLeftColor = aColor * Main.liquidAlpha[Main.waterStyle] * (color1.A / 255f);
                    vertices.BottomRightColor = aColor * Main.liquidAlpha[Main.waterStyle] * (color2.A / 255f);
                    vertices.TopLeftColor = aColor * Main.liquidAlpha[Main.waterStyle] * (color3.A / 255f);
                    vertices.TopRightColor = aColor * Main.liquidAlpha[Main.waterStyle] * (color4.A / 255f);
                }


                Main.DrawTileInWater(drawOffset, i, j);

                int frameYOffset = 48;
                if (Framing.GetTileSafely(i, j - 1).LiquidAmount == 0 && !Framing.GetTileSafely(i, j - 1).HasTile)
                {
                    frameYOffset = 0;
                }

                Rectangle sourceRectangle = new Rectangle(16, frameYOffset + (int)typeof(LiquidRenderer).GetField("_animationFrame", flags).GetValue(LiquidRenderer.Instance) * 80, 16, 16);

                /*Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, 
                    new Vector4(archivePos.X, archivePos.Y, liquidSize.Width, liquidSize.Height),
                    sourceRectangle, 
                    vertices, default(Vector2), SpriteEffects.None, 0f);*/

                if (Main.tile[i, j].IsHalfBlock)
                    Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, archivePos + new Vector2(0, 8), liquidSize, vertices, default(Vector2), 1f, SpriteEffects.None);
                else
                    Main.tileBatch.Draw(TextureAssets.Liquid[liquidType].Value, archivePos, liquidSize, vertices, default(Vector2), 1f, SpriteEffects.None);

                Main.tileBatch.End();

                return;
            }

            liquidSize.X += 18 * (slope - 1);

            if (tileCache.Slope == (SlopeType)1 || tileCache.Slope == (SlopeType)2 || tileCache.Slope == (SlopeType)3 || tileCache.Slope == (SlopeType)4)
            {
                Main.spriteBatch.Draw(TextureAssets.LiquidSlope[liquidType].Value, archivePos, liquidSize, aColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        private void WaterAlphaMod(On.Terraria.Main.orig_DrawWater orig, Main self, bool bg, int Style, float Alpha)
        {
            orig(self, bg, Style, SubworldSystem.IsActive<CoralReefs>() ? Alpha / 3.5f : Alpha);
        }

        private void Main_DoDraw_WallsAndBlacks(On.Terraria.Main.orig_DoDraw_WallsAndBlacks orig, Main self)
        {
            if (SubworldSystem.IsActive<CoralReefs>() && !Main.gameMenu)
            {
                if (Main.LocalPlayer.Center.Y >= ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
                {
                    bgAlpha += 0.01f;
                }
                else
                {
                    bgAlpha -= 0.01f;
                }

                bgAlpha = 0;

                bgAlpha = MathHelper.Clamp(bgAlpha, 0, 1);

                if (bgAlpha > 0)
                {
                    Texture2D tex = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceFar").Value;
                    Texture2D tex2 = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceMid").Value;
                    Texture2D tex3 = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceClose").Value;

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default);

                    ModContent.GetInstance<LightingBuffer>().PostDrawTiles();

                    Vector2 chunk1 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.8f, 0.3f)) / tex.Size();
                    Vector2 chunk2 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.6f, 0.3f)) / tex2.Size();
                    Vector2 chunk3 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.4f, 0.3f)) / tex3.Size();

                    for (int i = (int)chunk1.X - 1; i <= (int)chunk1.X + 1; i++)
                        for (int j = (int)chunk1.Y - 1; j <= (int)chunk1.Y + 1; j++)
                            global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                            tex,
                            new Vector2(tex.Width * i, tex.Height * j).ParalaxXY(new Vector2(-0.8f, -0.3f)), bgAlpha);

                    for (int i = (int)chunk2.X - 1; i <= (int)chunk2.X + 1; i++)
                        for (int j = (int)chunk2.Y - 1; j <= (int)chunk2.Y + 1; j++)
                            global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                            tex2,
                            new Vector2(tex2.Width * i, tex2.Height * j).ParalaxXY(new Vector2(-0.6f, -0.3f)), bgAlpha);

                    for (int i = (int)chunk3.X - 1; i <= (int)chunk3.X + 1; i++)
                        for (int j = (int)chunk3.Y - 1; j <= (int)chunk3.Y + 1; j++)
                            global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                            tex3,
                            new Vector2(tex3.Width * i, tex3.Height * j).ParalaxXY(new Vector2(-0.4f, -0.3f)), bgAlpha);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                }
                else
                {
                    //int a = 2;
                    //SurfaceBackgroundStylesLoader.ChooseStyle(ref a);
                }
            }

            orig(self);
        }

        public Entity loadingEntity;

        public RenderTarget2D loadingScreenRT;

        private void PrepLoadingScreen(GameTime obj)
        {
            if (loadingScreenRT == default)
            {
                loadingScreenRT = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height);
            }

            RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(loadingScreenRT);
            Main.graphics.GraphicsDevice.Clear(Color.Black);

            Main.spriteBatch.Begin();

            DrawLoadingScreenContent();

            Main.spriteBatch.End();

            if (loadingEntity != default)
            {
                (loadingEntity as LoadingShip).Update();

                //PrimitiveSystem.primitives.UpdateTrails();

                //PrimitiveSystem.primitives.DrawTrailsAboveTiles();
            }

            Main.spriteBatch.Begin();

            if (loadingEntity != default) (loadingEntity as LoadingShip).Draw(Main.spriteBatch);

            if ((SubworldSystem.Current as EESubworld) != null)
                EEMod.UIText(EESubworld.progressMessage, Color.White, new Vector2(Main.screenWidth / 2f, Main.screenHeight * 2f / 3f), 0);

            Main.spriteBatch.End();

            Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
        }

        public float leftBound = 1.2f;
        public float rightBound = 1.2f;

        private void DrawLoadingScreen(On.Terraria.Main.orig_Draw orig, Main self, GameTime gameTime)
        {
            orig(self, gameTime);

            if (!Main.gameMenu && !SubworldSystem.IsActive<Sea>() && Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().triggerSeaCutscene) leftBound -= 1.4f / 40f;
            if (!Main.gameMenu && SubworldSystem.IsActive<Sea>() && !Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamap) rightBound -= 1.4f / 40f;
            if (!Main.gameMenu && SubworldSystem.IsActive<Sea>() && Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamap) rightBound += 1.4f / 40f;
            if (!Main.gameMenu && !SubworldSystem.IsActive<Sea>() && !Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().triggerSeaCutscene) leftBound += 1.4f / 40f;

            if(loadingEntity == default)
            {
                loadingEntity = new LoadingShip(new Vector2(Main.graphics.GraphicsDevice.Viewport.Width / 2f, Main.graphics.GraphicsDevice.Viewport.Height / 2f - 200), Vector2.Zero, null);
                loadingEntity.Center = new Vector2(Main.graphics.GraphicsDevice.Viewport.Width / 2f, Main.graphics.GraphicsDevice.Viewport.Height / 2f - 200);
            }

            /*if(loadingEntity != default && !Main.gameMenu && (loadingEntity as LoadingShip).foamTrail == null)
            {
                PrimitiveSystem.primitives.CreateTrail((loadingEntity as LoadingShip).foamTrail = new FoamTrailLoading(loadingEntity, Color.Orange, 0.25f, 260));
            }

            if(leftBound == rightBound)
            {
                (loadingEntity as LoadingShip).foamTrail = null;
            }*/

            leftBound = MathHelper.Clamp(leftBound, -0.2f, 1.2f);
            rightBound = MathHelper.Clamp(rightBound, -0.2f, 1.2f);

            //leftBound = 0.5f;
            //rightBound = -0.2f;
            //rightBound = 1.2f;

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            EEMod.LoadingScreenVeil.Parameters["leftBound"].SetValue(leftBound);
            EEMod.LoadingScreenVeil.Parameters["rightBound"].SetValue(rightBound);

            EEMod.LoadingScreenVeil.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Textures/Black").Value, new Rectangle(0, 0, Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height), Color.White);

            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null);

            EEMod.LoadingScreenVeil.Parameters["leftBound"].SetValue(leftBound);
            EEMod.LoadingScreenVeil.Parameters["rightBound"].SetValue(rightBound);

            EEMod.LoadingScreenVeil.CurrentTechnique.Passes[0].Apply();

            if (loadingScreenRT != null)
                Main.spriteBatch.Draw(loadingScreenRT, new Rectangle(0, 0, Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height), Color.White);

            Main.spriteBatch.End();
        }

        public Vector2[] loadingScreenParticles = new Vector2[30];

        public int constantTicker;

        public bool flipBoat;

        private void DrawLoadingScreenContent()
        {
            constantTicker++;

            Texture2D waterTexture = ModContent.Request<Texture2D>("EEMod/Particles/Square").Value;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);

            SeamapCloudShaderLoading.Parameters["cloudNoisemap"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/CloudNoise").Value);
            SeamapCloudShaderLoading.Parameters["densityNoisemap"].SetValue(ModContent.Request<Texture2D>("EEMod/Textures/Noise/WaveNoiseAlt").Value);

            Vector2 wind = new Vector2((float)Math.Sin(constantTicker / 120f) * 20f, constantTicker * 1f) / 4800f;

            SeamapCloudShaderLoading.Parameters["wind"].SetValue(new Vector2(wind.X % 1, wind.Y % 1));

            SeamapCloudShaderLoading.Parameters["weatherDensity"].SetValue(1.2f);
            SeamapCloudShaderLoading.Parameters["stepsX"].SetValue(1f);
            SeamapCloudShaderLoading.Parameters["stepsY"].SetValue(0.8f);

            SeamapCloudShaderLoading.Parameters["counter"].SetValue((constantTicker / 300f) % MathHelper.TwoPi);

            SeamapCloudShaderLoading.Parameters["vec"].SetValue(new Vector2(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height));

            SeamapCloudShaderLoading.Parameters["cloudsColor4"].SetValue(new Color(13, 22, 36).ToVector4());
            SeamapCloudShaderLoading.Parameters["cloudsColor3"].SetValue(new Color(10, 17, 26).ToVector4());
            SeamapCloudShaderLoading.Parameters["cloudsColor2"].SetValue(new Color(8, 14, 23).ToVector4());
            SeamapCloudShaderLoading.Parameters["cloudsColor1"].SetValue(new Color(7, 9, 20).ToVector4());

            SeamapCloudShaderLoading.Parameters["arrayOffset"].SetValue(0);
            SeamapCloudShaderLoading.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(waterTexture, new Rectangle(0, 0, Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height), Color.White);


            //Rendering the shiny blue particles in the corners

            Texture2D noiseTex = ModContent.Request<Texture2D>("EEMod/Textures/Noise/noise2").Value;

            Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null);

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.75f));

            EEMod.MainParticles.SpawnParticles(new Vector2(0 + Main.rand.Next(0, 300), -10), Vector2.UnitY, 
                ModContent.Request<Texture2D>("EEMod/Particles/SmallCircle").Value, 200, 1, Color.Cyan, 
                new RotateTexture(0.02f), new SetMask(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value, Color.Aqua * 0.6f), new RotateVelocity(0.1f));

            //Main.screenPosition = Vector2.Zero;

            Particles.Update();

            Particles.Draw(Main.spriteBatch);

            Main.spriteBatch.Begin();
            Main.DrawThickCursor();
            Main.DrawCursor(new Vector2(2, 2));
            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);
        }

        private void DrawGoblinFortBg(On.Terraria.Main.orig_DoDraw_WallsAndBlacks orig, Main self)
        {
            orig(self);

            if (SubworldSystem.IsActive<GoblinFort>())
            {
                Texture2D bgTex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/Background").Value;
                Texture2D bgTexGlass = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/BackgroundGlass").Value;

                for (int i = 0; i < bgTex.Width; i += 16)
                {
                    for (int j = 0; j < bgTex.Height; j += 16)
                    {
                        Main.spriteBatch.Draw(bgTex,
                            new Vector2(((SubworldSystem.Current as GoblinFort).hallX * 16) + (24 * 16) + i, ((SubworldSystem.Current as GoblinFort).hallY * 16) + (23 * 16) + j) - Main.screenPosition, new Rectangle(i, j, 16, 16),
                            Lighting.GetColor((int)((((SubworldSystem.Current as GoblinFort).hallX * 16) + (24 * 16) + i) / 16f), (int)((((SubworldSystem.Current as GoblinFort).hallY * 16) + (23 * 16) + j) / 16f)),
                            0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                        Main.spriteBatch.Draw(bgTexGlass,
                            new Vector2(((SubworldSystem.Current as GoblinFort).hallX * 16) + (24 * 16) + i, ((SubworldSystem.Current as GoblinFort).hallY * 16) + (23 * 16) + j) - Main.screenPosition, new Rectangle(i, j, 16, 16),
                            Lighting.GetColor((int)((((SubworldSystem.Current as GoblinFort).hallX * 16) + (24 * 16) + i) / 16f), (int)((((SubworldSystem.Current as GoblinFort).hallY * 16) + (23 * 16) + j) / 16f)) * 0.5f,
                            0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                }

                Vector2 position = new Vector2(((SubworldSystem.Current as GoblinFort).hallX * 16) + (24 * 16), ((SubworldSystem.Current as GoblinFort).hallY * 16) + (23 * 16));

                Main.spriteBatch.Draw(bgTex, position - Main.screenPosition, Color.White);
            }
        }

        private void DrawCoralReefsBg(On.Terraria.Main.orig_DoDraw_Tiles_Solid orig, Main self)
        {
            if (SubworldSystem.IsActive<CoralReefs>() && !Main.gameMenu)
            {
                if (Main.LocalPlayer.Center.Y >= ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
                {
                    bgAlpha += 0.01f;
                }
                else
                {
                    bgAlpha -= 0.01f;
                }

                bgAlpha = MathHelper.Clamp(bgAlpha, 0, 1);

                if (bgAlpha > 0)
                {
                    Texture2D tex = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceFar").Value;
                    Texture2D tex2 = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceMid").Value;
                    Texture2D tex3 = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceClose").Value;

                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default);

                    ModContent.GetInstance<LightingBuffer>().PostDrawTiles();

                    Vector2 chunk1 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.8f, 0.3f)) / tex.Size();
                    Vector2 chunk2 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.6f, 0.3f)) / tex2.Size();
                    Vector2 chunk3 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.4f, 0.3f)) / tex3.Size();

                    for (int i = (int)chunk1.X - 1; i <= (int)chunk1.X + 1; i++)
                        for (int j = (int)chunk1.Y - 1; j <= (int)chunk1.Y + 1; j++)
                            global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                            tex,
                            new Vector2(tex.Width * i, tex.Height * j).ParalaxXY(new Vector2(-0.8f, -0.3f)), bgAlpha);

                    for (int i = (int)chunk2.X - 1; i <= (int)chunk2.X + 1; i++)
                        for (int j = (int)chunk2.Y - 1; j <= (int)chunk2.Y + 1; j++)
                            global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                            tex2,
                            new Vector2(tex2.Width * i, tex2.Height * j).ParalaxXY(new Vector2(-0.6f, -0.3f)), bgAlpha);

                    for (int i = (int)chunk3.X - 1; i <= (int)chunk3.X + 1; i++)
                        for (int j = (int)chunk3.Y - 1; j <= (int)chunk3.Y + 1; j++)
                            global::EEMod.LightingBuffer.Instance.DrawWithBuffer(
                            tex3,
                            new Vector2(tex3.Width * i, tex3.Height * j).ParalaxXY(new Vector2(-0.4f, -0.3f)), bgAlpha);

                    Main.spriteBatch.End();
                }
                else
                {
                    //int a = 2;
                    //SurfaceBackgroundStylesLoader.ChooseStyle(ref a);
                }
            }

            orig(self);
        }

        private bool DisableFancyUIOnSeamap(On.Terraria.UI.IngameFancyUI.orig_Draw orig, SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (SubworldSystem.IsActive<Sea>()) return false;
            else return orig(spriteBatch, gameTime);
        }

        public void GoblinTableCollision(On.Terraria.Player.orig_Update_NPCCollision orig, Player self)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.ModProjectile is PhantomTable table)
                {
                    if (!self.active || self.controlDown) return;

                    var playerBox = new Rectangle((int)self.position.X, (int)self.position.Y + self.height, self.width, 1);
                    var floorBox = new Rectangle((int)proj.position.X, (int)proj.position.Y - (int)table.falseVelocity.Y, proj.width, 8 + (int)Math.Max(self.velocity.Y, 0));

                    if (/*player.Bottom.Y > (Projectile.position.Y - player.height + ((float)Math.Sin(Projectile.rotation) * (player.Center.X - Projectile.Center.X)))
                            && */playerBox.Intersects(floorBox) && self.velocity.Y > 0 && !Collision.SolidCollision(self.Bottom, self.width, (int)Math.Max(1 + table.falseVelocity.Y, 0)))
                    {
                        if (self.velocity.Y > 1 && table.offsetVel.Y <= 0)
                        {
                            table.offsetVel.Y = self.velocity.Y / 3f;
                        }

                        self.gfxOffY = proj.gfxOffY;
                        self.position.Y = proj.position.Y - self.height + ((float)Math.Sin(proj.rotation) * (self.Center.X - proj.Center.X));
                        self.velocity.Y = 0;
                        self.oldVelocity.Y = 0;
                        self.fallStart = (int)(self.position.Y / 16f);
                        self.fallStart2 = (int)(self.position.Y / 16f);

                        proj.rotation = (self.Center.X - proj.Center.X) * 0.001f;

                        if (self == Main.LocalPlayer)
                            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, Main.LocalPlayer.whoAmI);
                    }
                    else
                    {
                        if (table.offsetPos.Y > 0) table.offsetPos.Y--;
                        else table.offsetPos.Y = 0;

                        if (Math.Abs(table.offsetVel.Y) <= 0.01f) table.offsetVel.Y = 0f;
                    }
                }
            }

            orig(self);
        }

        private void RenderPrimitives(On.Terraria.Main.orig_DoDraw_UpdateCameraPosition orig)
        {
            orig();

            if (SubworldSystem.IsActive<Sea>() && SeamapObjects.localship != null)
            {
                Main.screenPosition = SeamapObjects.localship.Center + new Vector2(-Main.screenWidth / 2f, -Main.screenHeight / 2f);

                ClampScreenPositionToWorld(Seamap.Core.Seamap.seamapWidth, Seamap.Core.Seamap.seamapHeight - 200);
            }

            if (Main.spriteBatch != null && PrimitiveSystem.primitives != null)
            {
                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(PrimitiveSystem.primitives.primTargetPixelated);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin();

                foreach (Primitive trail in PrimitiveSystem.primitives._trails.ToArray())
                {
                    if (!trail.behindTiles && trail.pixelated)
                    {
                        trail.Draw();
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
            }

            if (Main.spriteBatch != null && PrimitiveSystem.primitives != null)
            {
                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(PrimitiveSystem.primitives.primTargetUnpixelated);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin();

                foreach (Primitive trail in PrimitiveSystem.primitives._trails.ToArray())
                {
                    if (!trail.behindTiles && !trail.pixelated)
                    {
                        trail.Draw();
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
            }

            if (Main.spriteBatch != null && PrimitiveSystem.primitives != null)
            {
                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(PrimitiveSystem.primitives.primTargetBTPixelated);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin();

                foreach (Primitive trail in PrimitiveSystem.primitives._trails.ToArray())
                {
                    if (trail.behindTiles && trail.pixelated)
                    {
                        trail.Draw();
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
            }

            if (Main.spriteBatch != null && PrimitiveSystem.primitives != null)
            {
                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(PrimitiveSystem.primitives.primTargetBTUnpixelated);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin();

                foreach (Primitive trail in PrimitiveSystem.primitives._trails.ToArray())
                {
                    if (trail.behindTiles && !trail.pixelated)
                    {
                        trail.Draw();
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
            }
        }

        private void PrepareSubworldList(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, UIWorldListItem self, WorldFileData data, int orderInList, bool canBePlayed)
        {
            orig(self, data, orderInList, canBePlayed);

            float num = 56f + 24f;

            if (SocialAPI.Cloud != null)
            {
                num += 24f;
            }

            if (data.WorldGeneratorVersion != 0L)
            {
                num += 24f;
            }


            string EEPath = $@"{Main.SavePath}\Worlds\Subworlds\{data.Name.Replace(' ', '_')}";
            List<string> SubworldsUnlocked = new List<string>();

            if (Directory.Exists(EEPath))
            {
                //TODO make this better
                string[] Subworlds = new string[] { "Coral Reefs", "Sea", "Goblin Outpost" };
                foreach (string S in Subworlds)
                {
                    string subworldPath = $@"{EEPath}\EEMod_{S}.wld";

                    if (File.Exists(subworldPath))
                    {
                        SubworldsUnlocked.Add(S);
                    }
                }
            }

            string SLock = SubworldsUnlocked.Count > 0 ? string.Join(", ", SubworldsUnlocked) : "No Unlocked Islands"; // TODO: Localization maybe?

            UIText buttonLabel = new UIText(SLock)
            {
                VAlign = 1f
            };

            buttonLabel.Left.Set(num + 10, 0f);
            buttonLabel.Top.Set(-3f, 0f);

            DetourReflectionCache.UIWorldListItem_buttonLabel.SetValue(self, buttonLabel);

            self.Append(buttonLabel);
        }

        private void RenderSubworldList(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);

            WorldFileData data = (WorldFileData)DetourReflectionCache.UIWorldListItem_data.GetValue(self);
            UIImage worldIcon = (UIImage)DetourReflectionCache.UIWorldListItem_worldIcon.GetValue(self);

            CalculatedStyle innerDimensions = self.GetInnerDimensions();
            CalculatedStyle dimensions = worldIcon.GetDimensions();

            float num = 76f;

            if (SocialAPI.Cloud != null)
            {
                num += 24f;
            }

            if (data.WorldGeneratorVersion != 0L)
            {
                num += 24f;
            }

            float num2 = dimensions.X + num;

            Vector2 position = new Vector2(num2, innerDimensions.Y + 59);
            const float width = 370;

            Texture2D texture = ModContent.Request<Texture2D>("Terraria/Images/UI/InnerPanelBackground").Value;

            spriteBatch.Draw(texture, position, new Rectangle(0, 0, 8, texture.Height), Color.White);
            spriteBatch.Draw(texture, new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, texture.Height), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, texture.Height), Color.White);
        }

        private void PreRenderNPCs(On.Terraria.Main.orig_CacheNPCDraws orig, Main self)
        {
            BeforeNPCCache?.Invoke(Main.spriteBatch);

            orig(self);
        }

        private void ManageSaving(On.Terraria.WorldGen.orig_SaveAndQuitCallBack orig, object threadContext)
        {
            isSaving = true;

            orig(threadContext);

            isSaving = false;
        }

        private void RenderBehindTiles(On.Terraria.Main.orig_DrawWoF orig, Main self)
        {
            Main.spriteBatch.End();

            PrimitiveSystem.primitives.DrawTrailsBehindTiles();

            Main.spriteBatch.Begin();

            foreach (IComponent Updateable in Updatables)
                Updateable.Update();

            BeforeTiles?.Invoke(Main.spriteBatch);

            DrawLensFlares();

            orig(self);
        }

        private void PreparePrimitives(GameTime obj)
        {
            if (Main.gameMenu)
                PrimitiveSystem.primitives._trails.Clear();
        }

        private void RenderSeamap(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            if (!Main.dedServ)
            {
                if (!SubworldSystem.IsActive<Sea>())
                {
                    PrimitiveSystem.primitives.DrawTrailsAboveTiles();

                    Particles.Update();

                    Particles.Draw(Main.spriteBatch);
                }
                else
                {
                    Seamap.Core.Seamap.Render();
                }
            }

            orig(self);
        }

        private void ManageWorldLoadUI(On.Terraria.Main.orig_Draw orig, Main self, GameTime gameTime)
        {
            orig(self, gameTime);

            if (Main.menuMode == 10)
            {
                wasDoingWorldGen = true;
            }
            else if (!(Main.MenuUI.CurrentState is UIWorldLoad))
            {
                wasDoingWorldGen = false;
            }
        }

        private void RegisterLightPoint(On.Terraria.Lighting.orig_AddLight_int_int_float_float_float orig, int i, int j, float R, float G, float B)
        {
            global::EEMod.LightingBuffer.Instance._lightPoints.Add(new Vector2(i + 0.5f, j + 0.5f));
            global::EEMod.LightingBuffer.Instance._colorPoints.Add(new Color(R, G, B));

            orig(i, j, R, G, B);
        }

#pragma warning disable IDE0051 // Private members
        private static class DetourReflectionCache
        {
            public static FieldInfo UIWorldListItem_data;
            public static FieldInfo UIWorldListItem_worldIcon;
            public static FieldInfo UIWorldListItem_buttonLabel;

            [LoadingMethod]
            private static void Load()
            {
                Type t = typeof(UIWorldListItem);
                UIWorldListItem_data = t.GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic);
                UIWorldListItem_worldIcon = t.GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic);
                UIWorldListItem_buttonLabel = t.GetField("_buttonLabel", BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }
    }

    public class JITFixer : PreJITFilter
    {
        public override bool ShouldJIT(MemberInfo member) => member.Module.Assembly == typeof(EEMod).Assembly;
    }

    public class BlankLoadEntity : Entity
    {
        
    }

    public class LoadingScreenMusic : ModMenu
    {
        public override int Music => (ModContent.GetInstance<EEMod>().leftBound != ModContent.GetInstance<EEMod>().rightBound ?
            MusicLoader.GetMusicSlot(ModContent.GetInstance<EEMod>(), "Assets/Music/goodman") : 
            base.Music);
    }
}