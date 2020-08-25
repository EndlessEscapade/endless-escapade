using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Placeables;
using EEMod.ID;
using EEMod;

namespace EEMod.Tiles.Walls
{
    public class EEGlobalWall : GlobalWall
    {
        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
        {
            Main.NewText(EEWorld.EEWorld.instance.brightness);
            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
                r = g = b = EEWorld.EEWorld.instance.brightness;
        }
    }
}
