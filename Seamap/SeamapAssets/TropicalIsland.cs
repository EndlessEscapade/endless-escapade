using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapAssets
{
    public class TropicalIsland1 : Island
    {
        public override string name => "Tropical Island";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/TropicalIsland").Value;

        public override IslandID id => IslandID.TropicalIsland1;

        public TropicalIsland1(Vector2 pos): base(pos)
        {
            
        }
    }

    public class TropicalIsland2 : Island
    {
        public override string name => "Tropical Island";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/TropicalIsland").Value;

        public override IslandID id => IslandID.TropicalIsland2;

        public TropicalIsland2(Vector2 pos) : base(pos)
        {

        }
    }
}
