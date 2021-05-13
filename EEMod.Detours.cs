using EEMod.Autoloading;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.NPCs.Bosses.Kraken;
using EEMod.Prim;
using EEMod.Projectiles;
using EEMod.Projectiles.Mage;
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
        private void LoadDetours()
        {
            On.Terraria.Lighting.AddLight_int_int_float_float_float += Lighting_AddLight_int_int_float_float_float;
            On.Terraria.Main.DoUpdate += Main_DoUpdate;
            On.Terraria.Main.Draw += Main_Draw;
            On.Terraria.Main.DrawBG += Main_DrawBG;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Main.DrawNPC += Main_DrawNPC;
            On.Terraria.Main.DrawWoF += Main_DrawWoF;
            On.Terraria.Main.DrawWalls += Main_DrawWalls;
            On.Terraria.Main.DrawTiles += Main_DrawTiles;
            On.Terraria.Main.DrawWater += Main_DrawWater1;
            On.Terraria.Main.DrawBackground += Main_DrawBackground1;
            On.Terraria.Main.CacheNPCDraws += Main_CacheNPCDraws;
            //On.Terraria.Main.DrawNPC += Main_DrawNPC1;
            On.Terraria.Main.DrawPlayer += Main_DrawPlayer;
            //On.Terraria.Main.CacheNPCDraws += Main_CacheNPCDraws;
            //On.Terraria.Main.DrawGoreBehind += Main_DrawGoreBehind;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;
            //On.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw += LiquidRenderer_InternalDraw;
            On.Terraria.WorldGen.SaveAndQuitCallBack += WorldGen_SaveAndQuitCallBack;
            WP = new WaterPrimitive(null);
            primitives.CreateTrail(WP);
        }

        private void Main_CacheNPCDraws(On.Terraria.Main.orig_CacheNPCDraws orig, Main self)
        {
            if (Main.worldName == KeyID.CoralReefs)
            {
                if (Main.LocalPlayer.Center.Y > 3000)
                {
                    bgAlpha += (1 - bgAlpha) / 32f;
                }
                else
                {
                    bgAlpha += -bgAlpha / 32f;
                }
                Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/CoralReefsSurfaceFar");
                Texture2D tex2 = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/CoralReefsSurfaceMid");
                Texture2D tex3 = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/CoralReefsSurfaceClose");
                LightingBuffer.Instance.Draw(Main.spriteBatch);

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

        private void Main_DrawTiles(On.Terraria.Main.orig_DrawTiles orig, Main self, bool solidOnly, int waterStyleOverride)
        {
            orig(self, solidOnly, waterStyleOverride);
        }

        private void UnloadDetours()
        {
            //On.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw -= LiquidRenderer_InternalDraw;
            //On.Terraria.Main.CacheNPCDraws -= Main_CacheNPCDraws;
            On.Terraria.Main.DrawBackground -= Main_DrawBackground1;
            On.Terraria.Lighting.AddLight_int_int_float_float_float -= Lighting_AddLight_int_int_float_float_float;
            On.Terraria.Main.DoUpdate -= Main_DoUpdate;
            On.Terraria.Main.DrawNPC -= Main_DrawNPC;
            On.Terraria.Main.Draw -= Main_Draw;
            On.Terraria.Main.DrawPlayer -= Main_DrawPlayer;
            On.Terraria.Main.DrawBG -= Main_DrawBG;
            On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
            On.Terraria.Main.DrawWoF -= Main_DrawWoF;
            On.Terraria.Main.DrawTiles -= Main_DrawTiles;
            On.Terraria.Main.DrawWater -= Main_DrawWater1;
            On.Terraria.Main.CacheNPCDraws -= Main_CacheNPCDraws;
            On.Terraria.Main.DrawWalls -= Main_DrawWalls;
            //On.Terraria.Main.DrawNPC -= Main_DrawNPC1;
            //On.Terraria.Main.DrawGoreBehind -= Main_DrawGoreBehind;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor -= UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf -= UIWorldListItem_DrawSelf;
            On.Terraria.WorldGen.SaveAndQuitCallBack -= WorldGen_SaveAndQuitCallBack;
        }

        /*private void LiquidRenderer_InternalDraw(On.Terraria.GameContent.Liquid.LiquidRenderer.orig_InternalDraw orig, Terraria.GameContent.Liquid.LiquidRenderer self, SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float globalAlpha, bool isBackgroundDraw)
        {
            orig(self, spriteBatch, drawOffset, waterStyle, globalAlpha, isBackgroundDraw);
        }*/
        private void Main_DrawPlayer(On.Terraria.Main.orig_DrawPlayer orig, Main self, Player drawPlayer, Vector2 Position, float rotation, Vector2 rotationOrigin, float shadow)
        {
            // if(!Main.LocalPlayer.GetModPlayer<EEPlayer>().isLight)
            orig(self, drawPlayer, Position, rotation, rotationOrigin, shadow);
            if (!Main.gameMenu)
            {
               // if (Main.LocalPlayer.GetModPlayer<EEPlayer>().isLight)
               // {
                    /* Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.004f));
                     Particles.Get("Main").SpawnParticles(Main.LocalPlayer.Center, null, ModContent.GetInstance<EEMod>().GetTexture("Particles/Cross"), 200, 1, null, new CircularMotionSinSpin(3, 15, 0.06f, Main.LocalPlayer, 0, 0f, 0.04f, 0.01f));
                     Particles.Get("Main").SpawnParticles(Main.LocalPlayer.Center, null, ModContent.GetInstance<EEMod>().GetTexture("Particles/Cross"), 200, 1, null, new CircularMotionSinSpin(3, 15, 0.06f, Main.LocalPlayer, 0, 6.28f * 0.33f, 0.04f, 0.01f));
                     Particles.Get("Main").SpawnParticles(Main.LocalPlayer.Center, null, ModContent.GetInstance<EEMod>().GetTexture("Particles/Cross"), 200, 1, null, new CircularMotionSinSpin(3, 15, 0.06f, Main.LocalPlayer, 0, 6.28f * 0.66f, 0.04f, 0.01f));
                     Particles.Get("Main").SpawnParticles(Main.LocalPlayer.Center, null, null, 200, 1, null, new CircularMotionSinSpin(1, 1, 0.06f, Main.LocalPlayer, 0, 6.28f * 0.66f, 0.04f, 0.01f));*/
               // }

            }
            //Helpers.DrawAdditive(ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradientSlit"), Main.LocalPlayer.Center.ForDraw() + Main.LocalPlayer.velocity/3f, Color.White * (0.9f * Main.LocalPlayer.velocity.Length()/5f), (0.8f * Main.LocalPlayer.velocity.Length() / 10f), Main.LocalPlayer.velocity.ToRotation() + 3.14f);
        }

        /*private void Main_CacheNPCDraws(On.Terraria.Main.orig_CacheNPCDraws orig, Main self)
        {
            //DrawSpiderPort();
            orig(self);
        }
        private void Main_DrawGoreBehind(On.Terraria.Main.orig_DrawGoreBehind orig, Main self)
        {
            orig(self);
        }
        private void Main_DrawNPC1(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCIndex, bool behindTiles)
        {
            orig(self, iNPCIndex, behindTiles);
        }*/

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

            Texture2D texture = TextureManager.Load("Images/UI/InnerPanelBackground");
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, 8, texture.Height), Color.White);
            spriteBatch.Draw(texture, new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, texture.Height), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, texture.Height), Color.White);
        }

        public void DrawCR()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            var Bubbles = modPlayer.bubbles;
            for (int i = 0; i < Bubbles.Count; i++)
            {
                Vector2 pos = Bubbles[i].Position + new Vector2(Main.LocalPlayer.Center.X * Bubbles[i].paralax, 0);
                Color drawColour = Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16).MultiplyRGB(new Color(Bubbles[i].alpha, Bubbles[i].alpha, Bubbles[i].alpha));
                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("Particles/ForegroundParticles/Bob1"), pos.ForDraw(), null, drawColour * Bubbles[i].alpha, Bubbles[i].Velocity.ToRotation() + Bubbles[i].rotation, Vector2.Zero, Bubbles[i].scale, SpriteEffects.None, 0);
            }
        }

        private void UIWorldListItem_ctor(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, UIWorldListItem self, WorldFileData data, int snapPointIndex)
        {
            orig(self, data, snapPointIndex);
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


            float num = 56f;

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

        //CHAD NAME
        private int Projectile_NewProjectile_float_float_float_float_int_int_float_int_float_float(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1)
        {
            int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);

            if (Main.netMode != NetmodeID.Server)
            {
                trailManager.DoTrailCreation(Main.projectile[index]);
            }

            return index;
        }

        float bgAlpha;
        private void Main_DrawWoF(On.Terraria.Main.orig_DrawWoF orig, Main self)
        {
            if (Main.worldName == KeyID.CoralReefs)
            {
                if (Main.LocalPlayer.Center.Y >= ((Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60)) * 16)
                {
                    if (Main.LocalPlayer.Center.Y > 3000)
                    {
                        bgAlpha += (1 - bgAlpha) / 32f;
                    }
                    else
                    {
                        bgAlpha += -bgAlpha / 32f;
                    }

                    Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/CoralReefsSurfaceFar");
                    Texture2D tex2 = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/CoralReefsSurfaceMid");
                    Texture2D tex3 = ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/CoralReefsSurfaceClose");
                    LightingBuffer.Instance.Draw(Main.spriteBatch);

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
                else
                {
                    int a = 2;
                    SurfaceBgStyleLoader.ChooseStyle(ref a);
                }
            }



            foreach (IComponent Updateable in Updatables)
                Updateable.Update();

            BeforeTiles?.Invoke(Main.spriteBatch);

            Vector2 v = Main.LocalPlayer.Center.ForDraw() - new Vector2(Main.screenWidth/2,Main.screenHeight/2);
            Main.spriteBatch.Draw(lightingTarget, new Rectangle((int)v.X + 50,(int)v.Y + 50, Main.screenWidth/5, Main.screenHeight / 5), Color.White);
            DrawLensFlares();

            if (Main.worldName == KeyID.CoralReefs)
            {
                DrawCR();
                if (Main.LocalPlayer.Center.Y >= (Main.maxTilesY / 20f) * 16)
                {
                    DrawCoralReefsBg();
                }
            }

            //Main.spriteBatch.Draw(Main.magicPixel, ChangingPoints.ForDraw(), Color.Red);

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
                    if (Main.projectile[i].modProjectile is Gradient a)
                    {
                        a.pixelPlacmentHours();
                    }
                    if (Main.projectile[i].modProjectile is CyanoburstTomeKelp aa)
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
                        (Main.npc[i].modNPC as TentacleEdgeHandler).DrawTentacleBeziers();
                    }
                }
            }
            //WP.Draw();
            orig(self);
        }

        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            trailManager.DrawTrails(Main.spriteBatch);
            prims.DrawProjectileTrails();
            MechanicManager.PreDrawProjectiles();

            primitives.DrawTrailsAboveTiles();
            if (Main.worldName == KeyID.Sea)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                Seamap.SeamapContent.Seamap.Render();
                SeamapPlayerShip.localship.DrawSubText();
                Main.spriteBatch.End();
            }
            orig(self);
            MechanicManager.PostDrawProjectiles();
        }

        private void Main_DrawNPC(On.Terraria.Main.orig_DrawNPC orig, Main self, int iNPCTiles, bool behindTiles)
        {
            prims.DrawTrails(Main.spriteBatch);
            MechanicManager.PreDrawNPCs();
            orig(self, iNPCTiles, behindTiles);
            MechanicManager.PostDrawNPCs();
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

        private int osSucksAtBedwars;
        private float textPositionLeft;
        private bool wasDoingWorldGen = false;
        private void Main_Draw(On.Terraria.Main.orig_Draw orig, Main self, GameTime gameTime)
        {
            orig(self, gameTime);
            if (EEModConfigClient.Instance.EEDebug)
            {
                Main.spriteBatch.Begin();

                Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug MenuMode: " + Main.menuMode.ToString(), new Vector2(50, 60), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

                if (!Main.gameMenu)
                {
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Velocity X: " + Main.LocalPlayer.velocity.X.ToString(), new Vector2(50, 80), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Velocity Y: " + Main.LocalPlayer.velocity.Y.ToString(), new Vector2(50, 100), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Position X: " + Main.LocalPlayer.Center.Y.ToString(), new Vector2(50, 120), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Position Y: " + Main.LocalPlayer.Center.X.ToString(), new Vector2(50, 140), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Tile Pos X: " + ((int)Main.LocalPlayer.Center.Y / 16).ToString(), new Vector2(50, 160), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "EEModDebug Player Tile Pos Y: " + ((int)Main.LocalPlayer.Center.X / 16).ToString(), new Vector2(50, 180), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.DrawString(Main.fontMouseText, "Time: " + Main.time.ToString(), new Vector2(50, 200), Color.AliceBlue, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
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

            if ((isSaving && Main.gameMenu)) // || wasDoingWorldGen for some reason after a few reloads this ended up causing https://canary.discord.com/channels/707422488468127746/707627954691244063/842439577037832212
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

                Seamap.SeamapContent.SeamapPlayerShip.localship.velocity = Vector2.Zero;
                Main.numClouds = 0;
                Main.logo2Texture = ModContent.GetInstance<EEMod>().GetTexture("Empty");
                Main.logoTexture = ModContent.GetInstance<EEMod>().GetTexture("Empty");
                Main.sun2Texture = ModContent.GetInstance<EEMod>().GetTexture("Empty");
                Main.sun3Texture = ModContent.GetInstance<EEMod>().GetTexture("Empty");
                Main.sunTexture = ModContent.GetInstance<EEMod>().GetTexture("Empty");

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

                if (Main.fontDeathText != null && screenMessageText != null)
                {
                    Vector2 textSize = Main.fontDeathText.MeasureString(screenMessageText);

                    if (progressMessage != null)
                    {
                        Vector2 textSize2 = Main.fontMouseText.MeasureString(progressMessage);
                        float textPosition2Left = Main.screenWidth / 2 - textSize2.X / 2;

                        Main.spriteBatch.DrawString(Main.fontMouseText, progressMessage, new Vector2(textPosition2Left, Main.screenHeight / 2 + 200), Color.AliceBlue * alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
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


                    Main.spriteBatch.DrawString(Main.fontDeathText, screenMessageText, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), Color.White * tempAlpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                }

                try
                {
                    if (Main.MenuUI.CurrentState is UIWorldLoad worldLoadUI)
                        worldLoadUI.Draw(Main.spriteBatch);
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

                    Main.logo2Texture = ModContent.GetTexture("Terraria/Logo2");
                    Main.logoTexture = ModContent.GetTexture("Terraria/Logo");
                    Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
                    Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
                    Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
                }
            }
        }

        public static float lerp;
        private void Main_DoUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, GameTime gameTime)
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

                Main.logo2Texture = ModContent.GetTexture("Terraria/Logo2");
                Main.logoTexture = ModContent.GetTexture("Terraria/Logo");
                Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
                Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
                Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
            }

            Main.sunTexture = ModContent.GetInstance<EEMod>().GetTexture("Empty");

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
