using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.SeamapAssets
{
    public class MainIsland : Island
    {
        public override string name => "Main Island";
        public override int framecount => 1;
        public override int framespid => 0;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/MainIsland").Value;

        public override IslandID id => IslandID.MainIsland;

        public MainIsland(Vector2 pos): base(pos)
        {
            
        }
    }
}
