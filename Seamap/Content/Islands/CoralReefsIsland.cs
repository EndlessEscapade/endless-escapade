using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.Content.Islands
{
    public class CoralReefsIsland : Island
    {
        public override string name => "Coral Reefs";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/CoralReefsIsland", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public CoralReefsIsland(Vector2 pos): base(pos)
        {
            
        }
    }
}
