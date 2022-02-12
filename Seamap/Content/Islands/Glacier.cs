using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.Core;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EEMod.Seamap.Content.Islands
{
    public class Glacier : Island
    {
        public override string name => "Glacier";
        public override int framecount => 1;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/Content/Islands/Glacier", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public Glacier(Vector2 pos): base(pos)
        {
            
        }
    }
}
