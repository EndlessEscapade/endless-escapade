using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using EEMod.Autoloading;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Projectiles;
using EEMod.NPCs;
using EEMod.Tiles.Furniture;
using EEMod.VerletIntegration;
using EEMod.Systems.EEGame;
using Terraria.Audio;
using Terraria.GameContent;
using EEMod.Subworlds;
using Terraria.UI.Chat;
using Terraria.Graphics.Effects;
using Terraria.Graphics;
using EEMod.Config;
using Terraria.GameInput;

namespace EEMod
{
    partial class EEMod
    {
        public static int _lastSeed;
        public static Texture2D ScTex;
        public static int startingTextHandler;

        public IceHockey simpleGame;

        public int lerps;
        private float alphas;
        private int delays;
        public Verlet verlet;
        private bool mode = true;
        public string text;

        //TODO move this to the verlet system
        public void UpdateVerlet()
        {
            ScTex = Main.screenTarget;

            if (mode)
                verlet.Update();
            if (delays > 0)
                delays--;
        }

        //TODO move this to a separate ModSystem
        public void DrawZipline()
        {
            Vector2 PylonBegin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
            Vector2 PylonEnd = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;

            Main.spriteBatch.Begin();

            Main.spriteBatch.Draw(Assets.Request<Texture2D>("EEMod/Items/ZipCarrier2").Value, Main.LocalPlayer.position.ForDraw() + new Vector2(0, 6), new Rectangle(0, 0, 2, 16), Color.White, 0, new Vector2(2, 16) / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(Assets.Request<Texture2D>("EEMod/Items/ZipCarrier").Value, Main.LocalPlayer.position.ForDraw(), new Rectangle(0, 0, 18, 8), Color.White, (PylonEnd - PylonBegin).ToRotation(), new Vector2(18, 8) / 2, Vector2.One, SpriteEffects.None, 0);
            
            Main.spriteBatch.End();
        }

        //TODO move this to a separate system
        public void UpdateGame(GameTime gameTime)
        {
            lerps++;
            if (delays > 0)
            {
                delays--;
            }

            float lerpLol = Math.Abs((float)Math.Sin(lerps / 50f));
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && (npc.type == ModContent.NPCType<OrbCollection>() || npc.type == ModContent.NPCType<SpikyOrb>()))
                {
                    float Dist = Vector2.Distance(npc.Center, Main.LocalPlayer.Center);
                    if (Dist < 1000)
                    {
                        if (!Main.LocalPlayer.GetModPlayer<EEPlayer>().isPickingUp)
                        {
                            UIText("Pick Up?", Color.White * alphas, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 50), 1);
                        }
                        if (Dist < 100)
                        {
                            if (alphas < 1)
                            {
                                alphas += 0.01f;
                            }

                            if (PlayerInput.Triggers.JustPressed.Jump && delays == 0)
                            {
                                var modp = Main.LocalPlayer.GetModPlayer<EEPlayer>();
                                if (!modp.isPickingUp)
                                    npc.ai[1] = Main.myPlayer;
                                modp.isPickingUp = !modp.isPickingUp;
                                delays = 120;
                            }
                        }
                        else
                        {
                            if (alphas > 0)
                            {
                                alphas -= 0.01f;
                            }
                        }
                    }
                }
            }

            simpleGame = simpleGame ?? new IceHockey();
            simpleGame.Update(gameTime);
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    if (PlayerInput.Triggers.JustPressed.Jump && player.GetModPlayer<EEPlayer>().playingGame == true)
                    {
                        player.GetModPlayer<EEPlayer>().playingGame = false;
                        player.webbed = false;
                        simpleGame.EndGame();
                        break;
                    }

