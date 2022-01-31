using EEMod.Autoloading;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.NPCs.Bosses.Kraken;
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
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;
using EEMod.Seamap.SeamapContent;
using EEMod.Seamap.SeamapAssets;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.UI.Chat;
using EEMod.Systems;
using Terraria.GameContent;
using EEMod.NPCs.Glowshroom;

namespace EEMod
{
    internal delegate void MechanicDrawDelegate(SpriteBatch spriteBatch);
    public partial class EEMod
    {
        internal event MechanicDrawDelegate BeforeTiles;

        internal event MechanicDrawDelegate AfterTiles;

        internal event MechanicDrawDelegate BeforeNPCCache;

        public List<IComponent> Updatables = new List<IComponent>();

        WaterPrimitive WP;

        float bgAlpha;

        private int osSucksAtBedwars;
        private float textPositionLeft;
        private bool wasDoingWorldGen = false;

        public RenderTarget2D additiveRT;

        public static float lerp;

        private void LoadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float += Lighting_AddLight_int_int_float_float_float;
            On.Terraria.Main.Update += Main_Update;
            On.Terraria.Main.Draw += Main_Draw;
            On.Terraria.Main.DrawBG += Main_DrawBG;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Main.DrawWoF += Main_DrawWoF;
            On.Terraria.Main.DrawWalls += Main_DrawWalls;
            On.Terraria.Main.DrawTiles += Main_DrawTiles;
            On.Terraria.Main.DrawWater += Main_DrawWater1;
            On.Terraria.Main.DrawBackground += Main_DrawBackground1;
            On.Terraria.Main.CacheNPCDraws += Main_CacheNPCDraws;
            On.Terraria.Main.DrawTiles += Main_DrawTiles1;
            On.Terraria.Main.CacheNPCDraws += Main_CacheNPCDraws;

            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;
            On.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw += LiquidRenderer_InternalDraw;

            On.Terraria.WorldGen.SaveAndQuitCallBack += WorldGen_SaveAndQuitCallBack;

            On.Terraria.Main.DoDraw_UpdateCameraPosition += Main_DoDraw_UpdateCameraPosition;

            Main.OnPreDraw += Main_OnPreDraw;

            WP = new WaterPrimitive(null);
            PrimitiveSystem.primitives.CreateTrail(WP);
        }

        private void UnloadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float -= Lighting_AddLight_int_int_float_float_float;
            On.Terraria.Main.Update -= Main_Update;
            On.Terraria.Main.Draw -= Main_Draw;
            On.Terraria.Main.DrawBG -= Main_DrawBG;
            On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
            On.Terraria.Main.DrawWoF -= Main_DrawWoF;
            On.Terraria.Main.DrawWalls -= Main_DrawWalls;
            On.Terraria.Main.DrawTiles -= Main_DrawTiles;
            On.Terraria.Main.DrawWater -= Main_DrawWater1;
            On.Terraria.Main.DrawBackground -= Main_DrawBackground1;
            On.Terraria.Main.CacheNPCDraws -= Main_CacheNPCDraws;
            On.Terraria.Main.DrawTiles -= Main_DrawTiles1;
            On.Terraria.Main.DrawNPC -= Main_DrawNPC1;
            On.Terraria.Main.CacheNPCDraws -= Main_CacheNPCDraws;
            On.Terraria.Main.DrawGoreBehind -= Main_DrawGoreBehind;

            On.Terraria.Main.DoDraw_UpdateCameraPosition -= Main_DoDraw_UpdateCameraPosition;

            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor -= UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf -= UIWorldListItem_DrawSelf;
            On.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw -= LiquidRenderer_InternalDraw;

            On.Terraria.WorldGen.SaveAndQuitCallBack -= WorldGen_SaveAndQuitCallBack;

