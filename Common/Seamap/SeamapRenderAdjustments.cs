using EndlessEscapade.Subworlds;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Seamap;

internal class SeamapRenderAdjustments : ILoadable
{
    void ILoadable.Load(Mod mod) {
        On_Main.DrawProjectiles += static (orig, self) => {
            if (!Main.dedServ) {
                if (!SubworldSystem.IsActive<Sea>()) { }
                else {
                    Seamap.Render();
                }
            }
        };

        On_Main.DoDraw_UpdateCameraPosition += SeamapScreenAdjust;
    }

    void ILoadable.Unload() { }

    public void SeamapScreenAdjust(On_Main.orig_DoDraw_UpdateCameraPosition orig) {
        orig();

        if (SubworldSystem.IsActive<Sea>() && SeamapObjects.localship != null) {
            Main.screenPosition = SeamapObjects.localship.Center + new Vector2(-Main.screenWidth / 2f, -Main.screenHeight / 2f);

            ClampScreenPositionToWorld(Seamap.seamapWidth, Seamap.seamapHeight - 200);
        }
    }

    private static void ClampScreenPositionToWorld(int maxRight, int maxBottom) {
        var vector = new Vector2(0, 0) - Main.GameViewMatrix.Translation;
        var vector2 = new Vector2(maxRight - Main.screenWidth / Main.GameViewMatrix.Zoom.X, maxBottom - Main.screenHeight / Main.GameViewMatrix.Zoom.Y) -
                      Main.GameViewMatrix.Translation;

        vector = Utils.Round(vector);
        vector2 = Utils.Round(vector2);

        Main.screenPosition = Vector2.Clamp(Main.screenPosition, vector, vector2);
    }
}
