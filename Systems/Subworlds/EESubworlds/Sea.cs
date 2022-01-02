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
using EEMod.Seamap.SeamapContent;
using Terraria.Graphics.Effects;
using System.Diagnostics;

namespace EEMod.Systems.Subworlds.EESubworlds
{
    public class Sea : Subworld
    {
        public override Point Dimensions => new Point(400, 405);

        public override Point SpawnTile => new Point(234, 92);

        public override string Name => "Sea";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {
            Main.worldSurface = 200;
        }

        internal override void PlayerUpdate(Player player)
        {
            #region Opening cutscene for seamap
            if (player.GetModPlayer<EEPlayer>().quickOpeningFloat > 0.01f)
            {
                player.GetModPlayer<EEPlayer>().quickOpeningFloat -= player.GetModPlayer<EEPlayer>().quickOpeningFloat / 20f;
            }
            else
            {
                player.GetModPlayer<EEPlayer>().quickOpeningFloat = 0;
            }

            Filters.Scene["EEMod:SeaOpening"].GetShader().UseIntensity(player.GetModPlayer<EEPlayer>().quickOpeningFloat);

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:SeaOpening"].IsActive())
            {
                Filters.Scene.Activate("EEMod:SeaOpening", player.Center).GetShader().UseIntensity(player.GetModPlayer<EEPlayer>().quickOpeningFloat);
            }

            if (player.GetModPlayer<EEPlayer>().noU)
            {
                player.GetModPlayer<EEPlayer>().titleText -= 0.005f;
            }
            else
            {
                player.GetModPlayer<EEPlayer>().titleText += 0.005f;
            }

            if (player.GetModPlayer<EEPlayer>().titleText >= 1)
            {
                player.GetModPlayer<EEPlayer>().noU = true;
            }
            #endregion

            player.GetModPlayer<EEPlayer>().seamapUpdateCount++;

            #region Placing SeamapObjects.Islands
            if (player.GetModPlayer<EEPlayer>().seamapUpdateCount == 1)
            {
                Seamap.SeamapContent.Seamap.InitializeSeamap();
            }
            #endregion

            #region If ship crashes
            if (SeamapObjects.localship.shipHelth <= 0)
            {
                if (EEPlayer.prevKey == player.GetModPlayer<EEPlayer>().baseWorldName || EEPlayer.prevKey == "Main")
                {
                    ReturnHome(player);
                }
                else
                {
                    player.GetModPlayer<EEPlayer>().Initialize();

                    player.GetModPlayer<EEPlayer>().arrowFlag = false;

                    player.GetModPlayer<EEPlayer>().SM.Return(EEPlayer.prevKey);
                }
            }
            #endregion

            #region Warping to islands
            if (!player.GetModPlayer<EEPlayer>().arrowFlag)
            {
                player.GetModPlayer<EEPlayer>().arrowFlag = true;
            }

            player.position = player.oldPosition;
            player.invis = true;

            player.AddBuff(BuffID.Cursed, 100000);
            Main.GameZoomTarget = 1f;

            bool isCollidingWithAnyIsland = false;
            foreach (SeamapObject obj in SeamapObjects.SeamapEntities)
            {
                if (obj is Island)
                {
                    Island island = obj as Island;

                    EEPlayer.prevKey = KeyID.Sea;

                    player.ClearBuff(BuffID.Cursed);
                    player.ClearBuff(BuffID.Invisibility);

                    if (island.Hitbox.Intersects(SeamapObjects.localship.Hitbox) && EEMod.Inspect.JustPressed)
                    {
                        island.Interact();
                    }
                }
            }

            if (!isCollidingWithAnyIsland)
            {
                player.GetModPlayer<EEPlayer>().subTextAlpha -= 0.02f;

                if (player.GetModPlayer<EEPlayer>().subTextAlpha <= 0)
                {
                    player.GetModPlayer<EEPlayer>().subTextAlpha = 0;
                }
            }
            #endregion

            #region Warp cutscene
            if (player.GetModPlayer<EEPlayer>().importantCutscene)
            {
                EEMod.Noise2D.NoiseTexture = ModContent.Request<Texture2D>("EEMod/Textures/Noise/noise").Value;
                Filters.Scene["EEMod:Noise2D"].GetShader().UseOpacity(player.GetModPlayer<EEPlayer>().cutSceneTriggerTimer / 180f);

                if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Noise2D"].IsActive())
                {
                    Filters.Scene.Activate("EEMod:Noise2D", player.Center).GetShader().UseOpacity(0);
                }

                player.GetModPlayer<EEPlayer>().cutSceneTriggerTimer++;

                if (player.GetModPlayer<EEPlayer>().cutSceneTriggerTimer > 180)
                {
                    player.GetModPlayer<EEPlayer>().Initialize();
                    Filters.Scene.Deactivate("EEMod:Noise2D");
                    SubworldManager.EnterSubworld<CoralReefs>(); // coral reefs
                }
            }
            #endregion
        }

        public void ReturnHome(Player player)
        {
            player.GetModPlayer<EEPlayer>().Initialize();

            player.GetModPlayer<EEPlayer>().SM.Return(KeyID.BaseWorldName);
        }
    }
}