            Main.OnPreDraw -= Main_OnPreDraw;
        }

        private void Main_DoDraw_UpdateCameraPosition(On.Terraria.Main.orig_DoDraw_UpdateCameraPosition orig)
        {
            orig();

            if (Main.spriteBatch != null && PrimitiveSystem.primitives != null)
            {
                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(PrimitiveSystem.primitives.primTargetPixelated);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin();

                foreach (Primitive trail in PrimitiveSystem.primitives._trails.ToArray())
                {
                    if (!trail.behindTiles && !trail.ManualDraw && trail.pixelated)
                    {
                        trail.Draw();
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);

                PrimitiveSystem.primitives.DrawTrailsAboveTiles();
            }

            if (Main.spriteBatch != null && PrimitiveSystem.primitives != null)
            {
                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(PrimitiveSystem.primitives.primTargetUnpixelated);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin();

                foreach (Primitive trail in PrimitiveSystem.primitives._trails.ToArray())
                {
                    if (!trail.behindTiles && !trail.ManualDraw && !trail.pixelated)
                    {
                        trail.Draw();
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);

                PrimitiveSystem.primitives.DrawTrailsAboveTiles();
            }
        }

        private void Main_DrawTiles1(On.Terraria.Main.orig_DrawTiles orig, Main self, bool solidLayer, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            if (Main.worldName == KeyID.CoralReefs && !Main.gameMenu)
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
                    Texture2D tex = EEMod.Instance.Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceFar").Value;
                    Texture2D tex2 = EEMod.Instance.Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceMid").Value;
                    Texture2D tex3 = EEMod.Instance.Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceClose").Value;

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
                }
                else
                {
                    int a = 2;
                    //SurfaceBackgroundStylesLoader.ChooseStyle(ref a);
                }
            }

            orig(self, solidLayer, forRenderTargets, intoRenderTargets, waterStyleOverride);
        }

        private void UIWorldListItem_ctor(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, UIWorldListItem self, WorldFileData data, int orderInList, bool canBePlayed)
        {
            orig(self, data, orderInList, canBePlayed);

            string EEPath = $@"{Main.SavePath}\Worlds\{data.Name}Subworlds";
            List<string> SubworldsUnlocked = new List<string>();

            if (Directory.Exists(EEPath))
            {
                //TODO make this better
                string[] Subworlds = new string[] { KeyID.CoralReefs, KeyID.Sea, KeyID.VolcanoIsland };
                foreach (string S in Subworlds)
                {
                    string CRPath = $@"{EEPath}\{S}.wld";

                    if (File.Exists(CRPath))
                    {
                        SubworldsUnlocked.Add(S);
                    }
                }
            }


            float num = 56f + 24f;

            if (SocialAPI.Cloud != null)
            {
                num += 24f;
            }

            if (data.WorldGeneratorVersion != 0L)
            {
                num += 24f;
            }

            //foreach (string SW in SubworldsUnlocked)
            //{
            //    SLock += $" {SW},";
            //}
            //SLock = SLock.TrimEnd(',', ' ');

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

        private void Main_CacheNPCDraws(On.Terraria.Main.orig_CacheNPCDraws orig, Main self)
        {
            //DrawSpiderPort();

            /*if (Main.worldName == KeyID.CoralReefs)
            {
                if (Main.LocalPlayer.Center.Y > 3000)
                {
                    bgAlpha += (1 - bgAlpha) / 32f;
                }
                else
                {
                    bgAlpha += -bgAlpha / 32f;
                }
                Texture2D tex = EEMod.Instance.GetTexture("Backgrounds/CoralReefsSurfaceFar");
                Texture2D tex2 = EEMod.Instance.GetTexture("Backgrounds/CoralReefsSurfaceMid");
                Texture2D tex3 = EEMod.Instance.GetTexture("Backgrounds/CoralReefsSurfaceClose");
                //LightingBuffer.Instance.Draw(Main.spriteBatch);

                Vector2 chunk1 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.8f, 0.3f)) / tex.Size();
                Vector2 chunk2 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.6f, 0.3f)) / tex2.Size();
                Vector2 chunk3 = Main.LocalPlayer.Center.ParalaxXY(new Vector2(0.4f, 0.3f)) / tex3.Size();


                for (int i = (int)chunk1.X - 1; i <= (int)chunk1.X + 1; i++)
                    for (int j = (int)chunk1.Y - 1; j <= (int)chunk1.Y + 1; j++)
                        LightingBuffer.Instance.DrawWithBuffer(
                        tex,
                        new Vector2(tex.Width * i, tex.Height * j).ParalaxXY(new Vector2(-0.8f, -0.3f)));
                for (int i = (int)chunk2.X - 1; i <= (int)chunk2.X + 1; i++)
                    for (int j = (int)chunk2.Y - 1; j <= (int)chunk2.Y + 1; j++)
                        LightingBuffer.Instance.DrawWithBuffer(
                        tex2,
                        new Vector2(tex2.Width * i, tex2.Height * j).ParalaxXY(new Vector2(-0.6f, -0.3f)));
                for (int i = (int)chunk3.X - 1; i <= (int)chunk3.X + 1; i++)
                    for (int j = (int)chunk3.Y - 1; j <= (int)chunk3.Y + 1; j++)
                        LightingBuffer.Instance.DrawWithBuffer(
                        tex3,
                        new Vector2(tex3.Width * i, tex3.Height * j).ParalaxXY(new Vector2(-0.4f, -0.3f)));
            }
*/

            BeforeNPCCache?.Invoke(Main.spriteBatch);
            orig(self);
        }

        private void Main_DrawBackground1(On.Terraria.Main.orig_DrawBackground orig, Main self)
        {
            orig(self);
        }

        private void Main_DrawWalls(On.Terraria.Main.orig_DrawWalls orig, Main self)
        {
            orig(self);
        }

        private void Main_DrawWater1(On.Terraria.Main.orig_DrawWater orig, Main self, bool bg, int Style, float Alpha)
        {
            orig(self, bg, Style, Main.worldName == KeyID.CoralReefs ? Alpha/3.5f : Alpha);
        }

        private void Main_DrawTiles(On.Terraria.Main.orig_DrawTiles orig, Main self, bool solidOnly, bool forRenderTargets, bool intoRenderTargets, int waterStyleOverride)
        {
            orig(self, solidOnly, forRenderTargets, intoRenderTargets, waterStyleOverride);
        }

        private void LiquidRenderer_InternalDraw(On.Terraria.GameContent.Liquid.LiquidRenderer.orig_InternalDraw orig, Terraria.GameContent.Liquid.LiquidRenderer self, SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float globalAlpha, bool isBackgroundDraw)
        {
            orig(self, spriteBatch, drawOffset, waterStyle, globalAlpha, isBackgroundDraw);
        }

        private void Main_DrawGoreBehind(On.Terraria.Main.orig_DrawGoreBehind orig, Main self)
        {
            orig(self);
        }

        private void Main_DrawNPC1(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCIndex, bool behindTiles)
        {
            orig(self, iNPCIndex, behindTiles);
        }

        private void WorldGen_SaveAndQuitCallBack(On.Terraria.WorldGen.orig_SaveAndQuitCallBack orig, object threadContext)
        {
            isSaving = true;

            orig(threadContext);

            isSaving = false;

            //saveInterface?.SetState(null);
        }

        private void UIWorldListItem_DrawSelf(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);

            WorldFileData data = (WorldFileData)DetourReflectionCache.UIWorldListItem_data.GetValue(self);
            UIImage worldIcon = (UIImage)DetourReflectionCache.UIWorldListItem_worldIcon.GetValue(self);
            CalculatedStyle innerDimensions = self.GetInnerDimensions();
            CalculatedStyle dimensions = worldIcon.GetDimensions();
            float num = 56f;

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

            //Texture2D texture = TextureCollection.Load("Images/UI/InnerPanelBackground");
            //spriteBatch.Draw(texture, position, new Rectangle(0, 0, 8, texture.Height), Color.White);
            //spriteBatch.Draw(texture, new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, texture.Height), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
            //spriteBatch.Draw(texture, new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, texture.Height), Color.White);
        }

        private void Main_DrawWoF(On.Terraria.Main.orig_DrawWoF orig, Main self)
        {
            foreach (IComponent Updateable in Updatables)
                Updateable.Update();

            BeforeTiles?.Invoke(Main.spriteBatch);

            Vector2 v = Main.LocalPlayer.Center.ForDraw() - new Vector2(Main.screenWidth/2,Main.screenHeight/2);
            DrawLensFlares();

            if (Main.worldName == KeyID.CoralReefs)
            {
                /*if (Main.LocalPlayer.Center.Y >= (Main.maxTilesY / 20f) * 16)
                {
                    DrawCoralReefsBg();
                }*/
            }

            //Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, ChangingPoints.ForDraw(), Color.Red);

            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ZoneCoralReefs)
            {
                _alphaBG += (1 - _alphaBG) / 64f;
            }
            else
            {
                _alphaBG -= _alphaBG / 64f;
            }

            for (int i = 0; i < 400; i++)
            {
                if (Main.projectile[i].active)
                {
                    if (Main.projectile[i].ModProjectile is Gradient a)
                    {
                        a.pixelPlacmentHours();
                    }
                    if (Main.projectile[i].ModProjectile is CyanoburstTomeKelp aa)
                    {
                        aa.DrawBehind();
                    }
                }
            }

            if (NPC.AnyNPCs(ModContent.NPCType<TentacleEdgeHandler>()))
            {
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].type == ModContent.NPCType<TentacleEdgeHandler>())
                    {
                        (Main.npc[i].ModNPC as TentacleEdgeHandler).DrawTentacleBeziers();
                    }
                }
            }
            //WP.Draw();
            orig(self);
        }

        private void Main_OnPreDraw(GameTime obj)
        {
            if (Main.gameMenu)
                PrimitiveSystem.primitives._trails.Clear();

            Vector2 lastScreenPos = Main.screenPosition;

            /*if (!Main.gameMenu && !Main.LocalPlayer.GetModPlayer<EEPlayer>().triggerSeaCutscene)
            {
                Main.screenPosition = Main.LocalPlayer.position + new Vector2(Main.LocalPlayer.width / 2f, Main.LocalPlayer.height / 2f) - new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);

                Vector2 input = new Vector2(Main.leftWorld + 656f, Main.topWorld + 656f) - Main.GameViewMatrix.Translation;
                Vector2 input2 = new Vector2(Main.rightWorld - (float)Main.screenWidth / Main.GameViewMatrix.Zoom.X - 672f, Main.bottomWorld - (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y - 672f) - Main.GameViewMatrix.Translation;
                
                input = Utils.Round(input);
                input2 = Utils.Round(input2);
                
                Main.screenPosition = Vector2.Clamp(Main.screenPosition, input, input2);
            }*/

            Main.screenPosition = lastScreenPos;

            if (Main.spriteBatch != null && additiveRT != null)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                if (additiveRT != null)
                {
                    Main.spriteBatch.Draw(additiveRT, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                }

                Main.spriteBatch.End();

                RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTarget(additiveRT);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i] != null && Main.projectile[i].type == ModContent.ProjectileType<MushroomBall>())
                    {
                        (Main.projectile[i].ModProjectile as MushroomBall).DrawMushroom();
                    }
                }

                Main.spriteBatch.End();

                Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
            }
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            PrimitiveSystem.trailManager.DrawTrails(Main.spriteBatch);

            if (!Main.dedServ)
            {
                PrimitiveSystem.trailManager.DrawTrails(Main.spriteBatch);
                PrimitiveSystem.primitives.DrawTrailsAboveTiles();

                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null);

                if (additiveRT != null)
                {
                    Main.spriteBatch.Draw(additiveRT, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                }

                Main.spriteBatch.End();
            }

            //Main.QueueMainThreadAction(() =>
            //{
            //    if (Main.spriteBatch != null && PrimSystem.primitives != null)
            //    {
            //        PrimSystem.primitives.DrawTrailsAboveTiles();
            //        //PrimSystem.primitives.DrawTrailsBehindTiles();
            //    }
            //});

            if (Main.worldName == KeyID.Sea && SeamapObjects.localship != null)
            {
                Main.screenPosition = SeamapObjects.localship.Center + new Vector2(-Main.screenWidth / 2f, -Main.screenHeight / 2f);

                Main.screenPosition.X = MathHelper.Clamp(Main.screenPosition.X, 0, (Seamap.SeamapContent.Seamap.seamapWidth) - Main.screenWidth);
                Main.screenPosition.Y = MathHelper.Clamp(Main.screenPosition.Y, 0, (Seamap.SeamapContent.Seamap.seamapHeight - 200) - Main.screenHeight);


                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                Seamap.SeamapContent.Seamap.Render();

                Main.spriteBatch.End();
            }

            orig(self);
        }

        private void Main_DrawBG(On.Terraria.Main.orig_DrawBG orig, Main self)
        {
            if (EEModConfigClient.Instance.BetterLighting && !Main.gameMenu)
            {
                BetterLightingHandler();
                DrawGlobalShaderTextures();
            }
            else
            {
                UnloadShaderAssets();
            }

            orig(self);
        }

        private void Main_Draw(On.Terraria.Main.orig_Draw orig, Main self, GameTime gameTime)
        {
           orig(self, gameTime);
            if (EEModConfigClient.Instance.EEDebug)
            {
                Main.spriteBatch.Begin();

                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "EEModDebug MenuMode: " + Main.menuMode.ToString(), new Vector2(50, 60), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

                if (!Main.gameMenu) 
                {
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "EEModDebug Player Velocity X: " + Main.LocalPlayer.velocity.X.ToString(), new Vector2(50, 80), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "EEModDebug Player Velocity Y: " + Main.LocalPlayer.velocity.Y.ToString(), new Vector2(50, 100), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "EEModDebug Player Position X: " + Main.LocalPlayer.Center.Y.ToString(), new Vector2(50, 120), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "EEModDebug Player Position Y: " + Main.LocalPlayer.Center.X.ToString(), new Vector2(50, 140), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "EEModDebug Player Tile Pos X: " + ((int)Main.LocalPlayer.Center.Y / 16).ToString(), new Vector2(50, 160), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "EEModDebug Player Tile Pos Y: " + ((int)Main.LocalPlayer.Center.X / 16).ToString(), new Vector2(50, 180), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "Time: " + Main.time.ToString(), new Vector2(50, 200), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                }

                Main.spriteBatch.End();
            }

            if (Main.menuMode == 10)
            {
                wasDoingWorldGen = true;
            }
            else if (!(Main.MenuUI.CurrentState is UIWorldLoad))
            {
                wasDoingWorldGen = false;
            }

            if ((isSaving && Main.gameMenu) || Main.MenuUI.CurrentState is UIWorldLoad)
            {
                alpha += 0.01f;
                if (lerp != 1)
                    lerp += (1 - lerp) / 16f;
                if (lerp > 0.99f)
                {
                    lerp = 1;
                }
                if (alpha > 1)
                {
                    alpha = 1;
                }

                Main.numClouds = 0;

                if (SkyManager.Instance["EEMod:SavingCutscene"] != null)
                {
                    SkyManager.Instance.Activate("EEMod:SavingCutscene", default, new object[0]);
                }

                if (loadingFlag)
                {
                    loadingChoose = Main.rand.Next(68);
                    loadingChooseImage = Main.rand.Next(5);
                    loadingFlag = false;
                    osSucksAtBedwars = 0;
                    textPositionLeft = 0;
                }

                switch (loadingChoose)
                {
                    case 0:
                        screenMessageText = "All good idiocy done by EpicCrownKing";
                        break;

                    case 1:
                        screenMessageText = "All good sprites made by Nomis";
                        break;

                    case 2:
                        screenMessageText = "Tip of the Day: Loading screens are useless!";
                        break;

                    case 3:
                        screenMessageText = "Fear the MS Paint cat";
                        break;

                    case 4:
                        screenMessageText = "Terraria sprites need outlines... except when I make them";
                        break;

                    case 5:
                        screenMessageText = "Remove the banding";
                        break;

                    case 6:
                        screenMessageText = Main.LocalPlayer.name + " ... huh? What a cruddy name";
                        break;

                    case 7:
                        screenMessageText = "Don't ping everyone you big dumb stupid";
                        break;

                    case 8:
                        screenMessageText = "I'm nothing without attention";
                        break;

                    case 9:
                        screenMessageText = "Why are you even reading this?";
                        break;

                    case 10:
                        screenMessageText = "We actually think we're funny";
                        break;

                    case 11:
                        screenMessageText = "Interitos... what's that?";
                        break;

                    case 12:
                        screenMessageText = "It's my style";
                        break;

                    case 13:
                        screenMessageText = "Now featuring 50% more monkey per chimp!";
                        break;

                    case 14:
                        screenMessageText = "im angy";
                        break;

                    case 15:
                        screenMessageText = "All good music made by A44";
                        break;

                    case 16:
                        screenMessageText = "Mod is not edgy, I swear!";
                        break;

                    case 17:
                        screenMessageText = "All good art made by cynik";
                        break;

                    case 18:
                        screenMessageText = "I'm gonna have to mute you for that";
                        break;

                    case 19:
                        screenMessageText = "Gamers, rise up!";
                        break;

                    case 20:
                        screenMessageText = "THAT'S NOT THE CONCEPT";
                        break;

                    case 21:
                        screenMessageText = "All bad sprites made by Doodle";
                        break;

                    case 22:
                        screenMessageText = "D D D A G# G F D F G";
                        break;

                    case 23:
                        screenMessageText = "We live in a society";
                        break;

                    case 24:
                        screenMessageText = "Don't mine at night!";
                        break;

                    case 25:
                        screenMessageText = "deleting system32...";
                        break;

                    case 26:
                        screenMessageText = "Sans in real!";
                        break;

                    case 27:
                        screenMessageText = "I sure hope I didnt break the codeghsduighshsy";
                        break;

                    case 28:
                        screenMessageText = "If you like Endless Escapade, you'll love Endless Escapade Premium (patent pending)!";
                        break;

                    case 29:
                        screenMessageText = "slight smile";
                        break;

                    case 30:
                        screenMessageText = "mario in real life";
                        break;

                    case 31:
                        screenMessageText = "when the fruit salad is \nis yummy yummy";
                        break;

                    case 32:
                        screenMessageText = "EEMod Foretold? More like doesn't exist";
                        break;

                    case 33:
                        screenMessageText = "You think this is a game? Look behind you 0_0";
                        break;

                    case 34:
                        screenMessageText = "Respect the drip Karen";
                        break;

                    case 35:
                        screenMessageText = "phosh";
                        break;

                    case 36:
                        screenMessageText = "All good sprites made by daimgamer";
                        break;

                    case 37:
                        screenMessageText = "All good music made by Universe";
                        break;

                    case 38:
                        screenMessageText = "All good sprites made by Vadim";
                        break;

                    case 39:
                        screenMessageText = "All good sprites made by Pyxis";
                        break;

                    case 40:
                        screenMessageText = "All good builds made by Cherry";
                        break;

                    case 41:
                        screenMessageText = "Haha funny mod go brrr";
                        break;

                    case 42:
                        screenMessageText = "Do a Barrel Roll";
                        break;

                    case 43:
                        screenMessageText = "The man behind the laughter";
                        break;

                    case 44:
                        screenMessageText = "janding restart!";
                        break;

                    case 45:
                        screenMessageText = "An apple a day keeps the errors away!";
                        break;

                    case 46:
                        screenMessageText = "Poggers? Poggers.";
                        break;

                    case 47:
                        screenMessageText = $"By the way, {Main.LocalPlayer.name} is a dumb name";
                        break;

                    case 48:
                        screenMessageText = "It all ends eventually!";
                        break;

                    case 49:
                        screenMessageText = "Illegal in 5 countries!";
                        break;

                    case 50:
                        screenMessageText = "Inside jokes you wont understand!";
                        break;

                    case 51:
                        screenMessageText = "Big content mod bad!";
                        break;

                    case 52:
                        screenMessageText = "Loading the random chimp event...";
                        break;

                    case 53:
                        screenMessageText = "Sending you to the Aether...";
                        break;

                    case 54:
                        screenMessageText = "When";
                        break;

                    case 55:
                        screenMessageText = "[Insert non funny joke here]";
                        break;

                    case 56:
                        screenMessageText = "The dev server is indeed an asylum";
                        break;

                    case 57:
                        screenMessageText = "full moon with face";
                        break;

                    case 58:
                        screenMessageText = "That's how the mafia works";
                        break;

                    case 59:
                        screenMessageText = "Hacking the mainframe...";
                        break;

                    case 60:
                        screenMessageText = "Not Proud";
                        break;

                    case 61:
                        screenMessageText = "You know I think the ocean needs more con- Haha the literal ocean goes brr";
                        break;

                    case 62:
                        screenMessageText = "EA Jorts, it's in the seams.";
                        break;

                    case 63:
                        screenMessageText = "Forged in Fury";
                        break;

                    case 64:
                        screenMessageText = "Have you guys heard of calamity?";
                        break;

                    case 65:
                        screenMessageText = "Who's the ideas guy?";
                        break;

                    case 66:
                        screenMessageText = "When the impostor is Velma?!?!?";
                        break;

                    case 67:
                        screenMessageText = "All existing code programmed by Stevie";
                        break;
                }

                Main.spriteBatch.Begin();
                DrawSky();

                if (FontAssets.DeathText.Value != null && screenMessageText != null)
                {
                    Vector2 textSize = FontAssets.DeathText.Value.MeasureString(screenMessageText);

                    if (progressMessage != null)
                    {
                        Vector2 textSize2 = FontAssets.MouseText.Value.MeasureString(progressMessage);
                        textSize2 = new Vector2(textSize2.X * 1.2f, textSize2.Y);

                        float textPosition2Left = Main.screenWidth / 2 - textSize2.X / 2;

                        //Main.spriteBatch.DrawString(Main.fontMouseText, progressMessage, new Vector2(textPosition2Left, Main.screenHeight / 2 + 200), Color.AliceBlue * alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, progressMessage, new Vector2(textPosition2Left, Main.screenHeight / 2 - 350), Color.White * alpha, 0f, Vector2.Zero, new Vector2(1.2f, 1.2f));
                    }

                    osSucksAtBedwars++;
                    if (osSucksAtBedwars % 600 == 0)
                    {
                        loadingChoose = Main.rand.Next(68);
                        textPositionLeft = -textSize.X / 2;
                    }
                    else if (osSucksAtBedwars % 600 > 0 && osSucksAtBedwars % 600 <= 540)
                    {
                        textPositionLeft += ((Main.screenWidth / 2) - (textSize.X / 2) - textPositionLeft) / 25f;
                    }
                    else if (osSucksAtBedwars % 600 > 540 && osSucksAtBedwars % 600 < 600)
                    {
                        textPositionLeft += ((Main.screenWidth + (textSize.X / 2)) - textPositionLeft) / 25f;
                    }
                    float tempAlpha = alpha;
                    tempAlpha = 1 - (Math.Abs((Main.screenWidth / 2) - (textSize.X / 2) - textPositionLeft) / (Main.screenWidth / 2f));


                    //Main.spriteBatch.DrawString(Main.fontDeathText, screenMessageText, new Vector2(textPositionLeft, Main.screenHeight / 2 - 100), Color.White * tempAlpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.DeathText.Value, screenMessageText, new Vector2(textPositionLeft, Main.screenHeight / 2 - 100), Color.White * tempAlpha, 0f, Vector2.Zero, Vector2.One);
                }

                try
                {/*
                    if (Main.MenuUI.CurrentState is UIWorldLoad worldLoadUI)
                    {
                        Texture2D texture;
                        Texture2D texture2;
                        int frames;
                        int frameSpeed;

                        switch (EEMod.loadingChooseImage)
                        {
                            case 0:
                                texture2 = EEMod.Instance.Assets.Request<Texture2D>("UI/LoadingScreenImages/LoadingScreen1").Value;
                                break;
                            case 1:
                                texture2 = EEMod.Instance.Assets.Request<Texture2D>("UI/LoadingScreenImages/LoadingScreen2").Value;
                                break;
                            case 2:
                                texture2 = EEMod.Instance.Assets.Request<Texture2D>("UI/LoadingScreenImages/LoadingScreen3").Value;
                                break;
                            default:
                                texture2 = EEMod.Instance.Assets.Request<Texture2D>("UI/LoadingScreenImages/LoadingScreen4").Value;
                                break;
                        }
                        switch (EEMod.loadingChooseImage)
                        {
                            default:
                            {
                                texture = ModContent.Request<Texture2D>("Terraria/Images/UI/Sunflower_Loading").Value;
                                frames = 19;
                                frameSpeed = 3;
                                break;
                            }

                            case 1:
                            {
                                texture = EEMod.Instance.Assets.Request<Texture2D>("NPCs/SurfaceReefs/HermitCrab").Value;
                                frames = 4;
                                frameSpeed = 5;
                                break;
                            }
                            case 2:
                            {
                                texture = EEMod.Instance.Assets.Request<Texture2D>("NPCs/SurfaceReefs/Seahorse").Value;
                                frames = 7;
                                frameSpeed = 4;
                                break;
                            }
                            case 3:
                            {
                                texture = EEMod.Instance.Assets.Request<Texture2D>("NPCs/LowerReefs/Lionfish").Value;
                                frames = 8;
                                frameSpeed = 10;
                                break;
                            }
                            case 4:
                            {
                                texture = EEMod.Instance.Assets.Request<Texture2D>("NPCs/ThermalVents/MechanicalShark").Value;
                                frames = 6;
                                frameSpeed = 10;
                                break;
                            }
                        }
                        if (ticker++ > frameSpeed)
                        {
                            ticker = 0;
                            frame.Y += texture.Height / frames;
                        }
                        if (frame.Y >= texture.Height / frames * (frames - 1))
                        {
                            frame.Y = 0;
                        }

                        Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 30);
                        Main.spriteBatch.Draw(texture2, new Rectangle(Main.screenWidth / 2, Main.screenHeight / 2, texture2.Width, texture2.Height), texture2.Bounds, new Color(204, 204, 204), 0, origin: new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
                        Main.spriteBatch.Draw(texture, position, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames), new Color(0, 0, 0), 0, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, SpriteEffects.None, 0);


                        worldLoadUI.Draw(Main.spriteBatch);

                        //var bar = typeof(Main).Assembly.GetType("Terraria.GameContent.UI.States").GetField("_progressBar", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(new UIGenProgressBar()) as UIGenProgressBar;

                        //bar.SetProgress(0.5f, 0.4f);
                    } */
                }
                catch
                {
                    // ignore
                }

                Main.spriteBatch.End();
            }
            else
            {
                if (!Main.dedServ)
                {
                    loadingChoose = Main.rand.Next(68);
                    loadingChooseImage = Main.rand.Next(5);
                    Main.numClouds = 10;

                    if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive())
                    {
                        SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                    }
                }
            }
        }

        int ticker;
        Rectangle frame;

        private void Main_Update(On.Terraria.Main.orig_Update orig, Main self, GameTime gameTime)
        {
            if (!Main.gameMenu && Main.netMode != NetmodeID.MultiplayerClient && !isSaving)
            {
                lerp = 0;
                alpha = 0;
                loadingChoose = Main.rand.Next(68);
                loadingChooseImage = Main.rand.Next(5);
                Main.numClouds = 10;

                if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive())
                {
                    SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                }
            }

            TextureAssets.Sun = ModContent.Request<Texture2D>("Terraria/Images/Sun");

            orig(self, gameTime);
        }

        private void Lighting_AddLight_int_int_float_float_float(On.Terraria.Lighting.orig_AddLight_int_int_float_float_float orig, int i, int j, float R, float G, float B)
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
}
