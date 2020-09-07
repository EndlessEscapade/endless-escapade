using EEMod.ID;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles.Walls
{
    public class EEGlobalWall : GlobalWall
    {
        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
        {
            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
                r = g = b = Main.LocalPlayer.GetModPlayer<EEPlayer>().brightness;
        }
    }
}