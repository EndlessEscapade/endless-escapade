using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapAssets
{
    public class CoralReefsIsland : Island
    {
        public override string name => "Coral Reefs";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.GetTexture("EEMod/Seamap/SeamapAssets/CoralReefsIsland");

        public override IslandID id => IslandID.CoralReefs;

        public CoralReefsIsland(Vector2 pos): base(pos)
        {
            
        }
    }
}
