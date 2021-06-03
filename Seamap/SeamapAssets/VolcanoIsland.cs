using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapAssets
{
    public class VolcanoIsland : Island
    {
        public override string name => "Volcano";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.GetTexture("EEMod/Seamap/SeamapAssets/VolcanoIsland");

        public override IslandID id => IslandID.VolcanoIsland;

        public VolcanoIsland(Vector2 pos): base(pos)
        {
            
        }
    }
}
