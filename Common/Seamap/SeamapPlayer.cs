using EndlessEscapade.Content.Seamap.Islands;
using EndlessEscapade.Subworlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EndlessEscapade.Common.Seamap;

public class SeamapPlayer : ModPlayer
{
    public static bool isSaving;
    public bool arrowFlag = false;
    public int coralReefTrans;
    public bool dayTime;

    public bool exitingSeamap;

    public string exitingSeamapKey;

    public bool hasLoadedIntoWorld;
    public bool importantCutscene;

    public bool IncreaseStarFall;

    public bool lastKeySeamap;

    public Vector2 myLastBoatPos;
    public bool noU;

    public string prevKey = "Main";

    public float quickOpeningFloat = 60;
    public int seamapUpdateCount;
    public float subTextAlpha;

    public double time;

    public int timerForCutscene;
    public float titleText;
    public float titleText2;

    public void ReturnHome() {
        SubworldSystem.Exit();

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
            //if (lastKeySeamap) Main.LocalPlayer.position = (new Vector2((int)shipCoords.X - 2 + 7 + 12, (int)shipCoords.Y - 18 - 2 + 25) * 16);

            lastKeySeamap = false;

            Main.screenPosition = Main.LocalPlayer.Center - new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
        }

        Main.time = time;
        Main.dayTime = dayTime;
    }

    public void EnterSeamap() {
        time = Main.time;
        dayTime = Main.dayTime;

        seamapUpdateCount = 0;

        SubworldSystem.Enter<Sea>();

        quickOpeningFloat = 60;

        exitingSeamap = false;
    }

    public override void PreUpdate() {
        if (!SubworldSystem.IsActive<Sea>()) {
            return;
        }

        Player.position = Player.oldPosition;

        Player.position.X = Main.maxTilesX * 16 * (2f / 3f) + 300;
        Player.position.Y = Main.maxTilesY * 16 * (2f / 3f) + 300;

        Player.fallStart = (int)(Player.position.Y / 16f);

        #region Opening cutscene for seamap

        if (exitingSeamap) {
            quickOpeningFloat++;

            if (quickOpeningFloat > 60) {
                OnExitSeamap();
            }
        }
        else if (quickOpeningFloat > 0) {
            quickOpeningFloat--;
        }

        #endregion

        seamapUpdateCount++;

        if (seamapUpdateCount == 1) {
            Seamap.InitializeSeamap();
        }

        Seamap.UpdateSeamap();

        #region Island Interact methods

        foreach (var obj in SeamapObjects.SeamapEntities) {
            if (obj is Island) {
                var island = obj as Island;

                prevKey = KeyID.Sea;

                Player.ClearBuff(BuffID.Cursed);
                Player.ClearBuff(BuffID.Invisibility);

                if (Vector2.DistanceSquared(SeamapObjects.localship.Hitbox.Center.ToVector2(), obj.Center) < obj.width * 2f * (obj.width * 2f) &&
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
        }
    }
}
