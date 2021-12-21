using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using EEMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EEMod.Seamap.SeamapAssets
{
    public class GoblinFort : Island
    {
        public override string name => "Goblin Fort";
        public override int framecount => 1;
        public override int framespid => 10;
        public override bool cancollide => true;

        public override Texture2D islandTex => ModContent.Request<Texture2D>("EEMod/Seamap/SeamapAssets/GoblinFort", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

        public GoblinFort(Vector2 pos): base(pos)
        {
            
        }

        public override void Interact()
        {


            base.Interact();
        }
    }
}