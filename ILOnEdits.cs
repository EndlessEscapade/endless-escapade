using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using EEMod.Projectiles;
using EEMod.NPCs.Bosses.Kraken;
using ReLogic.Graphics;
using EEMod.Projectiles.Mage;
using EEMod.Effects;
using EEMod.EEWorld;
using static EEMod.EEWorld.EEWorld;
using EEMod.ID;
using System.Collections.Generic;
using EEMod.Extensions;
using EEMod.Tiles.Furniture;
using static IL.Terraria.Lighting;
using Terraria.GameContent.Liquid;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public static TrailManager TrailManager;
        public static Prims Prims;
        private void LoadIL()
        {
            On.Terraria.WorldGen.SmashAltar += WorldGen_SmashAltar;
            //IL.Terraria.Main.DrawBackground += Main_DrawBackground;
            IL.Terraria.NPC.AI_001_Slimes += Practice;
            //IL.Terraria.Main.OldDrawBackground += Main_OldDrawBackground;
            IL.Terraria.Main.DrawWater += TransparentWater;
            //IL.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw += Traensperentaoiasjpdfdsgwuttttttttttttttryddddddddddtyrrrrrrrrrrrrrrrrrvvfghnmvvb;
            On.Terraria.Main.DoUpdate += OnUpdate;
            On.Terraria.Lighting.AddLight_int_int_float_float_float += AddToLightArray;
            On.Terraria.WorldGen.SaveAndQuitCallBack += OnSave;
            On.Terraria.Main.DrawWoF += DrawBehindTiles;
            On.Terraria.Main.Draw += OnDrawMenu;
            On.Terraria.Main.DrawBG += BetterLightingDraw;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Projectile_NewProjectile;
            On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
            On.Terraria.Main.RenderBlack += d;
            if (Main.netMode != NetmodeID.Server)
            {
                TrailManager = new TrailManager(this);
                Prims = new Prims(this);
                Prims.CreateVerlet();
            }
        }
        List<Vector2> LightPoints = new List<Vector2>();
        List<Color> ColourPoints = new List<Color>();
        private void AddToLightArray(On.Terraria.Lighting.orig_AddLight_int_int_float_float_float orig, int d, int e, float a, float b, float c)
        {
            //LightPoints.Add(new Vector2(d + 0.5f,e + 0.5f));
            //ColourPoints.Add(new Color(a, b, c));
            orig(d,e, a, b, c);
        }

        //private void Traensperentaoiasjpdfdsgwuttttttttttttttryddddddddddtyrrrrrrrrrrrrrrrrrvvfghnmvvb(ILContext il)
        //{
        //    ILCursor c = new ILCursor(il);
        //    MethodInfo call = typeof(Lighting).GetMethod(nameof(Lighting.GetColor4Slice_New), new Type[]
        //    {
        //        typeof(int), typeof(int), typeof(VertexColors).MakeByRefType(), typeof(float)
        //    });
        //    if (!c.TryGotoNext(i => i.MatchCall(call)))
        //        throw new Exception("fapsdimajpxfasafasfdddddddddddddddddddddddddddddddddddfvcxfgresdgsedf");
        //    c.Index++;
        //    c.Emit(OpCodes.Ldloca, 9);
        //    c.Emit(OpCodes.Call, new D(ModifyWaterColor).GetMethodInfo());
        //}
        //private delegate void D(ref VertexColors colors);
        //private static void ModifyWaterColor(ref VertexColors colors)
        //{
        //    Color c = Color.White;
        //    colors.TopLeftColor =c;
        //    colors.TopRightColor = c;
        //    colors.BottomLeftColor = c;
        //    colors.BottomRightColor = c;
        //}

        private void TransparentWater(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            Type liqRend = typeof(LiquidRenderer);
            MethodInfo drawcall = liqRend.GetMethod(nameof(LiquidRenderer.Draw), new Type[] { typeof(SpriteBatch), typeof(Vector2), typeof(int), typeof(float), typeof(bool) });
            if (!c.TryGotoNext(i => i.MatchCallvirt(drawcall)))
                throw new Exception("Couldn't find argument 1 post lc1");
            c.Remove();
            c.EmitDelegate<Action<LiquidRenderer, SpriteBatch, Vector2, int, float, bool>>((t, spritebatch, drawOffset, Style, Alpha, bg) =>
               {
                   t.Draw(spritebatch, drawOffset, Style, Alpha / 2, bg);
               });
        }
        public void DrawRef()
        {
            RenderTarget2D buffer = Main.screenTarget;
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
            Color[] texdata = new Color[buffer.Width * buffer.Height];
            buffer.GetData(texdata);
            Texture2D screenTex = new Texture2D(Main.graphics.GraphicsDevice, buffer.Width, buffer.Height);
            screenTex.SetData(texdata);
            Main.spriteBatch.Draw(screenTex, Main.LocalPlayer.Center.ForDraw(), new Rectangle(0, 0, 1980, 1017), Color.White * 0.3f, 0f, new Rectangle(0, 0, 1980, 1017).Size() / 2, 1, SpriteEffects.FlipVertically, 0);
            Main.graphics.GraphicsDevice.SetRenderTarget(Main.screenTarget);
        }
        private void Practice(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdloc(12),
                i => i.MatchLdcR4(200),
                i => i.MatchBneUn(out _),
                i => i.MatchBneUn(out _),
                i => i.MatchStfld(typeof(Vector2).GetField("X"))))
                return;


            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Action<NPC>>(npc =>
            {
                npc.velocity.Y = -10;
            });
            throw new Exception("Couldn't find local variable 19 loading");
        }

        private void UnloadIL()
        {
            On.Terraria.WorldGen.SmashAltar -= WorldGen_SmashAltar;
            On.Terraria.Lighting.AddLight_int_int_float_float_float -= AddToLightArray;
            //IL.Terraria.Main.DrawBackground -= Main_DrawBackground;
            IL.Terraria.NPC.AI_001_Slimes -= Practice;
            //IL.Terraria.Main.OldDrawBackground -= Main_OldDrawBackground;
            IL.Terraria.Main.DrawWater -= TransparentWater;
            //IL.Terraria.GameContent.Liquid.LiquidRenderer.InternalDraw -= Traensperentaoiasjpdfdsgwuttttttttttttttryddddddddddtyrrrrrrrrrrrrrrrrrvvfghnmvvb;
            On.Terraria.Main.DoUpdate -= OnUpdate;
            On.Terraria.WorldGen.SaveAndQuitCallBack -= OnSave;
            On.Terraria.Main.DrawWoF -= DrawBehindTiles;
            On.Terraria.Main.Draw -= OnDrawMenu;
            On.Terraria.Main.DrawBG -= BetterLightingDraw;
            On.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= Projectile_NewProjectile;
            On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
            On.Terraria.Main.RenderBlack -= d;
            screenMessageText = null;
            TrailManager = null;
            progressMessage = null;
            Prims = null;
        }
        
        private void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
        {
            TrailManager.DrawTrails(Main.spriteBatch);
            Prims.DrawTrails(Main.spriteBatch);
            orig(self);
        }
        private void d(On.Terraria.Main.orig_RenderBlack orig, Main self)
        {
            //Main.spriteBatch.Begin();
            //  DrawCoralReefsBg();
            // Main.spriteBatch.End();
            orig(self);
        }
        private void BetterLightingDraw(On.Terraria.Main.orig_DrawBG orig, Main self)
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
        Vector2 ChangingPoints;
        private int Projectile_NewProjectile(On.Terraria.Projectile.orig_NewProjectile_float_float_float_float_int_int_float_int_float_float orig, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1)
        {
            int index = orig(X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1);

            return index;
        }
        public void UnloadShaderAssets()
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Saturation"].IsActive())
            {
                Filters.Scene["EEMod:Saturation"].Deactivate();
            }
        }
        private void Main_OldDrawBackground(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            Type spritebatchType = typeof(SpriteBatch);
            MethodInfo drawcall = spritebatchType.GetMethod(nameof(SpriteBatch.Draw), new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color) });
            MethodInfo drawcall2 = spritebatchType.GetMethod(nameof(SpriteBatch.Draw), new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(float), typeof(SpriteEffects), typeof(float) });
            MethodInfo get_noretro = typeof(Lighting).GetProperty(nameof(Lighting.NotRetro)).GetGetMethod();

            if (!c.TryGotoNext(i => i.MatchLdloc(19)))
                throw new Exception("Couldn't find local variable 19 loading");

            if (!c.TryGotoNext(i => i.MatchCallvirt(drawcall)))
                throw new Exception("Couldn't find call (post variable 19)");

            //int p = c.Index;
            //c.Index++; // move past
            //var label = c.DefineLabel(); // define label
            //c.Goto(p); // return
            //c.Emit(OpCodes.Br, label); // skip
            //c.MarkLabel(label); // define label target
            //c.Index--;
            c.Remove();
            c.Emit(OpCodes.Ldloc, 15); // array
            c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, int[]>>((sb, texture, pos, rectangle, color, array) =>
            {
                if (array[4] != 135 && array[4] != 131)
                    sb.Draw(texture, pos, rectangle, color);
            });

            if (!c.TryGotoNext(i => i.MatchCall(get_noretro)))
                throw new Exception("Couldn't find Lighting.NoRetro get call");

            for (int k = 0; k < 4; k++)
            {
                if (!c.TryGotoNext(i => i.MatchCallvirt(drawcall2)))
                    throw new Exception($"Couldn't find call {k}");
                c.Remove();
                c.Emit(OpCodes.Ldloc, 15);
                c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float, int[]>>((sb, texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth, array) =>
                {
                    if (array[5] != 126 && array[5] != 125)
                        sb.Draw(texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth);
                });
            }
        }
        float alphaBG;
        //Shader Setup
        Vector2 sunPos;
        float globalAlpha;
        float intensityFunction;
        Vector2 sunShaderPos;
        float nightHarshness = 1f;
        Color BaseColor;
        public void DrawGlobalShaderTextures()
        {
            double num10;
            if (Main.time < 27000.0)
            {
                num10 = Math.Pow(1.0 - Main.time / 54000.0 * 2.0, 2.0);
            }
            else
            {
                num10 = Math.Pow((Main.time / 54000.0 - 0.5) * 2.0, 2.0);
            }

            Rectangle[] rects =
            {new Rectangle(0, 0, TextureCache.SunRing.Width, TextureCache.SunRing.Height),
             new Rectangle(0, 0, TextureCache.LensFlare.Width, TextureCache.LensFlare.Height)};
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            if (EEModConfigClient.Instance.BetterLighting)
            {
                Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Nice"), sunPos - Main.screenPosition, new Rectangle(0, 0, 174, 174), Color.White * .5f * globalAlpha * (intensityFunction * 0.36f), (float)Math.Sin(Main.time / 540f), new Vector2(87), 10f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(TextureCache.LensFlare, sunPos - Main.screenPosition + new Vector2(5, 28 + (float)num10 * 250), rects[1], Color.White * globalAlpha * intensityFunction, (float)Math.Sin(Main.time / 540f), rects[1].Size() / 2, 1.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(TextureCache.SunRing, sunPos - Main.screenPosition + new Vector2(0, 37 + (float)num10 * 250), rects[0], Color.White * .7f * globalAlpha * (intensityFunction * 0.36f), (float)Math.Sin(Main.time / 5400f), rects[0].Size() / 2, 1f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }


        public void DrawLensFlares()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            if (EEModConfigClient.Instance.BetterLighting && Main.worldName != KeyID.CoralReefs)
            {
                Main.spriteBatch.Draw(TextureCache.LensFlare2, sunPos - Main.screenPosition + new Vector2(-400, 400), new Rectangle(0, 0, 174, 174), Color.White * .7f * globalAlpha * (intensityFunction * 0.36f), 0f, new Vector2(87), 1f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(TextureCache.LensFlare2, sunPos - Main.screenPosition + new Vector2(-800, 800), new Rectangle(0, 0, 174, 174), Color.White * .8f * globalAlpha * (intensityFunction * 0.36f), 0f, new Vector2(87), .5f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void BetterLightingHandler()
        {
            Color DayColour = new Color(2.5f, 1.4f, 1);
            Color NightColour = new Color(1.2f, 1.4f, 2f);
            float shiftSpeed = 32f;
            float Base = 0.5f;
            float timeProgression = (float)Main.time / 54000f;
            float baseIntesity = 0.05f;
            float Intensity = .7f;
            float flunctuationCycle = 20000;
            float nightTransitionSpeed = 0.005f;
            float globalAlphaTransitionSpeed = 0.001f;
            float maxNightDarkness = 0.5f;
            if (Main.dayTime)
            {
                BaseColor.R += (byte)((DayColour.R - BaseColor.R) / shiftSpeed);
                BaseColor.G += (byte)((DayColour.G - BaseColor.G) / shiftSpeed);
                BaseColor.B += (byte)((DayColour.B - BaseColor.B) / shiftSpeed);
                sunShaderPos = new Vector2(timeProgression, 0.1f);
                sunPos = Main.screenPosition + new Vector2(timeProgression * Main.screenWidth, 100);
                if (nightHarshness < 1)
                    nightHarshness += nightTransitionSpeed;
                if (globalAlpha < 1)
                    globalAlpha += globalAlphaTransitionSpeed;
            }
            else
            {
                BaseColor.R += (byte)((NightColour.R - BaseColor.R) / shiftSpeed);
                BaseColor.G += (byte)((NightColour.G - BaseColor.G) / shiftSpeed);
                BaseColor.B += (byte)((NightColour.B - BaseColor.B) / shiftSpeed);
                sunShaderPos = new Vector2(1 - timeProgression, 0.1f);
                sunPos = Main.LocalPlayer.Center - new Vector2((timeProgression - 0.5f) * 2 * Main.screenWidth, Main.LocalPlayer.Center.Y - Main.screenHeight / 2.2f);
                if (nightHarshness > maxNightDarkness)
                    nightHarshness -= nightTransitionSpeed;
                if (globalAlpha > 0)
                    globalAlpha -= globalAlphaTransitionSpeed * 10;
            }
            intensityFunction = Math.Abs((float)Math.Sin(Main.time / flunctuationCycle) * Intensity) + baseIntesity;

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Saturation"].IsActive())
            {
                Filters.Scene.Activate("EEMod:Saturation", Vector2.Zero).GetShader();
            }
            Filters.Scene["EEMod:Saturation"].GetShader().UseImageOffset(sunShaderPos).UseIntensity(intensityFunction).UseOpacity(4f).UseProgress(Main.dayTime ? 0 : 1).UseColor(Base, nightHarshness, 0).UseSecondaryColor(BaseColor);
        }
        public void DrawCoralReefsBg()
        {
            return; // nothing being drawn atm
            int maxLoops = 5;
            Color drawColor = Lighting.GetColor((int)(Main.LocalPlayer.Center.X / 16f), (int)(Main.LocalPlayer.Center.Y / 16f)) * alphaBG;
            float scale = 1.5f;
            Vector2 traverseFunction = new Vector2(4000, 1000);
            Vector2 traverse = new Vector2(-Main.LocalPlayer.Center.X / (Main.maxTilesX * 16) * traverseFunction.X, -Main.LocalPlayer.Center.Y / (Main.maxTilesY * 16) * traverseFunction.Y);
            Texture2D CB1 = TextureCache.CoralReefsSurfaceFar; //instance.GetTexture("Backgrounds/CoralReefsSurfaceFar");
            Texture2D CB2 = TextureCache.CoralReefsSurfaceMid; //instance.GetTexture("Backgrounds/CoralReefsSurfaceMid");
            Texture2D CB3 = TextureCache.CB1;
            Rectangle GlobalRect = new Rectangle(0, 0, (int)(CB1.Width * scale), (int)(CB1.Height * scale));
            Rectangle GlobalRectUnscaled = new Rectangle(0, 0, CB1.Width, CB1.Height);
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                for (int i = 0; i < maxLoops; i++)
                {
                    Vector2 Positions = new Vector2((i - ((maxLoops - 1) * 0.5f)) * CB1.Width * scale, traverseFunction.Y / 3f);
                    //  Main.spriteBatch.Draw(CB1, Positions + Main.LocalPlayer.Center - Main.screenPosition + traverse, GlobalRectUnscaled, drawColor, 0f, GlobalRectUnscaled.Size() / 2, scale, SpriteEffects.None, 0f);
                    //   Main.spriteBatch.Draw(CB2, Positions + Main.LocalPlayer.Center - Main.screenPosition + traverse, GlobalRectUnscaled, drawColor, 0f, GlobalRectUnscaled.Size() / 2, scale, SpriteEffects.None, 0f);
                    //  Main.spriteBatch.Draw(CB3, Positions + Main.LocalPlayer.Center - Main.screenPosition + traverse, GlobalRectUnscaled, drawColor, 0f, GlobalRectUnscaled.Size() / 2, scale, SpriteEffects.None, 0f);
                }
            }
        }
        public void UpdateLight()
        {
            for (int i = 0; i < maxNumberOfLights; i++)
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[$"EEMod:LightSource{i}"].IsActive())
                {
                    Filters.Scene.Deactivate($"EEMod:LightSource{i}");
                }
            }
            for (int i = 0; i < maxNumberOfLights; i++)
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[$"EEMod:LightSource{i}"].IsActive())
                {
                    Filters.Scene.Activate($"EEMod:LightSource{i}", Vector2.Zero).GetShader().UseIntensity(0f);
                }
            }
            List<Vector2> listTransformable = new List<Vector2>();
            for (int i = 0; i < LightPoints.Count; i++)
            {
                listTransformable.Add((LightPoints[i]*16 - Main.screenPosition) / new Vector2(Main.screenWidth, Main.screenHeight));
                
                if(i < maxNumberOfLights)
                Filters.Scene[$"EEMod:LightSource{i}"].GetShader().UseImageOffset(listTransformable[i]).UseIntensity(0.0045f).UseColor(ColourPoints[i]);
            }
            LightPoints.Clear();
            ColourPoints.Clear();
            listTransformable.Clear();
        }
        public void DrawBehindTiles(On.Terraria.Main.orig_DrawWoF orig, Main self)
        {
            //UpdateLight();
            DrawNoiseSurfacing();
            DrawLensFlares();
            if (Main.worldName == KeyID.CoralReefs)
                EEWorld.EEWorld.instance.DrawVines();
            //Main.spriteBatch.Draw(Main.magicPixel, ChangingPoints.ForDraw(), Color.Red);


            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ZoneCoralReefs)
            {
                alphaBG += (1 - alphaBG) / 64f;
            }
            else
            {
                alphaBG -= (alphaBG) / 64f;
            }
            for (int i = 0; i < 400; i++)
            {
                if (Main.projectile[i].active)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<Gradient>())
                    {
                        (Main.projectile[i].modProjectile as Gradient).pixelPlacmentHours();
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<CyanoburstTomeKelp>())
                    {
                        (Main.projectile[i].modProjectile as CyanoburstTomeKelp).DrawBehind();
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
            orig(self);

        }
        public void OnSave(On.Terraria.WorldGen.orig_SaveAndQuitCallBack orig, object threadcontext)
        {
            isSaving = true;
            orig(threadcontext);
            isSaving = false;
            //saveInterface?.SetState(null);
        }
        public float seed;
        public float speed;
        public void DrawNoiseSurfacing()
        {
            Vector2 mouseTilePos = Main.MouseWorld / 16;
            Tile tile = Framing.GetTileSafely((int)mouseTilePos.X, (int)mouseTilePos.Y);
            Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos = Vector2.Zero;
            if (tile.active() && tile.type == ModContent.TileType<OrbHolder>())
            {

                speed += 0.002f;
                if (speed % 0.5f < 0.002f)
                {
                    seed = Main.rand.NextFloat(0, 1);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                NoiseSurfacing.Parameters["yCoord"].SetValue(seed);
                NoiseSurfacing.Parameters["t"].SetValue((0.25f - Math.Abs(0.25f - (speed % 0.5f))) * 4);
                NoiseSurfacing.Parameters["xDis"].SetValue(speed % 0.5f);
                NoiseSurfacing.Parameters["noiseTexture"].SetValue(TextureCache.Noise);
                NoiseSurfacing.CurrentTechnique.Passes[0].Apply();
                Vector2 position = new Vector2((int)mouseTilePos.X * 16, (int)mouseTilePos.Y * 16) - new Vector2(tile.frameX / 18 * 16, tile.frameY / 18 * 16);
                Main.spriteBatch.Draw(TextureCache.BlackTex, position.ForDraw() + new Vector2(15, -20), Color.Purple);
                Main.LocalPlayer.GetModPlayer<EEPlayer>().currentAltarPos = position;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
        public void OnUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, GameTime gameTime)
        {

            if (!Main.gameMenu && Main.netMode != NetmodeID.MultiplayerClient && !isSaving)
            {
                alpha = 0;
                loadingChoose = Main.rand.Next(62);
                loadingChooseImage = Main.rand.Next(5);
                Main.numClouds = 10;
                if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                Main.logo2Texture = TextureCache.Terraria_Logo2Texture;
                Main.logoTexture = TextureCache.Terraria_LogoTexture;
                Main.sun2Texture = TextureCache.Terraria_Sun2Texture;
                Main.sun3Texture = TextureCache.Terraria_Sun3Texture;
                Main.sunTexture = TextureCache.Terraria_SunTexture;
            }
            Main.sunTexture = TextureCache.Empty;
            orig(self, gameTime);
        }
        Texture2D Screentexture;
        Texture2D texture2;
        Rectangle Screenframe;
        int Countur;
        int Screenframes;
        int ScreenframeSpeed;
        public static string progressMessage;
        public void DrawSky()
        {
            texture2 = TextureCache.NotBleckScren;
            switch (loadingChooseImage)
            {
                case 0:
                    {
                        Screentexture = TextureCache.DuneShambler;
                        Screenframes = 6;
                        ScreenframeSpeed = 5;
                        break;
                    }

                case 1:
                    {
                        Screentexture = TextureCache.GiantSquid;
                        Screenframes = 3;
                        ScreenframeSpeed = 5;
                        break;
                    }
                case 2:
                    {
                        Screentexture = TextureCache.Clam;
                        Screenframes = 3;
                        ScreenframeSpeed = 5;
                        break;
                    }
                case 3:
                    {
                        Screentexture = TextureCache.Hydros;
                        Screenframes = 8;
                        ScreenframeSpeed = 3;
                        break;
                    }
                case 4:
                    {
                        Screentexture = TextureCache.Seahorse;
                        Screenframes = 5;
                        ScreenframeSpeed = 3;
                        break;
                    }
            }
            if (Countur++ > ScreenframeSpeed)
            {
                Countur = 0;
                frame.Y += Screentexture.Height / Screenframes;
            }
            if (frame.Y >= (Screentexture.Height / Screenframes) * (Screenframes - 1))
            {
                frame.Y = 0;
            }
            Vector2 position = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 30);
            Main.spriteBatch.Draw(texture2, new Vector2(0, 0), new Color(204, 204, 204));
            Main.spriteBatch.Draw(Screentexture, position, new Rectangle(0, frame.Y, Screentexture.Width, Screentexture.Height / Screenframes), new Color(15, 15, 15), 0, new Rectangle(0, frame.Y, Screentexture.Width, Screentexture.Height / Screenframes).Size() / 2, 1, SpriteEffects.None, 0);
        }
        float alpha;
        public static string screenMessageText;

        private void OnDrawMenu(On.Terraria.Main.orig_Draw orig, Main self, GameTime gameTime)
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
            if (isSaving && Main.gameMenu)
            {
                alpha += 0.01f;
                if (alpha > 1)
                {
                    alpha = 1;
                }
                velocity = Vector2.Zero;
                Main.numClouds = 0;
                Main.logo2Texture = TextureCache.Empty;
                Main.logoTexture = TextureCache.Empty;
                Main.sun2Texture = TextureCache.Empty;
                Main.sun3Texture = TextureCache.Empty;
                Main.sunTexture = TextureCache.Empty;
                if (SkyManager.Instance["EEMod:SavingCutscene"] != null) SkyManager.Instance.Activate("EEMod:SavingCutscene", default, new object[0]);
                if (loadingFlag)
                {
                    loadingChoose = Main.rand.Next(65); // numbers from n to n-1
                    loadingChooseImage = Main.rand.Next(5);
                    loadingFlag = false;
                }
                switch (loadingChoose)
                {
                    case 0:
                        screenMessageText = "Watch Out! Dune Shamblers Pop from the ground from time to time!";
                        break;
                    case 1:
                        screenMessageText = "All good sprites made by Nomis";
                        break;
                    case 2:
                        screenMessageText = "Tip of the Day! Loading screens are useless";
                        break;
                    case 3:
                        screenMessageText = "Fear the MS Paint cat";
                        break;
                    case 4:
                        screenMessageText = "Terraria sprites need outlines... except when I make them";
                        break;
                    case 5:
                        screenMessageText = "Remove the Banding";
                        break;
                    case 6:
                        screenMessageText = Main.LocalPlayer.name + " ....huh...what a cruddy name";
                        break;
                    case 7:
                        screenMessageText = "Dont ping everyone you big dumb stupid";
                        break;
                    case 8:
                        screenMessageText = "I'm nothing without attention";
                        break;
                    case 9:
                        screenMessageText = "Why are you even reading this?";
                        break;
                    case 10:
                        screenMessageText = "We actually think we are funny";
                        break;
                    case 11:
                        screenMessageText = "Interitos...whats that?";
                        break;
                    case 12:
                        screenMessageText = "its my style";
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
                        screenMessageText = "Mod is not edgy I swear!";
                        break;
                    case 17:
                        screenMessageText = "All good art made by cynik";
                        break;
                    case 18:
                        screenMessageText = "Im gonna have to mute you for that";
                        break;
                    case 19:
                        screenMessageText = "Gamers rise up!";
                        break;
                    case 20:
                        screenMessageText = "THATS NOT THE CONCEPT";
                        break;
                    case 21:
                        screenMessageText = "caramel popcorn and celeste";
                        break;
                    case 22:
                        screenMessageText = "D D D A G# G F D F G";
                        break;
                    case 23:
                        screenMessageText = "We live in a society";
                        break;
                    case 24:
                        screenMessageText = "Dont mine at night!";
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
                        screenMessageText = "If you liked endless escapade you will love endless escapade premium!";
                        break;
                    case 29:
                        screenMessageText = "When\nBottomText";
                        break;
                    case 30:
                        screenMessageText = "mario in real life";
                        break;
                    case 31:
                        screenMessageText = "All good concept art made by phanta";
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
                        screenMessageText = "trust me there is a lot phesh down in here, the longer the player is in the reefs the more amphibious he will become";
                        break;
                    case 36:
                        screenMessageText = "All good sprites made by daimgamer!";
                        break;
                    case 37:
                        screenMessageText = "All good music made by Universe";
                        break;
                    case 38:
                        screenMessageText = "All good sprites made by Vadim";
                        break;
                    case 39:
                        screenMessageText = "All good sprites made by Zarn";
                        break;
                    case 40:
                        screenMessageText = "All good builds made by Cherry";
                        break;
                    case 41:
                        screenMessageText = "Totally not copying Starlight River";
                        break;
                    case 42:
                        screenMessageText = "Do a Barrel Roll";
                        break;
                    case 43:
                        screenMessageText = "The man behind the laughter";
                        break;
                    case 44:
                        screenMessageText = "All good sprites made by Darkpuppey";
                        break;
                    case 45:
                        screenMessageText = "An apple a day keeps the errors away!";
                        break;
                    case 46:
                        screenMessageText = "Poggers? Poggers.";
                        break;
                    case 47:
                        screenMessageText = $"Totally not sentient AI. By the way, {Main.LocalPlayer.name} is a dumb name";
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
                        screenMessageText = "owo";
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
                        Main.spriteBatch.DrawString(Main.fontMouseText, progressMessage.ToString(), new Vector2(textPosition2Left, Main.screenHeight / 2 + 200), Color.AliceBlue * alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    }
                    float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
                    Main.spriteBatch.DrawString(Main.fontDeathText, screenMessageText, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), Color.White * alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.End();
            }
            else
            {
                if (!Main.dedServ)
                {
                    loadingChoose = 47;//Main.rand.Next(64);
                    loadingChooseImage = Main.rand.Next(5);
                    Main.numClouds = 10;
                    if (SkyManager.Instance["EEMod:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("EEMod:SavingCutscene", new object[0]);
                    Main.logo2Texture = TextureCache.Terraria_Logo2Texture;
                    Main.logoTexture = TextureCache.Terraria_LogoTexture;
                    Main.sun2Texture = TextureCache.Terraria_Sun2Texture;
                    Main.sun3Texture = TextureCache.Terraria_Sun3Texture;
                    Main.sunTexture = TextureCache.Terraria_SunTexture;
                }

            }
        }
        
        private void Main_DrawBackground(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            Type spritebatchtype = typeof(SpriteBatch);

            if (!c.TryGotoNext(i => i.MatchLdloc(18)))
                throw new Exception("Ldloc for local variable 18 not found");

            MethodInfo call1 = spritebatchtype.GetMethod("Draw", new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color) });

            if (!c.TryGotoNext(i => i.MatchCallvirt(call1)))
                throw new Exception("No call found for SpriteBatch.Draw(Texture2D, Vector2, Rectangle?, Color)");

            // 1st call
            c.Remove();
            c.Emit(OpCodes.Ldloc, 13); // array
            c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, int[]>>((spritebatch, texture, pos, sourcerectangle, color, array) =>
            {
                if (array[4] != 135)
                    spritebatch.Draw(texture, pos, sourcerectangle, color);
            });
            // 2nd call
            // getting to the else
            MethodInfo lightningnoretroget = typeof(Lighting).GetProperty(nameof(Lighting.NotRetro)).GetGetMethod();
            if (!c.TryGotoNext(i => i.MatchCallOrCallvirt(lightningnoretroget)))
                throw new Exception("Call for the get method of the property Lighting.NoRetro not found");
            // finding the call
            MethodInfo draw = spritebatchtype.GetMethod("Draw", new Type[] { typeof(Texture2D), typeof(Vector2), typeof(Rectangle?), typeof(Color), typeof(float), typeof(Vector2), typeof(float), typeof(SpriteEffects), typeof(float) });
            for (int k = 0; k < 4; k++) // 4 calls
            {
                if (!c.TryGotoNext(i => i.MatchCallvirt(draw)))
                    throw new Exception($"Call number {k} not found");

                c.Remove();
                c.Emit(OpCodes.Ldloc, 13); // array
                c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, float, Vector2, float, SpriteEffects, float, int[]>>((spritebatch, texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth, array) =>
                {
                    if (array[5] != 126)
                        spritebatch.Draw(texture, position, sourcerectangle, color, rotation, origin, scale, effects, layerdepth);
                });
            }

            // 3rd call
            if (!c.TryGotoNext(i => i.MatchLdloc(20))) throw new Exception("Ldloc for local variable 20 (flag4) not found"); // flag4
            if (!c.TryGotoNext(i => i.MatchCallvirt(call1))) throw new Exception("'Last' SpriteBatch.Draw call not found"); // same overload
            c.Remove();
            c.Emit(OpCodes.Ldloc, 13); // array
            c.EmitDelegate<Action<SpriteBatch, Texture2D, Vector2, Rectangle?, Color, int[]>>((spritebatch, texture, position, sourcerectangle, color, array) =>
            {
                if (array[6] != 186)
                    spritebatch.Draw(texture, position, sourcerectangle, color);
            });
        }

        public static void WorldGen_SmashAltar(On.Terraria.WorldGen.orig_SmashAltar orig, int i, int j)
        {
            orig(i, j);
            EEPlayer.moralScore -= 50;
            Main.NewText(EEPlayer.moralScore);
        }
        /*private static void ILSaveWorldTiles(ILContext il)
{
    ILCursor c = new ILCursor(il);
    PropertyInfo statusText = typeof(Main).GetProperty(nameof(Main.statusText));
    MethodInfo set = statusText.GetSetMethod();

    if (!c.TryGotoNext(i => i.MatchCall(set)))
        throw new Exception();

    c.EmitDelegate<Func<string, string>>((originalText) =>
    {
        return originalText;
    });
}*/
        /*
        //private static void ModifyColor(ref Color color, byte val)
        //{
        //
        //}
        // private delegate void colorrefdelegate(ref Color color, byte val);
        private delegate void modifyingdelegate(Main instance, ref int focusmenu, ref int selectedmenu, ref int num2, ref int num4, ref int[] array4, ref byte[] array6, ref string[] array9, ref bool[] array, ref int num5, ref bool flag);
#pragma warning disable ChangeMagicNumberToID // Change magic numbers into appropriate ID values
        private static void GenkaiMenu(Main instance, ref int focusMenu, ref int selectedMenu, ref int num2, ref int num4, ref int[] array4, ref byte[] array6, ref string[] array9, ref bool[] array, ref int num5, ref bool flag5)
        {
            num2 = 200;
            num4 = 60;
            int offset = -10;
            array4[2] = 30 + offset - 1; //30 - 20; // 30
            array4[3] = 30 + offset - 3 - 1; //30 - 10; // 30
            array6[3] = 2; //2; // rarity // 2
            array4[4] = 70; // 70
            array4[5] = -40 + offset / 2 - 1; // -40 - 10;
            array6[5] = 5;
            if (focusMenu == 2)
            {
                array9[0] = Language.GetTextValue("UI.NormalDescriptionFlavor");
                array9[1] = Language.GetTextValue("UI.NormalDescription");
            }
            else if (focusMenu == 3)
            {
                array9[0] = Language.GetTextValue("UI.ExpertDescriptionFlavor");
                array9[1] = Language.GetTextValue("UI.ExpertDescription");
            }
            else if (focusMenu == 5) // Genkai's
            {
                array9[0] = "Not for easily angried";
                array9[1] = "(What'll it be? Who knows, find out ;])";
            }
            else
            {
#pragma warning disable CS0618 // El tipo o el miembro est�n obsoletos
                array9[0] = Lang.menu[32].Value;
#pragma warning restore CS0618 // El tipo o el miembro est�n obsoletos
            }
            array[0] = true;
            array[1] = true;

            array9[2] = Language.GetTextValue("UI.Normal");
            array9[3] = Language.GetTextValue("UI.Expert");
            array9[4] = Language.GetTextValue("UI.Back");
            array9[5] = "Genkai"; // Genkai
            num5 = 6;
            if (selectedMenu == 2)
            {
                Main.expertMode = false;
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = 7;
                if (Main.SettingsUnlock_WorldEvil)
                {
                    Main.menuMode = -71;
                }
            }
            else if (selectedMenu == 3)
            {
                Main.expertMode = true;
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = 7;
                if (Main.SettingsUnlock_WorldEvil)
                {
                    Main.menuMode = -71;
                }
            }
            else if (selectedMenu == 5) // Genkai's
            {
                Main.PlaySound(10, -1, -1, 1, 1f, 0f);
                Main.menuMode = Main.SettingsUnlock_WorldEvil ? -71 : 7;
                Main.expertMode = true;
                EEWorld.EEWorld.GenkaiMode = true;
            }
            else if (selectedMenu == 4 || flag5)
            {
                flag5 = false;
                Main.PlaySound(11, -1, -1, 1, 1f, 0f);
                Main.menuMode = 16;
            }
            Main.clrInput();
        }
#pragma warning restore ChangeMagicNumberToID // Change magic numbers into appropriate ID values
        */
    }
}
