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
//using System.Reflection;
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
            On.Terraria.Main.DrawWater += WaterAlphaMod;
            On.Terraria.Main.CacheNPCDraws += PreRenderNPCs;
            On.Terraria.Main.DoDraw_Tiles_Solid += DrawCoralReefsBg;
            On.Terraria.Main.DoDraw_UpdateCameraPosition += RenderPrimitives;

            On.Terraria.Main.DoDraw_WallsAndBlacks += DrawGoblinFortBg;
            //On.Terraria.Main.DoDraw_Tiles_NonSolid += DrawGoblinFortBg;

            On.Terraria.UI.IngameFancyUI.Draw += DisableFancyUIOnSeamap;

            On.Terraria.Player.Update_NPCCollision += GoblinTableCollision;

            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += PrepareSubworldList;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += RenderSubworldList;

            On.Terraria.WorldGen.SaveAndQuitCallBack += ManageSaving;

            Main.OnPreDraw += PreparePrimitives;

            if (Main.dedServ)
                return;
        }

        private void UnloadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float -= RegisterLightPoint;

            On.Terraria.Main.Draw -= ManageWorldLoadUI;
            On.Terraria.Main.DrawProjectiles -= RenderSeamap;
            On.Terraria.Main.DrawWoF -= RenderBehindTiles;
            On.Terraria.Main.DrawWater -= WaterAlphaMod;
            On.Terraria.Main.CacheNPCDraws -= PreRenderNPCs;
            On.Terraria.Main.DoDraw_Tiles_Solid -= DrawCoralReefsBg;
            On.Terraria.Main.DoDraw_UpdateCameraPosition -= RenderPrimitives;

            On.Terraria.Main.DoDraw_WallsAndBlacks -= DrawGoblinFortBg;

            On.Terraria.UI.IngameFancyUI.Draw -= DisableFancyUIOnSeamap;

            On.Terraria.Player.Update_NPCCollision -= GoblinTableCollision;

            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor -= PrepareSubworldList;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf -= RenderSubworldList;

            On.Terraria.WorldGen.SaveAndQuitCallBack -= ManageSaving;

            Main.OnPreDraw -= PreparePrimitives;
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
            for(int i = 0; i < Main.maxProjectiles; i++) 
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
                        if(self.velocity.Y > 1 && table.offsetVel.Y <= 0)
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

        private void WaterAlphaMod(On.Terraria.Main.orig_DrawWater orig, Main self, bool bg, int Style, float Alpha)
        {
            orig(self, bg, Style, Main.worldName == KeyID.CoralReefs ? (Alpha / 3.5f) : Alpha);
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
}