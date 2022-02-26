using EEMod.Buffs.Debuffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Net;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.Projectiles;
using EEMod.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEWorld.EEWorld;
using static Terraria.ModLoader.ModContent;
using ReLogic.Graphics;
using EEMod.Seamap.Content;
using EEMod.Seamap.Core;
using EEMod.Autoloading;
using EEMod.Systems.Subworlds.EESubworlds;
using System.Diagnostics;
using EEMod.Tiles.Furniture;
using Terraria.Audio;
using Terraria.ModLoader.IO;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        public int timerForCutscene;
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public float subTextAlpha;
        public bool noU;
        public bool triggerSeaCutscene;
        public int cutSceneTriggerTimer;
        public int coralReefTrans;
        public int seamapUpdateCount;

        public bool IncreaseStarFall;

        public static string prevKey = "Main";

        public string baseWorldName;

        public bool hasLoadedIntoWorld;

        public int powerLevel;
        public float maxPowerLevel;

        public Vector2 myLastBoatPos;

        public bool lastKeySeamap;

        public void ReturnHome()
        {
            Initialize();

            SM.Return(KeyID.BaseWorldName);

            cutSceneTriggerTimer = 0;
            triggerSeaCutscene = false;
            speedOfPan = 0;
            hasLoadedIntoWorld = false;

            lastKeySeamap = true;

            ModContent.GetInstance<EEMod>().Countur = 0;
            ModContent.GetInstance<EEMod>().frame2.Y = 0;
            ModContent.GetInstance<EEMod>().osSucksAtBedwars = 0;

            if (Main.netMode == NetmodeID.Server)
            {
                Netplay.Connection.State = 1;
            }

            EEMod.isSaving = true;
        }

        public override void clientClone(ModPlayer clientClone) { }

        public override void OnEnterWorld(Player player)
        {
            if (prevKey == KeyID.Sea && !hasLoadedIntoWorld)
            {
                hasLoadedIntoWorld = true;
                if(lastKeySeamap) player.position = (new Vector2((int)shipCoords.X - 2 + 7 + 12, (int)shipCoords.Y - 18 - 2 + 25) * 16);

                cutSceneTriggerTimer = 0;
                triggerSeaCutscene = false;
                speedOfPan = 0;

                lastKeySeamap = false;

                Main.screenPosition = player.Center - new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            }

            EEMod.isSaving = false;

            Main.time = time;
            Main.dayTime = dayTime;
        }

        public void UpdateCutscenesAndTempShaders()
        {
            Filters.Scene[SeaTransShader].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (!Filters.Scene[SeaTransShader].IsActive())
            {
                Filters.Scene.Activate(SeaTransShader, Player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }

            if (!triggerSeaCutscene)
            {
                if (Filters.Scene[SeaTransShader].IsActive())
                {
                    Filters.Scene.Deactivate(SeaTransShader);
                }
            }

            if (cutSceneTriggerTimer >= 500)
            {
                EnterSeamap();
            }
        }

        public double time;
        public bool dayTime;

        public void EnterSeamap()
        {
            time = Main.time;
            dayTime = Main.dayTime;

            Initialize();

            prevKey = KeyID.BaseWorldName;

            if (Main.netMode == NetmodeID.Server)
            {
                //Netplay.Clients[0].State = 1;
            }

            Player.GetModPlayer<EEPlayer>().seamapUpdateCount = 0;

            SubworldManager.EnterSubworld<Sea>();

            EEMod.isSaving = true;

            cutSceneTriggerTimer = 0;
        }
    }
}