                    if (PlayerInput.Triggers.JustPressed.Jump && Framing.GetTileSafely((int)player.Center.X / 16, (int)player.Center.Y / 16).TileType == ModContent.TileType<AirHockeyTableTile>() && player.GetModPlayer<EEPlayer>().playingGame == false && PlayerExtensions.GetSavings(player) >= 2500)
                    {
                        simpleGame = new IceHockey();
                        SoundEngine.PlaySound(SoundID.CoinPickup, Main.LocalPlayer.Center);
                        simpleGame.StartGame(i);
                        player.GetModPlayer<EEPlayer>().playingGame = true;

                        break;
                    }
                }
            }
        }

        //should be in helper class
        public static void UIText(string text, Color colour, Vector2 position, int style)
        {
            if (text == null) text = "";

            var font = style == 0 ? FontAssets.DeathText.Value : FontAssets.MouseText.Value;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            //float textPositionRight = position.X + textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        internal void DoPostDrawTiles(SpriteBatch spriteBatch) => AfterTiles?.Invoke(spriteBatch);

        public void DrawRef()
        {
            RenderTarget2D buffer = Main.screenTarget;

            Main.graphics.GraphicsDevice.SetRenderTarget(null);

            Color[] texdata = new Color[buffer.Width * buffer.Height];

            buffer.GetData(texdata);

            Texture2D screenTex = new Texture2D(Main.graphics.GraphicsDevice, buffer.Width, buffer.Height);

            screenTex.SetData(texdata);

            Main.spriteBatch.Draw(screenTex, Main.LocalPlayer.Center.ForDraw(), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * 0.3f, 0f, new Rectangle(0, 0, Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height).Size() / 2, 1, SpriteEffects.FlipVertically, 0);
            Main.graphics.GraphicsDevice.SetRenderTarget(Main.screenTarget);
        }

        private static void ModifyWaterColor(ref VertexColors colors)
        {
            Color c = Color.White;

            colors.TopLeftColor = c;
            colors.TopRightColor = c;
            colors.BottomLeftColor = c;
            colors.BottomRightColor = c;
        }

        //TODO move these three to their own ModSystem
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

            Rectangle[] rects = { new Rectangle(0, 0, ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/SunRing").Value.Width, ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/SunRing").Value.Height), new Rectangle(0, 0, ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare").Value.Width, ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare").Value.Height) };

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            if (EEModConfigClient.Instance.BetterLighting)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Projectiles/Nice").Value, _sunPos - Main.screenPosition, new Rectangle(0, 0, 174, 174), Color.White * .5f * _globalAlpha * (_intensityFunction * 0.36f), (float)Math.Sin(Main.time / 540f), new Vector2(87), 10f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare").Value, _sunPos - Main.screenPosition + new Vector2(5, 28 + (float)num10 * 250), rects[1], Color.White * _globalAlpha * _intensityFunction, (float)Math.Sin(Main.time / 540f), new Vector2(rects[1].Width, rects[1].Height) / 2, 1.3f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/SunRing").Value, _sunPos - Main.screenPosition + new Vector2(0, 37 + (float)num10 * 250), rects[0], Color.White * .7f * _globalAlpha * (_intensityFunction * 0.36f), (float)Math.Sin(Main.time / 5400f), new Vector2(rects[0].Width, rects[0].Height) / 2, 1f, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void DrawLensFlares()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            if (EEModConfigClient.Instance.BetterLighting && Main.worldName != KeyID.CoralReefs)
            {
                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare2").Value, _sunPos - Main.screenPosition + new Vector2(-400, 400), new Rectangle(0, 0, 174, 174), Color.White * .7f * _globalAlpha * (_intensityFunction * 0.36f), 0f, new Vector2(87), 1f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/LensFlare2").Value, _sunPos - Main.screenPosition + new Vector2(-800, 800), new Rectangle(0, 0, 174, 174), Color.White * .8f * _globalAlpha * (_intensityFunction * 0.36f), 0f, new Vector2(87), .5f, SpriteEffects.None, 0);
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
                _baseColor.R += (byte)((DayColour.R - _baseColor.R) / shiftSpeed);
                _baseColor.G += (byte)((DayColour.G - _baseColor.G) / shiftSpeed);
                _baseColor.B += (byte)((DayColour.B - _baseColor.B) / shiftSpeed);
                _sunShaderPos = new Vector2(timeProgression, 0.1f);
                _sunPos = Main.screenPosition + new Vector2(timeProgression * Main.screenWidth, 100);

                if (_nightHarshness < 1)
                {
                    _nightHarshness += nightTransitionSpeed;
                }

                if (_globalAlpha < 1)
                {
                    _globalAlpha += globalAlphaTransitionSpeed;
                }
            }
            else
            {
                _baseColor.R += (byte)((NightColour.R - _baseColor.R) / shiftSpeed);
                _baseColor.G += (byte)((NightColour.G - _baseColor.G) / shiftSpeed);
                _baseColor.B += (byte)((NightColour.B - _baseColor.B) / shiftSpeed);
                _sunShaderPos = new Vector2(1 - timeProgression, 0.1f);
                _sunPos = Main.LocalPlayer.Center - new Vector2((timeProgression - 0.5f) * 2 * Main.screenWidth, Main.LocalPlayer.Center.Y - Main.screenHeight / 2.2f);

                if (_nightHarshness > maxNightDarkness)
                {
                    _nightHarshness -= nightTransitionSpeed;
                }

                if (_globalAlpha > 0)
                {
                    _globalAlpha -= globalAlphaTransitionSpeed * 10;
                }
            }

            _intensityFunction = Math.Abs((float)Math.Sin(Main.time / flunctuationCycle) * Intensity) + baseIntesity;

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Saturation"].IsActive())
            {
                Filters.Scene.Activate("EEMod:Saturation", Vector2.Zero).GetShader();
            }

            Filters.Scene["EEMod:Saturation"].GetShader().UseImageOffset(_sunShaderPos).UseIntensity(_intensityFunction).UseOpacity(4f).UseProgress(Main.dayTime ? 0 : 1).UseColor(Base, _nightHarshness, 0).UseSecondaryColor(_baseColor);
        }

        //TODO move to a helper class
        private static void ClampScreenPositionToWorld(int maxRight, int maxBottom)
        {
            Vector2 vector = new Vector2(0, 0) - Main.GameViewMatrix.Translation;
            Vector2 vector2 = new Vector2(maxRight - (float)Main.screenWidth / Main.GameViewMatrix.Zoom.X, maxBottom - (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y) - Main.GameViewMatrix.Translation;

            vector = Utils.Round(vector);
            vector2 = Utils.Round(vector2);

            Main.screenPosition = Vector2.Clamp(Main.screenPosition, vector, vector2);
        }
    }
}
