using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using ReLogic.Graphics;
using System.Diagnostics;
using Terraria.Audio;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using System.Runtime.CompilerServices;
using EndlessEscapade.Subworlds;
using EndlessEscapade.Content.Seamap.Islands;

namespace EndlessEscapade.Common.Seamap
{
    public class SeamapPlayer : ModPlayer
    {
        public bool importantCutscene;

        public int timerForCutscene;
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public float subTextAlpha;
        public bool noU;
        public int coralReefTrans;
        public int seamapUpdateCount;

        public bool IncreaseStarFall;

        public string prevKey = "Main";

        public bool hasLoadedIntoWorld;

        public Vector2 myLastBoatPos;

        public bool lastKeySeamap;

        public float quickOpeningFloat = 60;

        public string exitingSeamapKey;

        public double time;
        public bool dayTime;

        public bool exitingSeamap = false;

        public void ReturnHome() {
            SubworldLibrary.SubworldSystem.Exit();

            hasLoadedIntoWorld = false;

            lastKeySeamap = true;

            prevKey = KeyID.Sea;

            if (Main.netMode == NetmodeID.Server) {
                Netplay.Connection.State = 1;
            }
        }

        public override void OnEnterWorld() {
            if (prevKey == KeyID.Sea && !hasLoadedIntoWorld) {
                hasLoadedIntoWorld = true;
                if (lastKeySeamap) player.position = (new Vector2((int)shipCoords.X - 2 + 7 + 12, (int)shipCoords.Y - 18 - 2 + 25) * 16);

                lastKeySeamap = false;

                Main.screenPosition = player.Center - new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            }

            Main.time = time;
            Main.dayTime = dayTime;
        }

        public void EnterSeamap() {
            time = Main.time;
            dayTime = Main.dayTime;

            seamapUpdateCount = 0;

            SubworldLibrary.SubworldSystem.Enter<Sea>();

            quickOpeningFloat = 60;

            exitingSeamap = false;
        }

        public override void PreUpdate() {
            if (!SubworldLibrary.SubworldSystem.IsActive<Sea>()) return;

            Player.position = Player.oldPosition;

            Player.position.X = (Main.maxTilesX * 16 * (2f / 3f)) + 300;
            Player.position.Y = (Main.maxTilesY * 16 * (2f / 3f)) + 300;

            Player.fallStart = (int)(Player.position.Y / 16f);

            #region Opening cutscene for seamap

            if (exitingSeamap) {
                quickOpeningFloat++;

                if (quickOpeningFloat > 60) OnExitSeamap();
            }
            else if (quickOpeningFloat > 0) {
                quickOpeningFloat--;
            }

            #endregion

            seamapUpdateCount++;

            if (seamapUpdateCount == 1)
                Seamap.InitializeSeamap();

            Seamap.UpdateSeamap();

            #region Island Interact methods
            foreach (SeamapObject obj in SeamapObjects.SeamapEntities) {
                if (obj is Island) {
                    Island island = obj as Island;

                    prevKey = KeyID.Sea;

                    Player.ClearBuff(BuffID.Cursed);
                    Player.ClearBuff(BuffID.Invisibility);

                    if (Vector2.DistanceSquared(SeamapObjects.localship.Hitbox.Center.ToVector2(), obj.Center) < (obj.width * 2f) * (obj.width * 2f) &&
                        Main.LocalPlayer.controlJump) {
                        island.Interact();
                    }
                }
            }
            #endregion
        }

        public override void SaveData(TagCompound tag) {
            tag["lastPos"] = myLastBoatPos;
        }

        public override void LoadData(TagCompound tag) {
            tag.TryGet("lastPos", out myLastBoatPos);
        }

        public void OnExitSeamap() {
            quickOpeningFloat = 0;

            switch (exitingSeamapKey) {
                case "Main":
                    ReturnHome();
                    break;
                default:
                    break;
            }
        }
    }
}