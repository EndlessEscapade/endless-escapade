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
using EEMod.Seamap.SeamapAssets;
using EEMod.Seamap.SeamapContent;
using EEMod.Autoloading;
using EEMod.Systems.Subworlds.EESubworlds;
using System.Diagnostics;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        public List<SeagullsClass> seagulls = new List<SeagullsClass>();

        public int timerForCutscene;
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public float subTextAlpha;
        public bool noU;
        public bool triggerSeaCutscene;
        public int cutSceneTriggerTimer;
        public float cutSceneTriggerTimer3;
        public int coralReefTrans;
        public int seamapUpdateCount;
        public Vector2 position;
        public Vector2 velocity;

        public bool IncreaseStarFall;

        public static string prevKey = "Main";

        public string baseWorldName;

        public void ReturnHome()
        {
            Initialize();

            SM.Return(KeyID.BaseWorldName);
        }

        public override void clientClone(ModPlayer clientClone) { }

        public Vector2 EEPosition;

        public static void InitializeSeamap()
        {
            Seamap.SeamapContent.Seamap.InitializeSeamap();
        }

        public void UpdateCutscenesAndTempShaders()
        {
            //Filters.Scene[RippleShader].GetShader().UseOpacity(timerForCutscene);
            //if (Main.netMode != NetmodeID.Server && !Filters.Scene[RippleShader].IsActive())
            //{
            //    Filters.Scene.Activate(RippleShader, Player.Center).GetShader().UseOpacity(timerForCutscene);
            //}
            //if (!godMode)
            //{
            //    if (Main.netMode != NetmodeID.Server && Filters.Scene[RippleShader].IsActive())
            //    {
            //        Filters.Scene.Deactivate(RippleShader);
            //    }
            //}
            Filters.Scene[SeaTransShader].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[SeaTransShader].IsActive())
            {
                Filters.Scene.Activate(SeaTransShader, Player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            if (!triggerSeaCutscene)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[SeaTransShader].IsActive())
                {
                    Filters.Scene.Deactivate(SeaTransShader);
                }
            }
            if (timerForCutscene >= 1400)
            {
                Initialize();
                prevKey = KeyID.BaseWorldName;
                SubworldManager.EnterSubworld<CoralReefs>();
            }
            if (cutSceneTriggerTimer >= 500)
            {
                Initialize();
                prevKey = KeyID.BaseWorldName;
                SubworldManager.EnterSubworld<Sea>();
            }
        }
    }

    /*public partial class EEMod : Mod
    {
        public static bool IsPlayerLocalServerOwner(int whoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                RemoteClient client = Netplay.Clients[i];
                if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
                {
                    return true;
                }
            }
            return false;
        }
    }*/
        }