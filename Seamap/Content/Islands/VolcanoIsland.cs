using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Seamap.Content
{
    public class VolcanoIsland : Island
    {
        public override string name => "Volcano";
        public override int framecount => 16;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/VolcanoIsland", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public VolcanoIsland(Vector2 pos): base(pos)
        {
            
        }
    }
}
