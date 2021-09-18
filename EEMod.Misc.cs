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
using EEMod.NPCs.CoralReefs;
using EEMod.Tiles.Furniture;
using EEMod.VerletIntegration;
using EEMod.Systems.EEGame;
using Terraria.Audio;
using Terraria.GameContent;

namespace EEMod
{
    partial class EEMod
    {
        public static double worldSurface;
        public static double worldSurfaceLow;
        public static double worldSurfaceHigh;
        public static double rockLayer;
        public static double rockLayerLow;
        public static double rockLayerHigh;
        public static int _lastSeed;
        public static Texture2D ScTex;
        public static Effect NoiseSurfacing;
        public static int AscentionHandler;
        public static int startingTextHandler;
        public static bool isAscending;

        [FieldInit(FieldInitType.ArrayMultipleLengths, arrayLengths: new int[] { 3, 200, 2 })]
        public static Vector2[,,] lol1 = new Vector2[3, 200, 2];

        private int delay;
        private float pauseShaderTImer;
        public SpaceInvaders simpleGame;
        public int lerps;
        private float alphas;
        private int delays;
        public Verlet verlet;
        private bool mode = true;
        bool bufferVariable;
        private float rotationBuffer;
        private float rotGoto;
        public string text;
        float counter;
        bool IsTraining;

        //MechanicPort
        public void UpdateVerlet()
        {
            ScTex = Main.screenTarget;
            if (ActivateVerletEngine.JustPressed)
                mode = !mode;
            if (mode)
                verlet.Update();
            if (delays > 0)
                delays--;
        }

        public void DrawZipline()
        {
            Vector2 PylonBegin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
            Vector2 PylonEnd = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;
            Main.spriteBatch.Begin();
            Main.spriteBatch.Draw(Assets.Request<Texture2D>("EEMod/Items/ZipCarrier2").Value, Main.LocalPlayer.position.ForDraw() + new Vector2(0, 6), new Rectangle(0, 0, 2, 16), Color.White, 0, new Vector2(2, 16) / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(Assets.Request<Texture2D>("EEMod/Items/ZipCarrier").Value, Main.LocalPlayer.position.ForDraw(), new Rectangle(0, 0, 18, 8), Color.White, (PylonEnd - PylonBegin).ToRotation(), new Vector2(18, 8) / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.End();
        }

        //This needs to be fixed. 
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

                            if (Inspect.JustPressed && delays == 0)
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
            simpleGame = simpleGame ?? new SpaceInvaders();
            simpleGame.Update(gameTime);
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.dead)
                {
                    if (Inspect.JustPressed && player.GetModPlayer<EEPlayer>().playingGame == true)
                    {
                        // player.GetModPlayer<EEPlayer>().playingGame = false;
                        // player.webbed = false;
                        simpleGame.EndGame();
                        break;
                    }
                    if (Inspect.JustPressed && Framing.GetTileSafely((int)player.Center.X / 16, (int)player.Center.Y / 16).type == ModContent.TileType<BlueArcadeMachineTile>() && player.GetModPlayer<EEPlayer>().playingGame == false && PlayerExtensions.GetSavings(player) >= 2500)
                    {
                        simpleGame = new SpaceInvaders();
                        SoundEngine.PlaySound(SoundID.CoinPickup, Main.LocalPlayer.Center);
                        player.BuyItem(2500);
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
            var font = style == 0 ? FontAssets.DeathText.Value : FontAssets.MouseText.Value;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            //float textPositionRight = position.X + textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }

        internal void DoPostDrawTiles(SpriteBatch spriteBatch) => AfterTiles?.Invoke(spriteBatch);

       

        public static void DrawText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.titleText;
            Color color = Color.White * alpha;
            /*if (Main.worldName == KeyID.Sea)
            {
                text = "The Ocean";
                color = new Color((1 - alpha), (1 - alpha), 1) * alpha;
            }*/
            /*if (Main.ActiveWorldFileData.Name == KeyID.Pyramids)
            {
                text = "The Pyramids";
                color = Color.Yellow * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                text = "The Coral Reefs";
                color = Color.Blue * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoIsland)
            {
                text = "The Volcano";
                color = Color.OrangeRed * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoInside)
            {
                text = "The Volcano's Core";
                color = Color.Red * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Island)
            {
                text = "Tropical Island";
                color = Color.GreenYellow * alpha;
            }*/

            Texture2D Outline = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("UI/Outline").Value;
            Texture2D OceanScreen = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Seamap/SeamapAssets/OceanScreen").Value;
            if (FontAssets.MouseText.Value != null)
            {
                if (Main.worldName == KeyID.Sea)
                {
                    Vector2 drawpos = new Vector2(Main.screenWidth / 2, 100);

                    Main.spriteBatch.Begin();
                    Main.spriteBatch.Draw(OceanScreen, drawpos, new Rectangle(0, 0, OceanScreen.Width, OceanScreen.Height), Color.White * alpha, 0, OceanScreen.TextureCenter(), 1, SpriteEffects.None, 0);
                    Main.spriteBatch.End();
                }

                /*Vector2 textSize = Main.fontDeathText.MeasureString(text);
                float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
                float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
                Vector2 drawpos = new Vector2(Main.screenWidth / 2, 100);
                if (Main.worldName == KeyID.Sea)
                    Main.spriteBatch.Draw(OceanScreen, drawpos, new Rectangle(0, 0, OceanScreen.Width, OceanScreen.Height), Color.White * alpha, 0, OceanScreen.TextureCenter(), 1, SpriteEffects.None, 0);
                if (Main.worldName == KeyID.Sea)
                {
                    Main.spriteBatch.Draw(OceanScreen, drawpos, new Rectangle(0, 0, OceanScreen.Width, OceanScreen.Height), Color.White * alpha, 0, OceanScreen.TextureCenter(), 1, SpriteEffects.None, 0);
                }
                else
                {
                    Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(Outline, new Vector2(textPositionLeft - 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, Outline.TextureCenter(), 1, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(Outline, new Vector2(textPositionRight + 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, Outline.TextureCenter(), 1, SpriteEffects.FlipHorizontally, 0);
                }*/
            }
        }
    }
}
