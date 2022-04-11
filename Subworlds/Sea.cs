using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using EEMod.ID;
using EEMod.Tiles;
using EEMod.VerletIntegration;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage;
using System;
using EEMod.Systems.Noise;
using System.Collections.Generic;
using EEMod.Autoloading;
using Terraria.WorldBuilding;
using EEMod.Seamap.Core;
using Terraria.Graphics.Effects;
using System.Diagnostics;
using Terraria.Audio;
using EEMod.Seamap.Content;
using EEMod.Seamap.Content.Islands;
using EEMod.Seamap;
using EEMod;
using Terraria.UI.Chat;
//using EEMod.Subworlds.CoralReefs;
using SubworldLibrary;
using Terraria.IO;
using Terraria.GameContent;

namespace EEMod.Subworlds
{
    public class Sea : Subworld
    {
        public override int Width => 600;
        public override int Height => 600;

        public override string Name => "Sea";

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new SeaGenPass(progress =>
            {
                progress.Message = "Spawning Seamap"; //Sets the text above the worldgen progress bar

	    		Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
	    		Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            })
        };

        public void ReturnHome(Player player)
        {
            SubworldSystem.Exit();
        }

        public override void DrawMenu(GameTime gameTime)
        {
            Main.spriteBatch.Begin();

            switch (EEMod.loadingChooseImage)
            {
                case 0:
                    ModContent.GetInstance<EEMod>().texture2 = ModContent.Request<Texture2D>("EEMod/UI/LoadingScreenImages/LoadingScreen1").Value;
                    break;
                case 1:
                    ModContent.GetInstance<EEMod>().texture2 = ModContent.Request<Texture2D>("EEMod/UI/LoadingScreenImages/LoadingScreen2").Value;
                    break;
                case 2:
                    ModContent.GetInstance<EEMod>().texture2 = ModContent.Request<Texture2D>("EEMod/UI/LoadingScreenImages/LoadingScreen3").Value;
                    break;
                default:
                    ModContent.GetInstance<EEMod>().texture2 = ModContent.Request<Texture2D>("EEMod/UI/LoadingScreenImages/LoadingScreen4").Value;
                    break;
            }
            switch (EEMod.loadingChoose)
            {
                default:
                {
                    ModContent.GetInstance<EEMod>().texture = ModContent.Request<Texture2D>("Terraria/Images/UI/Sunflower_Loading").Value;
                    ModContent.GetInstance<EEMod>().frames = 19;
                    ModContent.GetInstance<EEMod>().frameSpeed = 3;
                    break;
                }

                case 1:
                {
                    ModContent.GetInstance<EEMod>().texture = ModContent.Request<Texture2D>("EEMod/NPCs/SurfaceReefs/HermitCrab").Value;
                    ModContent.GetInstance<EEMod>().frames = 4;
                    ModContent.GetInstance<EEMod>().frameSpeed = 5;
                    break;
                }
                case 2:
                {
                    ModContent.GetInstance<EEMod>().texture = ModContent.Request<Texture2D>("EEMod/NPCs/SurfaceReefs/Seahorse").Value;
                    ModContent.GetInstance<EEMod>().frames = 7;
                    ModContent.GetInstance<EEMod>().frameSpeed = 4;
                    break;
                }
                case 3:
                {
                    ModContent.GetInstance<EEMod>().texture = ModContent.Request<Texture2D>("EEMod/NPCs/LowerReefs/Lionfish").Value;
                    ModContent.GetInstance<EEMod>().frames = 8;
                    ModContent.GetInstance<EEMod>().frameSpeed = 10;
                    break;
                }
                case 4:
                {
                    ModContent.GetInstance<EEMod>().texture = ModContent.Request<Texture2D>("EEMod/NPCs/ThermalVents/MechanicalShark").Value;
                    ModContent.GetInstance<EEMod>().frames = 6;
                    ModContent.GetInstance<EEMod>().frameSpeed = 10;
                    break;
                }
            }
            if (ModContent.GetInstance<EEMod>().Countur++ > ModContent.GetInstance<EEMod>().frameSpeed)
            {
                ModContent.GetInstance<EEMod>().Countur = 0;
                ModContent.GetInstance<EEMod>().frame2.Y += ModContent.GetInstance<EEMod>().texture.Height / ModContent.GetInstance<EEMod>().frames;
            }
            if (ModContent.GetInstance<EEMod>().frame2.Y >= ModContent.GetInstance<EEMod>().texture.Height / ModContent.GetInstance<EEMod>().frames * (ModContent.GetInstance<EEMod>().frames - 1))
            {
                ModContent.GetInstance<EEMod>().frame2.Y = 0;
            }

            Vector2 position = new Vector2(Main.graphics.GraphicsDevice.Viewport.Width / 2, Main.graphics.GraphicsDevice.Viewport.Height / 2 + 30);

            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().texture2,
                new Rectangle(Main.graphics.GraphicsDevice.Viewport.Width / 2, Main.graphics.GraphicsDevice.Viewport.Height / 2, Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height),
                ModContent.GetInstance<EEMod>().texture2.Bounds, new Color(204, 204, 204), 0, origin: new Vector2(ModContent.GetInstance<EEMod>().texture2.Width / 2, ModContent.GetInstance<EEMod>().texture2.Height / 2), SpriteEffects.None, 0);

            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().texture, position, new Rectangle(0, ModContent.GetInstance<EEMod>().frame2.Y, ModContent.GetInstance<EEMod>().texture.Width, ModContent.GetInstance<EEMod>().texture.Height / ModContent.GetInstance<EEMod>().frames), new Color(0, 0, 0), 0, new Rectangle(0, ModContent.GetInstance<EEMod>().frame2.Y, ModContent.GetInstance<EEMod>().texture.Width, ModContent.GetInstance<EEMod>().texture.Height / ModContent.GetInstance<EEMod>().frames).Size() / 2, 1, SpriteEffects.None, 0);

            float tempAlpha = ModContent.GetInstance<EEMod>().alpha;
            tempAlpha = 1 - (Math.Abs((Main.graphics.GraphicsDevice.Viewport.Width / 2) - (FontAssets.DeathText.Value.MeasureString(EEMod.screenMessageText).X / 2) - ModContent.GetInstance<EEMod>().textPositionLeft) / (Main.graphics.GraphicsDevice.Viewport.Width / 2f));

            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.DeathText.Value, EEMod.screenMessageText, new Vector2(ModContent.GetInstance<EEMod>().textPositionLeft, Main.graphics.GraphicsDevice.Viewport.Height / 2 - 100), Color.White * tempAlpha, 0f, Vector2.Zero, Vector2.One);

            Main.spriteBatch.End();

            return;
        }

        public override void OnEnter()
        {
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().cutSceneTriggerTimer = 0;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().triggerSeaCutscene = false;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().speedOfPan = 0;

            base.OnEnter();
        }

        public override void OnExit()
        {
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().cutSceneTriggerTimer = 0;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().triggerSeaCutscene = false;
            Main.LocalPlayer.GetModPlayer<ShipyardPlayer>().speedOfPan = 0;

            base.OnExit();
        }
    }

    public class SeaSystem : ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(ModContent.GetInstance<EEMod>(), "Assets/Music/Seamap");
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            return SubworldLibrary.SubworldSystem.IsActive<Sea>();
        }
    }

    public class SeaGenPass : GenPass
    {
        private Action<GenerationProgress> method;

        public SeaGenPass(Action<GenerationProgress> method) : base("", 1)
        {
            this.method = method;
        }

        public SeaGenPass(float weight, Action<GenerationProgress> method) : base("", weight)
        {
            this.method = method;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            method(progress);
        }
    }
}