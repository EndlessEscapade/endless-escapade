using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace EndlessEscapade.Common.Seamap;

public partial class Seamap
{
    public static float windRot;
    public static Vector2 windVector;

    public static bool firstOpened = true;

    public static void UpdateSeamap() {
        for (var i = 0; i < SeamapObjects.SeamapEntities.Length; i++) {
            if (SeamapObjects.SeamapEntities[i] != null) {
                SeamapObjects.SeamapEntities[i].Update();
                //SeamapObjects.SeamapEntities[i].UpdateComponents();
            }
        }

        permaWindVector += windVector;

        windRot += (float)Math.Sin(Main.LocalPlayer.GetModPlayer<SeamapPlayer>().seamapUpdateCount / 1600f) * Main.rand.NextFloat(1f) / 120f;

        windVector = Vector2.UnitY.RotatedBy(windRot);
        windVector.Y *= 0.6f;

        Main.LocalPlayer.cursorItemIconEnabled = false;
    }

    public static void InitializeSeamap() {
        SeamapObjects.InitObjects(new Vector2(seamapWidth - 450, seamapWidth - 500));

        Main.LocalPlayer.GetModPlayer<SeamapPlayer>().exitingSeamap = false;
        Main.LocalPlayer.GetModPlayer<SeamapPlayer>().quickOpeningFloat = 5f;

        Main.LocalPlayer.GetModPlayer<SeamapPlayer>().myLastBoatPos = Vector2.Zero;

        //SeamapObjects.NewSeamapObject(new MainIsland(new Vector2(seamapWidth - 402 - 30, seamapHeight - 118 - 200 - 30)));
    }
}
